using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text timerText;

    public GameObject player;

    float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time Left: " + (int)timeLeft;

        if(timeLeft < 0)
        {
            // Load scene here to go back to dialogue 
            // SceneManager.LoadScene()

            if(player.tag == "Player")
            {
                PlayerPrefs.SetInt("MiniGame Result", 1);
            }
            else
            {
                PlayerPrefs.SetInt("MiniGame Result", 0);
            }
        }
    }
}
