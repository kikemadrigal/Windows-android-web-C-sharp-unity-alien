using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rigidbody2DBullet;
    private float speed=2;
    private Vector2 vector2Direction;
    public bool addPoints = false;
    //El audio del disparo ya lo tiene asignado el player cuando presiona espacio
    //public AudioSource audioDisparo;
    // Start is called before the first frame update
    void Start()
    {
        //Necesitamos el rigidBody para generar una fuerza hacia arriba para el salto
        rigidbody2DBullet = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rigidbody2DBullet.velocity = Vector2.right * speed;
        rigidbody2DBullet.velocity = vector2Direction * speed;
    }

    public void SetDirection(Vector2 vector2Direction)
    {
        this.vector2Direction = vector2Direction;
    }

    public void DestroyBullet()
    {

        Destroy(gameObject);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Destruimos al enemigo
            Destroy(collision.gameObject);
            //Destruimos la bala
            Destroy(gameObject);
            addPoints = true;
        }
        if (collision.gameObject.tag == "Platform")
        {
            //Destruimos la bala
            Destroy(gameObject);
        }

    }
}
