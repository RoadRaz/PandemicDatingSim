using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayManager : MonoBehaviour
{
    enum DialogueState { Off, Writing, Done }
    DialogueState currentDialogueState = DialogueState.Off;
    public enum BackAndForthState { Off, Dialogue, Consequence }
    [HideInInspector]
    public BackAndForthState currentBackAndForthState = BackAndForthState.Off;
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
    GameObject data;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GameObject.Find("DialogueText");
        romanceText = GameObject.Find("Romance");
        statusText = GameObject.Find("Status");
        data = GameObject.Find("SAVEDDATA");
        textToDisplay = new string[theDialogue.masterText.GetLength(1)];

        for (int i = 0; i < theDialogue.masterText.GetLength(1); i++)
        {
            textToDisplay[i] = theDialogue.masterText[theDialogue.scenarioID, i];
        }
        currentDialogueState = DialogueState.Writing;
        currentBackAndForthState = BackAndForthState.Dialogue;

        if (data.GetComponent<DataHolding>().lastMiniGameResult != -1) 
        {
            changeScenario();
        }
    }

    void SetUIVariables()
    {
        romanceText.GetComponent<Text>().text = "Romance: " + theDialogue.romanceValue;
        if (theDialogue.statusValue == DialogueDistributor.PartnerStatus.Normal)
        {
            statusText.GetComponent<Text>().text = "Status: ---- ";
        }
        else
        {
            statusText.GetComponent<Text>().text = "Status: " + theDialogue.statusValue;
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
            else if (currentDialogueState == DialogueState.Done)
            {
                // Advance to the next bit of text, unless circumstances demand otherwise
                if (currentBackAndForthState == BackAndForthState.Dialogue && theDialogue.thereAreChoices[theDialogue.scenarioID, currentTextIndex])
                {
                    // If currently on a choice point, hitting enter does nothing, and this script waits for the word in the Update() function
                }
                else if (currentBackAndForthState == BackAndForthState.Dialogue && !theDialogue.thereAreChoices[theDialogue.scenarioID, currentTextIndex])
                {
                    // So long as there's more to display, display it
                    currentTextIndex++;
                    currentWritingIndex = 0;
                    dialogue.GetComponent<Text>().text = "";
                    currentDialogueState = DialogueState.Writing;
                }
                else if (currentBackAndForthState == BackAndForthState.Consequence && theDialogue.goesToMinigame[theDialogue.scenarioID, currentTextIndex])
                {
                    // Cut out and go to minigame if appropriate
                    goToMiniGame();
                }
                else if (currentBackAndForthState == BackAndForthState.Consequence && !theDialogue.goesToMinigame[theDialogue.scenarioID, currentTextIndex])
                {
                    // If at the end of consequence text, go to the next scenario
                    changeScenario();
                    goToMiniGame();
                }
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
                if (currentBackAndForthState == BackAndForthState.Dialogue && theDialogue.thereAreChoices[theDialogue.scenarioID, currentTextIndex])
                {
                    theChoices.PrepareChoice(theDialogue.masterChoices[theDialogue.scenarioID, 0],
                        theDialogue.masterChoices[theDialogue.scenarioID, 1],
                        theDialogue.masterChoices[theDialogue.scenarioID, 2]);
                }
                }
            }
            lastCharacterWrite = Time.timeSinceLevelLoad;
        }

        if (currentBackAndForthState == BackAndForthState.Dialogue && theChoices.choiceCompleted)
        {
            // Go back to the start of the dialogue print sequence, preparing to fill with consequence text
            currentTextIndex = 0;
            currentWritingIndex = 0;
            dialogue.GetComponent<Text>().text = "";

            // Check if the choice was bad or good based on current status, then update all variables and status
            if (theDialogue.badConsequenceTriggers[theDialogue.scenarioID, theChoices.choiceMade - 1] == theDialogue.statusValue)
            {
                theDialogue.romanceValue += theDialogue.consequenceRomance[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Bad];
                theDialogue.statusValue = theDialogue.consequenceStatus[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Bad];
                textToDisplay[0] = theDialogue.consequenceText[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Bad];
            }
            else if (theDialogue.goodConsequenceTriggers[theDialogue.scenarioID, theChoices.choiceMade - 1] == theDialogue.statusValue)
            {
                theDialogue.romanceValue += theDialogue.consequenceRomance[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Good];
                theDialogue.statusValue = theDialogue.consequenceStatus[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Good];
                textToDisplay[0] = theDialogue.consequenceText[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Good];
            }
            else
            {
                theDialogue.romanceValue += theDialogue.consequenceRomance[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Neutral];
                theDialogue.statusValue = theDialogue.consequenceStatus[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Neutral];
                textToDisplay[0] = theDialogue.consequenceText[theDialogue.scenarioID, theChoices.choiceMade - 1, (int)DialogueDistributor.Modifier.Neutral];
            }

            // Move to the next stage in the back-and-forth
            currentDialogueState = DialogueState.Writing;
            currentBackAndForthState = BackAndForthState.Consequence;

            theChoices.ResetChoice();
        }
    }

    private void changeScenario()
    {
        // Just as with initial setup, go to the start of the dialogue print sequence
        currentTextIndex = 0;
        currentWritingIndex = 0;
        dialogue.GetComponent<Text>().text = "";

        // Change the scenario
        if (theDialogue.scenarioID == 0)
        {
            // The only linked events are ScenarioID 0 followed by 1, while all others are randomized
            theDialogue.scenarioID = 1;
        }
        else
        {
            theDialogue.scenarioID = Random.Range(2, theDialogue.masterText.GetLength(0));
        }

        // Fill with the new text and move to the next stage in the back-and-forth
        for (int i = 0; i < theDialogue.masterText.GetLength(1); i++)
        {
            textToDisplay[i] = theDialogue.masterText[theDialogue.scenarioID, i];
        }
        currentDialogueState = DialogueState.Writing;
        currentBackAndForthState = BackAndForthState.Dialogue;
    }
    public void goToMiniGame()
    {
        if (data) 
        {
            data.GetComponent<DataHolding>().romanceVal = theDialogue.romanceValue;
            data.GetComponent<DataHolding>().statusVal = theDialogue.statusValue;
            data.GetComponent<DataHolding>().ID = theDialogue.scenarioID;
        }
        SceneManager.LoadScene("MiniGame");
    }
}
