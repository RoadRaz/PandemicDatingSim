using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDistributor : MonoBehaviour
{
    /* Each scenarioID indicates a whole scene, and it is the first index used when calling the arrays
    masterText[scenarioID][] contains 1 or more strings to print as boxes of dialogue, ending with "" when it's out of text
    thereAreChoices[scenarioID][] counts down alongside masterText, and returns True once it hits the end of text
    masterChoices[scenarioID][] contains from 1 to 3 strings for choices, and any nonavailable choice is given ""
    */
    [HideInInspector]
    public int scenarioID;
    [HideInInspector]
    public string[,] masterText;
    [HideInInspector]
    public bool[,] thereAreChoices;
    [HideInInspector]
    public string[,] masterChoices;

    // Start is called before the first frame update . . . but apparently we need Awake for this one
    void Awake()
    {
        scenarioID = 0;

        masterText = new string[,] {
            { // ID: 0
        "Hello! My Name is ----- and I'm looking forward to having a great time with you!",
        "I'm really into sports and social gatherings, but sadly due to the virus I can't do any of that...",
        "So maybe we can talk and get to know each other better! As long as we remember not to get too close ;)",
        "What fun activities can we do in quarantine?"
            },
            { // ID: 1
        "Cheeese!", "", "", ""
            },
            { // ID: 2
        "sadasd", "asdasdas", "", ""
            }
        };

        masterChoices = new string[,] {
            { // ID: 0
        "Likewise! What are you interested in?",
        "Fantastic! I am excited too!",
        "We will have to think of something cool to do together!"
            },
            { // ID: 1
        "Argyle?", "", ""
            },
            { // ID: 2
        "Nematode", "sdafsf", ""
            }
        };

        thereAreChoices = new bool[masterText.GetLength(0), masterText.GetLength(1)];

        // Figure out where the choice point is by looking for data, since this is easier than manually recording each one
        for (int i = 0; i < masterText.GetLength(0); i++)
        {
            for (int j = 1; j < masterText.GetLength(1); j++) // Skip the first string, since there's always at least one
            {
                if (masterText[i, j] == "")
                {
                    // Having found empty strings, the choice point must have been reached by the previous one
                    thereAreChoices[i, j - 1] = true;
                }
                else if (j == masterText.GetLength(1) - 1)
                {
                    // If gameplay gets as far as the end of the strings, the current one must have the choice point
                    thereAreChoices[i, j] = true;
                }
                else
                {
                    thereAreChoices[i, j] = false;
                }

            }
            
        }

        /*for (int i = 0; i < masterText.GetLength(0); i++)
        {
            for (int j = 0; j < masterText.GetLength(1); j++)
            {
                Debug.Log(masterText[i, j]);
                Debug.Log(thereAreChoices[i, j]);
            }

        }*/

    }
}
