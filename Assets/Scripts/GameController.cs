using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceLost;
    public PlayerController playerController;
    public GameObject[] levelPrefab;
    //Esto es para poder detectar cuando hay que sumar puntos
    public BulletController bulletController;
    public int indiceDeNiveles;
    private GameObject objetoNivel;
    public Camera camera;
    public int score;
    public Text textoDeJuego;
    public bool yaSeEscucho = false;
    private int lives;

    public float playerPositionX;
    public float playerPositionY;


    // Start is called before the first frame update
    void Start()
    {
        audioSourceMusic.Play();
        indiceDeNiveles = 0;
        lives = 10;
        playerController.SetPosition(new Vector3 (-4f,0,0));
        //Creamos el nivel 0
        objetoNivel = Instantiate(levelPrefab[indiceDeNiveles]);
        //objetoNivel.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //Controlando la cámara
        playerPositionX = playerController.transform.position.x;
        playerPositionY = playerController.transform.position.y;
        //Mathf.Clamp(playerPositionX,42f , -3.80f);
        //Mathf.Clamp(playerPositionY, 0, 10);
        if (indiceDeNiveles == 0 )
        {
            if (playerPositionX > -3.80f && playerPositionX < 42f)
            {
                camera.transform.position = new Vector3(playerPositionX, camera.transform.position.y, camera.transform.position.z);
            }
            //Chekeo colision que se ha caido el player
            if (playerController.transform.position.y < -1f)
            {
               PlayerMuere();
            }

        }

        textoDeJuego.text = "Level " + indiceDeNiveles + 1 + "\nScore: " + score + "\nVidas: " + lives;

        if (playerController.perdiste)
        {
            
            textoDeJuego.text = "Level " + indiceDeNiveles + 1 +  "\nScore: " + score + "\nVidas: " + lives;

            if (Input.GetKeyDown("r"))
            {
                score = 0;
            }



            PlayerMuere();
        }




        if (playerController.nextLevel)
        {
            Debug.Log("Cambiando de nivel");
            textoDeJuego.text = "NIvel " + indiceDeNiveles + 1 +
                "\nScore: " + score +
                "Completaste el nivel";
            //Si hemos llegado al final
            if(indiceDeNiveles== levelPrefab.Length - 1)
            {
                SceneManager.LoadScene("FinalScene");
            }
            else
            {
                Destroy(objetoNivel);
                indiceDeNiveles += 1;
                objetoNivel = Instantiate(levelPrefab[indiceDeNiveles]);
                objetoNivel.transform.SetParent(this.transform);
                playerController.nextLevel = false;
            }

        }


        if (bulletController.addPoints)
        {
            addPoints(10);
            updateText();
            bulletController.addPoints = false;
        }


        if (playerController.addPoints)
        {
            addPoints(10);
            updateText();
            playerController.addPoints = false;
        }
    }


    private void PlayerMuere()
    {
        Vector3 vector3;
        //Con esto conseguimos que el player se descongele ya que al morir se congelaba
        playerController.rigidbody2DPlayer.constraints = RigidbodyConstraints2D.None;
        //Con esto conseguimos que el player se congele la rotación z
        playerController.rigidbody2DPlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Debug.Log(startPoint.transform.position.x+"   "+ startPoint.transform.position.y);
        //Volvemos al jugador al punto de Inicio
 
        if (indiceDeNiveles==0)
        {
            vector3=new Vector3(-3.7f, 0, 0);
            playerController.transform.position = vector3;
            //ponemos bien la cámara
            //camera.transform.position = vector3;
        }
        

        //transform.position = new Vector3(playerController.startPoint.transform.position.x, playerController.startPoint.transform.position.y, playerController.gameObject.transform.position.z);
        //Destruimos el nivel y lo volvemos a crear
        Destroy(objetoNivel);
        objetoNivel = Instantiate(levelPrefab[indiceDeNiveles]);
        objetoNivel.transform.SetParent(this.transform);
        //Volvemos a colocar la variable perdiste bien
        playerController.perdiste = false;
        yaSeEscucho = false;
        lives -= 1;
        //Posicionamos al player 
        //playerController.setTransform();
    }

    public void addPoints(int points)
    {
        score += points;
        playerController.addPoints = false;
        bulletController.addPoints = false;
    }

    private void updateText()
    {
        textoDeJuego.text = "Level " + indiceDeNiveles + 1 +
                    "\nScore: " + score +
                    "\nVidas: " + lives;
    }
}
