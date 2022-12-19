using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //Mover a Bola
    //Pontuação
    //Turnos
    public GameObject strike;
    public GameObject bola;
    bool flag = true;
    int score = 0;
    int turnoContador = 0;
    public GameObject [] pinos;
    int scoreReset = 0 ;
    Vector3 [] positions;
    public GameObject pinoPrefab;
    int contador = 0;
    public Text scoreUI;
    void Start() 
    {
         
        pinos=GameObject.FindGameObjectsWithTag("Pino");
        positions = new Vector3[pinos.Length];

        for(int i = 0; i < pinos.Length; i++) {
            positions[i]= new Vector3(pinos[i].transform.position.x,pinos[i].transform.position.y,pinos[i].transform.position.z);
        }
    }
    void Update() 
    {
        MoveBall();  

       

        if(Input.GetKeyDown(KeyCode.Space) || bola.transform.position.z>1.5)
        {
            ContadorPino();
            turnoContador ++;
            ResetBall();
            if(scoreReset>=10){
                Invoke("ResetPinos",1);
            }
        }
        
    }

    void MoveBall ()
    {
        Vector3 position= bola.transform.position;
        position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime;
        position.x = Mathf.Clamp(position.x,-0.525f,0.525f);
        bola.transform.position=position;

    }

    void ContadorPino()
    {
        for(int i = 0; i<pinos.Length; i++)
        {   
            if (pinos[i] && Vector3.Dot(Vector3.up, pinos[i].transform.up)<0.9f ){
                score++;
                scoreReset++;
                Destroy(pinos[i]);
            }
        }
        scoreUI.text = score.ToString();
        
        
         

    }
    void ResetBall() 
    {         
        bola.transform.position = new Vector3(0,0.10915f,-8.65f);
        bola.GetComponent<Rigidbody>().velocity =  Vector3.zero;
        bola.GetComponent<Rigidbody>().angularVelocity =  Vector3.zero;
        bola.transform.rotation = Quaternion.identity;
    }
    void ResetPinos(){

        for(int i = 0; i < pinos.Length; i++) {
            GameObject novoPino =Instantiate(pinoPrefab);
            novoPino.transform.position=positions[i];
            pinos[i]=novoPino;
            
            
        }  
        scoreReset=0;
    }
   

}
