
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool contactoSuelo = false;
    private float speed = 0.5f;
    public Rigidbody2D rigidbody2DPlayer;
    //Le ponemos public a la bala para que se pueda arrastras y asociar desde la interface de Unity
    public GameObject bulletPrefab;
    private Animator animatorPlayer;
    public AudioSource audioSourceSalto;
    //public AudioSource audioSourceCaida;
    public AudioSource audioSourceShot;
    public AudioSource audioSourceCoin;

    //Estas 2 variables son utilizadas en el sceneController
    public bool nextLevel = false;
    public bool perdiste = false;
    public bool addPoints = false;
    //delTime devuelve el intervalo en segundos desde el último frame, así que se utiliza para sincronizar
    float deltaTime;
    bool canMoveRight = false;
    bool canMoveLeft = false;



   
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
        if (Input.GetKey(KeyCode.LeftArrow) || canMoveLeft)
        {
            //para desplazarlo a la izquierda le sumamos un vector que el x tenga una unidad menos
            transform.position += new Vector3(-1.0f * speed * Time.deltaTime, 0, 0);
            //Para invertirlo y que mire el sprite a la izquierda creamos un nuevo vector que la x en negativo
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            //Para la animación utilizamos la variable velocity del compnente animator
            animatorPlayer.SetFloat("velocity", 1f);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            animatorPlayer.SetFloat("velocity", 0); ;
        
        if (Input.GetKey(KeyCode.RightArrow) || canMoveRight)
        {
            transform.position += new Vector3(1.0f * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //Como queremos que se mueva ponemos la variable velocity del animator a 1
            animatorPlayer.SetFloat("velocity", 1f);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
            animatorPlayer.SetFloat("velocity", 0);

        //Al presionar la tecla hacia arriba saltará
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveJumpWithRigidBody();

        //Si se pulsa la tecla espacio disparamos
        if (Input.GetKeyDown(KeyCode.Space))
            CreateShot();

       
    }

    public void MoveLeftWithTransform()
    {
        canMoveLeft = true;
    }
    public void NotMoveLeftWithTransform()
    {
        //paramos la animación
        animatorPlayer.SetFloat("velocity", 0);
        canMoveLeft = false;
    }
    public void MoveRightWithTransform() { canMoveRight = true; }
    
    public void NotMoveRightWithTransform()
    {
        //paramos la animación
        animatorPlayer.SetFloat("velocity", 0);
        canMoveRight = false;
    }

    public void CreateShot()
    {
        audioSourceShot.Play();
        //La función Instantiate coge un prefab y lo duplica en algún sitio del mundo, con nuestra posivión, uaternion.identity le está diciendo con rotación 0
        Vector3 vector2Direction;
        if (transform.localScale.x == 1.0f) vector2Direction = Vector2.right;
        else vector2Direction = Vector2.left;

        GameObject bullet = Instantiate(bulletPrefab, transform.position + vector2Direction * 0.15f, (Quaternion.identity));
        bullet.GetComponent<BulletController>().SetDirection(vector2Direction);
    }

    
    private void collider()
    {
        //Chekeo colision que se ha caido el player
        /*if (transform.position.y < -1)
        {
            audioSourceCaida.Play();
            PlayerMuere();
        }*/

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
    public void MoveJumpWithRigidBody()
    {
        if (contactoSuelo)
        {
            audioSourceSalto.Play();
            //Aplicamos una fuerza al rigidBody cada vez que se pulse el espacio
            //También es posible ponerlo así: rigidbody2DPlayer.AddForce(Vector2.up * 200f);
            rigidbody2DPlayer.AddForce(new Vector2(0, 200f));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") contactoSuelo = true;
        //Si el player colisiona con un enemigo
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            PlayerMuere();
        }
       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") contactoSuelo = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Next_level")
        {
            Debug.Log("player va a cambiar de nivel");
            nextLevel = true;
        }
        if (collision.gameObject.tag == "Coin")
        {
            //destruimos la moneda
            Destroy(collision.gameObject);
            //Le sumamos puntos al score
            addPoints = true;
            audioSourceCoin.Play();
        }
    }


    private void PlayerMuere() {
        //transform.position = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, transform.position.z);
        
        //Con esto conseguimos que el player se congele
        rigidbody2DPlayer.constraints = RigidbodyConstraints2D.FreezeAll;
        //El perdiste será gestionado en SceneController
        perdiste = true;

    }

    public void SetPosition(Vector3 vector3)
    {
        gameObject.transform.position = vector3;
    }

    public Vector3 getTransform()
    {
        Vector3 vector3 = gameObject.transform.position;
        return vector3;
    }

}
