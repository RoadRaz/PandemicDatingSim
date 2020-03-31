using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour
{
    public GameObject choice01;
    public GameObject choice02;
    public GameObject choice03;
    [HideInInspector]
    public int choiceMade = 0;
    [HideInInspector]
    public bool choiceCompleted = false;

    public void Start()
    {
        choice01.SetActive(false);
        choice02.SetActive(false);
        choice03.SetActive(false);
    }

    public void PrepareChoice(string inputChoice1, string inputChoice2, string inputChoice3)
    {
        if (inputChoice1 != "")
        {
            choice01.SetActive(true);
            choice01.GetComponentsInChildren<Text>()[0].text = inputChoice1;
        }
        if (inputChoice2 != "")
        {
            choice02.SetActive(true);
            choice02.GetComponentsInChildren<Text>()[0].text = inputChoice2;
        }
        if (inputChoice3 != "")
        {
            choice03.SetActive(true);
            choice03.GetComponentsInChildren<Text>()[0].text = inputChoice3;
        }
    }

    // Button 1 invokes this
    public void ChoiceOption1()
    {
        choiceMade = 1;
        choiceCompleted = true;
    }

    // Button 2 invokes this
    public void ChoiceOption2()
    {
        choiceMade = 2;
        choiceCompleted = true;
    }

    // Button 3 invokes this
    public void ChoiceOption3()
    {
        choiceMade = 3;
        choiceCompleted = true;
    }

    public void ResetChoice()
    {
        choice01.GetComponentsInChildren<Text>()[0].text = "";
        choice02.GetComponentsInChildren<Text>()[0].text = "";
        choice03.GetComponentsInChildren<Text>()[0].text = "";

        choice01.SetActive(false);
        choice02.SetActive(false);
        choice03.SetActive(false);

        choiceCompleted = false;
    }
}