using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float x_minimo;
    public float x_maximo;
    public float y_minimo;
    public float y_maximo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float playerPositionX = player.transform.position.x;
        float playerPositionY = player.transform.position.y;
        //Mathf.Clamp(playerPositionX, -17.33f, 30f);
        Mathf.Clamp(playerPositionX, x_minimo, x_maximo);
        transform.position = new Vector3(playerPositionX,transform.position.y, transform.position.z);
    }
}
