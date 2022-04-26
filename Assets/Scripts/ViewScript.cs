using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScript : MonoBehaviour
{
 private CharacterController controller;
 public GameObject camera;
 public float moveSpeed = 40f;
 private float verticalVelocity;
 private float gravity = 60f;
 private float jumpForce = 150f;
 private float rotateSpeed= 40f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();      
    }

    // Update is called once per frame
    private void LateUpdate()
    {
       
        if(controller.isGrounded)
        {
                verticalVelocity = -gravity *Time.deltaTime;
                if(Input.GetKey(KeyCode.Space))
                {
                    verticalVelocity = jumpForce;
                }
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        camera.transform.position = this.transform.position;     
        
        Vector3 moveVector = new Vector3(0,verticalVelocity,0);
        controller.Move(moveVector * Time.deltaTime);

        if(Input.GetKey(KeyCode.UpArrow))
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.DownArrow))
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.LeftArrow))
        transform.Rotate(Vector3.up,-rotateSpeed * Time.deltaTime);  

        if(Input.GetKey(KeyCode.RightArrow))
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime); 
    }
}
