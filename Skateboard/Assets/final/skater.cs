using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skater : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }
    public float moveSpeed = 5f;
    public float turnSpeed = 540f;


    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis ("Horizontal");
        float v = Input.GetAxis ("Vertical");
 
        //transform.Translate (0f, 0f, h * moveSpeed * Time.deltaTime);
 
     
        //Turn
        transform.Rotate (0f, h * turnSpeed * Time.deltaTime, 0f);

        transform.Translate (0f, 0f, v * moveSpeed * Time.deltaTime);



    }
 
}
