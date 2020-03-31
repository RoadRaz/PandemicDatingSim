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

    GameObject data;
    
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 30.0f;
        data = GameObject.Find("SAVEDDATA");
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
                if (data) 
                {
                    data.GetComponent<DataHolding>().lastMiniGameResult = 1;
                }
                PlayerPrefs.SetInt("MiniGame Result", 1);
            }
            else
            {
                if (data)
                {
                    data.GetComponent<DataHolding>().lastMiniGameResult = 0;
                }
                PlayerPrefs.SetInt("MiniGame Result", 0);
            }
            SceneManager.LoadScene("DavidTestScene");
        }
    }
}
