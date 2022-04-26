using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class terrainGenerator : MonoBehaviour
{

    [System.Serializable]
    public struct TerrainType {
	public string name;
	public float heightType;
	public Color colour;
    }
	public TerrainType[] regions;


    [System.Serializable]
    public struct clcTerrain {
	public string name;
	public float clcType;
	public Color colour;
    }
    public clcTerrain[] clcRegions;
    public clcTerrain[] clcSimplifiedRegions;
    public Texture2D test;
/////////////////////////////////////////////////////////// GENERATION DU TERRAIN EN FONCTION DES HAUTEURS///////////////////////////////////////////////////////////
    public int width = 2049; //x-axis of the terrain
    public int length = 2049; //z-axis
    public int depth = 250; //y-axis
    public string filename;
    public string CLCfilename;
    public ForestGenerator forestGenerator;
    private float max = 0;
    // Start is called before the first frame update
    void Start()
    {
        string CLCmapType= "test";
        int terrainType = 0;
        Terrain terrain = GetComponent<Terrain>(); 
       // String mapType = filename.Substring(0,3);
        if(CLCfilename.Length >= 3){
            CLCmapType = CLCfilename.Substring(0,3);
            if(CLCmapType == "CLC"){
                terrainType = 1; 
            }    
        }
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        GenerateSplat(terrain.terrainData, terrainType, terrain);
        
    }

    // Update is called once per frame
      private void Update()
    {

    }

    TerrainData GenerateTerrain (TerrainData terrainData)
    {

        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, length);
        float[,] HeightsList = GenerateHeights(filename);
        for(int i = 0; i < width; i++){
            for(int j = 0; j <length; j++){
                if(HeightsList[i,j]>max){
                    max = HeightsList[i,j];
                }
            }
        }
        for(int i = 0; i < width; i++){
            for(int j = 0; j < length; j++){
            HeightsList[i,j] = HeightsList[i,j] /max;
            }
        }
        terrainData.SetHeights(0, 0, HeightsList);        
        return terrainData;
    }

    float[,] GenerateHeights(String filename)
    {
    int count = 0;     
    List<int> list = new List<int>();
    byte[] fileBytes = File.ReadAllBytes("D:/Unity_Projects/Projects_directories/OpenWorld/Assets/"+filename);
 //  float[] result = new float[fileBytes.Length];  
    float[,] heights = new float[width, length];
    StringBuilder sb = new StringBuilder();

    for(int i = 0; i < fileBytes.Length; i+=2)
    {
    int byte1 = Convert.ToUInt16(fileBytes[i+1])<<8;
    int byte2 = Convert.ToUInt16(fileBytes[i]);

    list.Add(byte1 + byte2);
    }
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = list[count];
                count++;
            }
        }   
    return heights;
    }

	public void GenerateSplat(TerrainData terrainData, int terrainType, Terrain terrain) {
        
        
        SplatPrototype[] tabSplat = new SplatPrototype[1];
        Texture2D texture = new Texture2D(width, length, TextureFormat.RGBA32, true);
        tabSplat[0] = new SplatPrototype();
        tabSplat[0].texture = texture;
        tabSplat[0].tileSize = new Vector2(width, length);
        int elementType = 0;
        float[, ,] va_alphamaps = new float[width, length, tabSplat.Length];
        va_alphamaps = terrainData.GetAlphamaps(0, 0, width, length);
        Color[] colourMap = new Color[width * length];

        
        if(terrainType == 0){
        float[,] tabHeights =  GenerateHeights(filename);
            for (int y = 0; y < length; y++) {
			    for (int x = 0; x < width; x++) {
				    float currentHeight = tabHeights[x,y];
				    for (int i = 0; i < regions.Length; i++) {
					    if (currentHeight <= regions [i].heightType) {
						    colourMap [y * width + x] = regions[i].colour;
                            if(regions[i].name == "Grass"){
                            elementType = 1;
                            forestGenerator.GenerateElements(y,x,terrain,elementType);                          
                                }                 
                            if(regions[i].name == "Sand"){
                            elementType = 2;
                            forestGenerator.GenerateElements(y,x,terrain,elementType);                          
                                }                 
                            break;
					        }
                        }                      
				    }
			    }
            } 
        if(terrainType == 1){
        float[,] tabHeights =  GenerateHeights(CLCfilename);
            for (int y = 0; y < length; y++) {
			    for (int x = 0; x < width; x++) {
				    int currentHeight = (int)tabHeights[x,y];
                    currentHeight = currentHeight/100;
				    for (int i = 0; i < clcSimplifiedRegions.Length; i++) {
					    if (currentHeight == clcSimplifiedRegions [i].clcType) {
						    colourMap [y * width + x] = clcSimplifiedRegions[i].colour;
                            if(clcSimplifiedRegions[i].clcType == 3){
                           // Debug.Log(clcRegions[i].name);
                            elementType = 1;
                            forestGenerator.GenerateElements(y,x,terrain,elementType);                          
                                }                 
                            if(clcSimplifiedRegions[i].clcType == 1){
                           // Debug.Log(clcRegions[i].name);
                            elementType = 2;
                            forestGenerator.GenerateElements(y,x,terrain,elementType);                          
                                }//fonction créant les assets en fonction des couleurs                  
                            break;
					        }
                        }                      
				    }
			    }
            }
               
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, colourMap[x * width + y]);
            }
        }
        texture.Apply();
        terrainData.splatPrototypes = tabSplat;
        terrainData.SetAlphamaps(0, 0, va_alphamaps);
	}
}