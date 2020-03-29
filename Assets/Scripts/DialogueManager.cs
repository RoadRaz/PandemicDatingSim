using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    enum DialogueState {Off,Writing,Done }
    DialogueState currentDialogueState = DialogueState.Off;
    int currentTextIndex = 0;

    int currentTextStringIndex = 0;
    float writingDelay = 0.01f;
    float lastCharacterWrite = 0;

    GameObject dialogue;
    string[] text =
    {
        "Hello! My Name is ----- and I'm looking forward to having a great time with you!",
        "I'm really into sports and social gatherings, but sadly due to the virus I can't do any of that...",
        "So maybe we can talk and get to know each other better! As long as we remember to not get too close ;)"
    };
    void Start()
    {
        dialogue = GameObject.Find("DialogueText");
        currentDialogueState = DialogueState.Writing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (currentDialogueState == DialogueState.Writing)
            {
                //set dialogue to full current text
                currentDialogueState = DialogueState.Done;
            }
            else if (currentDialogueState == DialogueState.Done) 
            {
                //go to next text field (or next state)
                currentTextIndex++;
                currentTextStringIndex = 0;
                dialogue.GetComponent<Text>().text = "";
                lastCharacterWrite = 0;
                currentDialogueState = DialogueState.Writing;
            }

        }
        if (currentDialogueState == DialogueState.Writing && Time.timeSinceLevelLoad - lastCharacterWrite > writingDelay) 
        {
            if (currentTextIndex < text.Length) 
            {
                if (currentTextStringIndex < text[currentTextIndex].Length)
                {
                    dialogue.GetComponent<Text>().text += text[currentTextIndex][currentTextStringIndex];
                    currentTextStringIndex++;
                }
                else
                {
                    currentDialogueState = DialogueState.Done;
                }
            }
            lastCharacterWrite = Time.timeSinceLevelLoad;
        }
        if (currentDialogueState == DialogueState.Writing) 
        {
            
        }
    }
}
