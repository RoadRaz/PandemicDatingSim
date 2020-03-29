using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour
{
    public GameObject dialogueText;
    public GameObject choice01;
    public GameObject choice02;
    public GameObject choice03;
    int choiceMade = 0;
    public bool choiceCompleted = false;

    int choiceTextIndex1;
    int choiceTextIndex2;
    int choiceTextIndex3;

    public void Start()
    {
        choice01.GetComponentsInChildren<Text>()[0].text = "Likewise! What are you interested in?";
        choice02.GetComponentsInChildren<Text>()[0].text = "Fantastic! I am excited too!";
        choice03.GetComponentsInChildren<Text>()[0].text = "We will have to think of something cool to do together!";

        choiceTextIndex1 = 0;
        choiceTextIndex2 = 1;
        choiceTextIndex3 = 2;
    }

    // Button 1 invokes this
    public void ChoiceOption1()
    {
        gameObject.GetComponent<DialogueManager>().currentTextIndex = choiceTextIndex1;
        choiceMade = 1;
        choiceCompleted = true;
    }

    // Button 2 invokes this
    public void ChoiceOption2()
    {
        gameObject.GetComponent<DialogueManager>().currentTextIndex = choiceTextIndex2;
        choiceMade = 2;
        choiceCompleted = true;
    }

    // Button 3 invokes this
    public void ChoiceOption3()
    {
        gameObject.GetComponent<DialogueManager>().currentTextIndex = choiceTextIndex3;
        choiceMade = 3;
        choiceCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Do things depending on choice made

        if(choiceMade == 1)
        {
            SetChoiceText("Further option #1", "Further option #2", "Further option #3");
            // can set choice text index here as well 
        }
        if (choiceMade == 2)
        {
            SetChoiceText("Further option #1", "Further option #2", "Further option #3");
        }
        if (choiceMade == 3)
        {
            SetChoiceText("Further option #1", "Further option #2", "Further option #3");
        }
    }

    void ResetChoiceText()
    {
        choice01.GetComponentsInChildren<Text>()[0].text = "";
        choice02.GetComponentsInChildren<Text>()[0].text = "";
        choice03.GetComponentsInChildren<Text>()[0].text = "";
    }

    void SetChoiceText(string choice01, string choice02, string choice03)
    {
        this.choice01.GetComponentsInChildren<Text>()[0].text = choice01;
        this.choice02.GetComponentsInChildren<Text>()[0].text = choice02;
        this.choice03.GetComponentsInChildren<Text>()[0].text = choice03;
    }
}
