using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCamera : MonoBehaviour
{

    public GameObject bola;
    private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - bola.transform.position;
    }

    // Update is called once per frame
   void LateUpdate()
    {
        
        transform.position = new Vector3(transform.position.x,transform.position.y,bola.transform.position.z + offset.z) ;
    }
}
