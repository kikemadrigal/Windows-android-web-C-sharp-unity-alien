
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool contactoSuelo = false;
    public Rigidbody2D rigidbody2DPlayer;
    //Le ponemos public a la bala para que se pueda arrastras y asociar desde la interface de Unity
    public GameObject bulletPrefab;
    private Animator animatorPlayer;
    public AudioSource audioSourceSalto;
    public AudioSource audioSourceCaida;
    public AudioSource audioSourceShot;

    //Estas 2 variables son utilizadas en el sceneController
    public bool nextLevel = false;
    public bool perdiste = false;
    // Start is called before the first frame update
    void Start()
    {
        //Necesitamos el rigidBody para generar una fuerza hacia arriba para el salto
        rigidbody2DPlayer = GetComponent<Rigidbody2D>();
        //Para manejar las transiciones entre animaciones (que se vean los sprites mirando arriba o abajo) necesitamos acceder al animator
        animatorPlayer = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inputSystem();
        collider();
        //Debug(); 
    }
  
    private void inputSystem()
    {
        //delTime devuelve el intervalo en segundos desde el último frame, así que se utiliza para sincronizar
        float deltaTime = Time.deltaTime;
        //Solo utilizada para que vaya más rápido
        float speed = 1f;
        /***El movimiento se puede hacer por 2 formas****/
        //La 1 forma es con Input.GetAxis
        //Si pulsa en Edit->Project settings->Input, le abrirá InputMangaer, despliega Axes, despues despliega Horizontal
        //Unity guarda unas teclas relaccionadas por defecto, en Negative button pone flecha izquierda y en positive button pone flecha derecha
        //Si te fijas en Joy num permite detectar el JoyStick
        /*
        float vHorizontal = Input.GetAxis("Horizontal");
        transform.position += new Vector3(vHorizontal*deltaTime, 0, 0);
        */
        /**
         * 2 Forma con Input.GetKey
         */
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * deltaTime, 0, 0);
        }
        //Al presionar la tecla hacia arriba saltará
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (contactoSuelo)
            {
                audioSourceSalto.Play();
                //Aplicamos una fuerza al rigidBody cada vez que se pulse el espacio
                //También es posible ponerlo así: rigidbody2DPlayer.AddForce(Vector2.up * 200f);
                rigidbody2DPlayer.AddForce(new Vector2(0, 200f));
            }
        }
        //Si se pulsa la tecla espacio disparamos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSourceShot.Play();
            //La función Instantiate coge un prefab y lo duplica en algún sitio del mundo, con nuestra posivión, uaternion.identity le está diciendo con rotación 0
            Vector3 vector2Direction;
            if (transform.localScale.x == 1.0f) vector2Direction = Vector2.right;
            else vector2Direction = Vector2.left;

            GameObject bullet= Instantiate(bulletPrefab, transform.position+vector2Direction*0.01f, (Quaternion.identity));
            bullet.GetComponent<BulletController>().SetDirection(vector2Direction);

        }
            /**
            * Invirtiendo el sprite cuando cambia de dirección
            */
            float vHorizontal = Input.GetAxis("Horizontal");
        //animator.SetBool("running",vHorizontal!=0.0f );
        //Aki pongo Abs por vHorizontal da 0.01
        animatorPlayer.SetFloat("velocity",Mathf.Abs(vHorizontal));
        //Cambiamos la animación cuando vaya a la izquierda tan solo detectando si es menor qe 0 e innviertiendo el sprite
        if (vHorizontal < 0) transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
        else if (vHorizontal > 0) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void collider()
    {
        // Chekeo con los bordes del mundo
        //Clamp hace que nuestra variable no se salga de un mínimo y un máximo y así no ande más de la cuenta
        //float newX = Mathf.Clamp(transform.position.x, cameraController.x_minimo, cameraController.x_maximo);
        //float newY = Mathf.Clamp(transform.position.y, cameraController.y_minimo, cameraController.y_maximo);
        //transform.position = new Vector3(newX, transform.position.y, transform.position.z);


        //Chekeo colision que se ha caido el player
        if (transform.position.y < -1)
        {
            audioSourceCaida.Play();
            PlayerMuere();
        }

        /**
         * Exsiten 2 formas para detectar la colisión con la plataforma 
        */

        //La 1 forma es con un rayo que ira el sprite haca abajo
        //Con esto le estamos diciendo que no salte en el aire
        //Con esto pintamos una linea dentro de Unitity para que nos muestre el rayo que comprueba si está tocando el suelo
        /*
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        contactoSuelo = Physics2D.Raycast(transform.position, Vector3.down);
        */
        //La 2 forma es con los 2 métodos que viene a continuación que son Callbacks



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") contactoSuelo = true;
        if (collision.gameObject.tag == "Enemy") PlayerMuere();
        if (collision.gameObject.tag == "Next_level")
        {
            nextLevel = true;
        }
        //animatorPlayer.SetFloat("Saltando",0);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") contactoSuelo = false;
    }




    private void PlayerMuere() {
        //transform.position = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, transform.position.z);
        
        //Con esto conseguimos que el player se congele
        rigidbody2DPlayer.constraints = RigidbodyConstraints2D.FreezeAll;
        //El perdiste será gestionado en SceneController
        perdiste = true;
    }


    private void Debug()
    {
        //float x = transform.position.x;
        //float y = transform.position.y;
        //Debug.Log("x: "+x+", y: "+y);
    }
}
