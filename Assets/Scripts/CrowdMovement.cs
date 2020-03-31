using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    public Material infectedMat;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        // Random "movement" for crowd member to have them moving all over the space
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-800, 800), Random.Range(-800, 800)) * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // using arbitrary scalar here
        gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.forward * 10000.0f * Time.deltaTime);
        

        if (collision.gameObject.tag == "Infected" && gameObject.tag != "Infected")
        {
            gameObject.GetComponent<Renderer>().material = infectedMat;
            gameObject.tag = "Infected";
        }
    }
}
