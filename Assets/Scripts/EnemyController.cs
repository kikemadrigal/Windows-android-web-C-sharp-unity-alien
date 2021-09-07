using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rigidbody2DEnemy;
    private float velocidad=0.1f;
    //public AudioSource audioSourceColision;
    // Start is called before the first frame update
    void Start()
    {
        //rigidbody2DEnemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rigidbody2DEnemy.velocity = new Vector2(velocidad,0);
        transform.position -= new Vector3(velocidad * Time.deltaTime, 0, 0);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //audioSourceColision.Play();
        if (collision.tag == "Rebote")
        {
            velocidad *= -1;
        }
        //Si es una bala destruimos el enemigo
        if (collision.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //audioSourceColision.Play();
        if (collision.gameObject.tag == "Rebote")
        {
            //Debug.Log("Rebote");
            velocidad *= -1;
            //Vector3 vector3Scale = transform.localScale;
            //vector3Scale.x *= -1;
            //transform.localScale = vector3Scale;
        }
        //Si es una bala destruimos el enemigo
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }

}
