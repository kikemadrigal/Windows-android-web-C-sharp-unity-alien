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
    public float playerClampX;
    public float playerClampY;

    //
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


        if (indiceDeNiveles == 0 )
        {
            //Mathf.Clamp(playerClampX, -3.80f, 42f);
            //Mathf.Clamp(playerClampY, 0, 10);
            if (playerPositionX > -3.80f && playerPositionX < 30.26f)
            {
                camera.transform.position = new Vector3(playerPositionX, camera.transform.position.y, camera.transform.position.z);
            }
            //Chekeo colision que se ha caido el player
            if (playerController.transform.position.y < -1f)
            {
               PlayerMuere();
            }

        }
        if (indiceDeNiveles == 1)
        {
            if (playerPositionX > -3.4f && playerPositionX < 1.8F)
            {
                camera.transform.position = new Vector3(playerPositionX, playerPositionY, camera.transform.position.z);
            }
            else if (playerPositionY > -7f && playerPositionX < 7F)
            {
                camera.transform.position = new Vector3(camera.transform.position.x, playerPositionY, camera.transform.position.z);
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


    public float getNivel()
    {
        return indiceDeNiveles;
    }
    public void NextLevel()
    {
        Debug.Log("Cambiando de nivel");
        textoDeJuego.text = "NIvel " + indiceDeNiveles + 1 +
            "\nScore: " + score +
            "Completaste el nivel";
        indiceDeNiveles += 1;
        //Si hemos llegado al final
        if (indiceDeNiveles == levelPrefab.Length)
        {
            SceneManager.LoadScene("FinalScene");
        }
        else
        {
            if (indiceDeNiveles == 0)
            {
                playerController.transform.position = new Vector3(-4,0.4f,playerController.transform.position.z);
            }
            else if (indiceDeNiveles == 1)
            {
                playerController.transform.position = new Vector3(-3f, 0.5f, playerController.transform.position.z);
            }
        }
        Destroy(objetoNivel);
        objetoNivel = Instantiate(levelPrefab[indiceDeNiveles]);
    }

    public void PlayerMuere()
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
