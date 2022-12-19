using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{     
    
   /* Variável que define a força da bola */ 
   
    public float Force;

    /* Variável para guardar a referência do corpo rigído da bola */
    private Rigidbody rbg;
    private AudioSource source;
    public void Start()
    {
        /* Guarda uma referência do componente Rigidbody */
        rbg= GetComponent<Rigidbody>();
        source= GetComponent<AudioSource>();
    }

   public void OnMouseDown()
    {
       //if(Input.GetKeyDown(KeyCode.Return))
        /* Adiciona uma força na bola de acordo com a direção calculada */
        rbg.AddForce(Vector3.forward * Force);
        source.Play();
      
    }

}
