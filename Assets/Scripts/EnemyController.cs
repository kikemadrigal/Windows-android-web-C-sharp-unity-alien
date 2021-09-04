using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rigidbody2DEnemy;
    private float velocidad=1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2DEnemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2DEnemy.velocity = new Vector2(velocidad,rigidbody2DEnemy.velocity.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Rebote") {
            //Debug.Log("Rebote");
            velocidad *= -1;
            //Vector3 vector3Scale = transform.localScale;
            //vector3Scale.x *= -1;
            //transform.localScale = vector3Scale;

        }

    }

}
