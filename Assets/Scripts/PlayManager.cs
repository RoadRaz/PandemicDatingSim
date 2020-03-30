using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    // Start is called before the first frame update
    enum DialogueState {Off, Writing, Done}
    DialogueState currentDialogueState = DialogueState.Off;
    public DialogueDistributor theDialogue;
    public ChoiceScript theChoices;

    [HideInInspector]
    public int currentTextIndex = 0;

    int currentWritingIndex = 0;
    float writingDelay = 0.01f;
    float lastCharacterWrite = 0;

    GameObject dialogue;
    GameObject romanceText;
    GameObject statusText;
    
    string[] textToDisplay;

    void Start()
    {
        dialogue = GameObject.Find("DialogueText");
        romanceText = GameObject.Find("Romance");
        statusText = GameObject.Find("Status");
        textToDisplay = new string[theDialogue.masterText.GetLength(1)];
        for (int i = 0; i < theDialogue.masterText.GetLength(1); i++)
        {
            textToDisplay[i] = theDialogue.masterText[theDialogue.scenarioID, i];
        }
        currentDialogueState = DialogueState.Writing;
    }
    void SetUIVariables()
    {
        romanceText.GetComponent<Text>().text = "Romance: " + this.gameObject.GetComponent<DialogueDistributor>().romanceValue;
        if (this.gameObject.GetComponent<DialogueDistributor>().statusValue == DialogueDistributor.PartnerStatus.Normal)
        {
            statusText.GetComponent<Text>().text = "Status: ---- ";
        }
        else
        {
            statusText.GetComponent<Text>().text = "Status: " + this.gameObject.GetComponent<DialogueDistributor>().statusValue;
        }
    }
    // Update is called once per frame
    void Update()
    {
        SetUIVariables();
        // Hitting enter either skips the gradual typing of text or goes to the next dialogue
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentDialogueState == DialogueState.Writing)
            {
                // Set dialogue to full current text
                dialogue.GetComponent<Text>().text = textToDisplay[currentTextIndex];
                currentWritingIndex = textToDisplay[currentTextIndex].Length;
                currentDialogueState = DialogueState.Done;

                // Display the choices when at a choice point
                if (theDialogue.thereAreChoices[theDialogue.scenarioID, currentTextIndex])
                {
                    theChoices.PrepareChoice(theDialogue.masterChoices[theDialogue.scenarioID, 0],
                        theDialogue.masterChoices[theDialogue.scenarioID, 1],
                        theDialogue.masterChoices[theDialogue.scenarioID, 2]);
                }
            }
            else if (currentDialogueState == DialogueState.Done && !theDialogue.thereAreChoices[theDialogue.scenarioID, currentTextIndex])
            {
                // Advance to the next bit of text, unless currently on a choice point
                currentTextIndex++;
                currentWritingIndex = 0;
                dialogue.GetComponent<Text>().text = "";
                currentDialogueState = DialogueState.Writing;
            }

        }

        // This types text gradually
        if (currentDialogueState == DialogueState.Writing && Time.timeSinceLevelLoad - lastCharacterWrite > writingDelay) 
        {
            if (currentTextIndex < textToDisplay.Length) 
            {
                if (currentWritingIndex < textToDisplay[currentTextIndex].Length)
                {
                    dialogue.GetComponent<Text>().text += textToDisplay[currentTextIndex][currentWritingIndex];
                    currentWritingIndex++;
                }
                else
                {
                    currentDialogueState = DialogueState.Done;

                // Display the choices when at a choice point
                if (theDialogue.thereAreChoices[theDialogue.scenarioID, currentTextIndex])
                {
                    theChoices.PrepareChoice(theDialogue.masterChoices[theDialogue.scenarioID, 0],
                        theDialogue.masterChoices[theDialogue.scenarioID, 1],
                        theDialogue.masterChoices[theDialogue.scenarioID, 2]);
                }
                }
            }
            lastCharacterWrite = Time.timeSinceLevelLoad;
        }

        // Choice code is to be updated
        if (gameObject.GetComponent<ChoiceScript>().choiceCompleted)
        {
            if (currentDialogueState == DialogueState.Writing)
            {
                dialogue.GetComponent<Text>().text = textToDisplay[currentTextIndex];
                currentDialogueState = DialogueState.Done;
            }
            else if (currentDialogueState == DialogueState.Done)
            {
                //go to next text field (or next state)
                currentTextIndex++;
                currentWritingIndex = 0;
                dialogue.GetComponent<Text>().text = "";
                lastCharacterWrite = 0;
                currentDialogueState = DialogueState.Writing;


                gameObject.GetComponent<ChoiceScript>().choiceCompleted = false;
            }

        }

    }
}
