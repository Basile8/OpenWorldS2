using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
//    public terrainGenerator terraingenerator;
//    public float[,] heightlist;
//    public int width = 2049; //x-axis of the terrain
    
//    public int length = 2049; //z-axis
//    public int elementspacing = 20;
    
//    public Terrain terrain;
    public Element[] elements;

    [System.Serializable]
    public class Element
    {
        public string name;
        [Range(1,10)]
        public int density;
        [Range(1,0)]
        public float size;
        public GameObject[] prefabs;
    }

    public void GenerateElements(int x, int z, Terrain terrain, int elementType)
    {
    Element element = elements[0];
    Element element2 = elements[1];
        
        if(elementType == 1){
            float randomNumber = Random.Range(0,1000);           
            if(randomNumber <= element.density){
                Vector3 position = new Vector3();
                position.x = x;
                position.z = z;            
                position.y = terrain.SampleHeight(position);
                Vector3 sizeChange = new Vector3(element.size,element.size,element.size);
                GameObject newElement = Instantiate(element.prefabs[Random.Range(0,2)]);
                newElement.transform.position = position;
                newElement.transform.Rotate(Vector3.up,Random.Range(-90,90)); 
                newElement.transform.localScale = newElement.transform.localScale - sizeChange;                 
            }
        }
        if(elementType == 2){
            float randomNumber = Random.Range(0,2000);                      
            if(randomNumber <= element2.density){
                Vector3 position = new Vector3();
                position.x = x;
                position.z = z;            
                position.y = terrain.SampleHeight(position);
                Vector3 sizeChange = new Vector3(element2.size,element2.size,element2.size); 
                GameObject newElement = Instantiate(element2.prefabs[Random.Range(0,2)]);
                newElement.transform.position = position;
                newElement.transform.localScale = newElement.transform.localScale - sizeChange; 
            //  newElement.transform.Rotate(Vector3.up,Random.Range(-20,20)); 
            }            
        }
    //    Debug.Log(elements.density[0],elements.density[1]);
    }                
}



