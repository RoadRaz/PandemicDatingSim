using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public Text status;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if(Input.GetKey(KeyCode.W))
        {
            /* Commented code is alternative movement using coordinates rather than forces */

            // transform.Translate(new Vector3(0, 3) * Time.deltaTime);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // transform.Translate(new Vector3(0, -3) * Time.deltaTime);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -100) * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.A))
        {
            // transform.Translate(new Vector3(-3, 0) * Time.deltaTime);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 0) * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            // transform.Translate(new Vector3(3, 0) * Time.deltaTime);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0) * Time.deltaTime);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // using arbitrary scalar here
        gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.forward * 10000.0f * Time.deltaTime);

        if (collision.gameObject.tag == "Infected")
        {
            gameObject.tag = "InfectedPlayer";
            status.text = "Status: Infected";
            status.color = Color.red;
        }
    }
}
