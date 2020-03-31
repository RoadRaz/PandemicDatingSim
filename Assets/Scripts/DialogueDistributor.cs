using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDistributor : MonoBehaviour
{
    // Displayed stat and appearance variables
    public int romanceValue = 0;
    public enum PartnerStatus { Error, Normal, Impatient, Scared, Lonely, Antisocial, Reckless }
    public PartnerStatus statusValue = PartnerStatus.Normal;
    public enum PartnerEmotion { Error, Normal, Mask, Angry, Cry, Happy }

    GameObject data;
    /* Each scenarioID indicates a whole scene, and it is the first index used when calling the arrays
    masterText[scenarioID, ] contains 1 or more strings to print as boxes of dialogue, ending with "" when it's out of text
    emotionByText[scenarioID, ] contains emotion to display for the romantic partner with those boxes
    thereAreChoices[scenarioID, ] goes alongside masterText, and returns True once it hits the end of text
    masterChoices[scenarioID, ] contains from 1 to 3 strings for choices, and any nonavailable choice is given ""

        Consequences are in groups of 3, divided Bad/Neutral/Good
    consequenceText[scenarioID, , ] contains from 1 to 3 sets of B/N/G strings to print as a single-box reaction to a choice, and any nonavailable choice is given ""
    consequenceEmotionByText[scenarioID, , ] matches B/N/G emotions to those
    consequenceRomance[scenarioID, , ] matches B/N/G Romance stat changes to those
    consequenceStatus[scenarioID, , ] matches B/N/G Status stat changes to those
    badConsequenceTriggers[scenarioID, ] contains from 1 to 3 identifiers of what existing Status would make this choice a bad one, and any nonavailable choice is given PartnerStatus.Error
    goodConsequenceTriggers[scenarioID, ] contains from 1 to 3 identifiers of what existing Status would make this choice a good one, and any nonavailable choice is given PartnerStatus.Error

    goesToMinigame[scenarioID, ] returns True if this choice prompts a minigame
    */
    [HideInInspector]
    public int scenarioID;
    [HideInInspector]
    public string[,] masterText;
    [HideInInspector]
    public PartnerEmotion[,] emotionByText;
    [HideInInspector]
    public bool[,] thereAreChoices;
    [HideInInspector]
    public string[,] masterChoices;

    [HideInInspector]
    public string[,,] consequenceText;
    [HideInInspector]
    public PartnerEmotion[,,] consequenceEmotionByText;
    [HideInInspector]
    public int[,,] consequenceRomance;
    [HideInInspector]
    public PartnerStatus[,,] consequenceStatus;
    [HideInInspector]
    public PartnerStatus[,] badConsequenceTriggers;
    [HideInInspector]
    public PartnerStatus[,] goodConsequenceTriggers;

    [HideInInspector]
    public bool[,] goesToMinigame;

    public enum Modifier { Bad = 0, Neutral = 1, Good = 2 } // Redundant numbers, but listed here to emphasize that these are array locations

    // Start is called before the first frame update . . . but apparently we need Awake for this one
    void Awake()
    {
        data = GameObject.Find("SAVEDDATA");
        if (data)
        {
            statusValue = data.GetComponent<DataHolding>().statusVal;
            romanceValue = data.GetComponent<DataHolding>().romanceVal;
            scenarioID = data.GetComponent<DataHolding>().ID;
        }

        masterText = new string[,] {
            { // ID: 0
                "Hello! My name is Hmhmhmmm and I'm looking forward to having a great time with you!",
                "I'm really into sports and social gatherings, but sadly due to the virus I can't do any of that...",
                "So maybe we can talk and get to know each other better! As long as we remember not to get too close ;)",
                "What fun activities can we do in quarantine?"
            },
            { // ID: 1
                "Well, from here on out, there will be a lot of randomly-selected scenarios with placeholder text",
                "There is no actual end or goal in sight!",
                "Just be sure to click your choice instead of hitting \"return\", since keyboard input is a little off",
                "And watch out for the minigame!!!"
            },
            { // ID: 2
                "This is a 'randomly-selected' bit of dialogue",
                "It is nonsense put in here to test interactions!", "", ""
            },
            { // ID: 3
                "This dialog is 'selected at random'",
                "It is nonsense put in here to test interactions!", "", ""
            },
            { // ID: 4
                "Scenario: Suddenly, crowd minigame!",
                "Tons of ill-coordinated people with no respect for healthy distance get in your way",
                "I mean, I've got a mask and I'm made of pixels. I'm fine. But can you keep yourself safe?",
                "No actual consequences for now; just try!"
            },
            { // ID: 5
                "Scenario: Boring afternoon",
                "There's nothing to do, so how I react to your choice depends heavily on current \"Status\"", "", ""
            },
            { // ID: 6
                "Scenario: Urgent phone call",
                "I just found out from my parents that somebody important got sick", "", ""
            },
            { // ID: 7
                "Scenario: Late night phone call",
                "There's no one else for conversation, so how I react to your choice depends heavily on current \"Status\"", "", ""
            },
            { // ID: 8
                "Scenario: Resource management",
                "With people hoarding, what in the world are we supposed to do if we run out of toilet paper?", "", ""
            },
            { // ID: 9
                "Scenario: Hobby chitchat",
                "A simple check for matching interests and raising the \"Romance\" stat", "", ""
            },
            { // ID: 10
                "Scenario: Outdoors chitchat",
                "A simple check for matching interests and raising the \"Romance\" stat", "", ""
            },
            { // ID: 11
                "Scenario: Shopping plans",
                "Store hours and availability are changing every week!",
                "It's like I need to watch them like a hawk to do normal shopping! Or should I quit it and look online?", ""
            },
            { // ID: 12
                "Scenario: Health and planning activities",
                "Too much sitting around, and the only social stuff is to take walks. Tell me you have some plans!", "", ""
            }
        };

        emotionByText = new PartnerEmotion[,] {
            { // ID: 0
                PartnerEmotion.Happy,
                PartnerEmotion.Normal,
                PartnerEmotion.Happy,
                PartnerEmotion.Normal
            },
            { // ID: 1
                PartnerEmotion.Normal,
                PartnerEmotion.Normal,
                PartnerEmotion.Happy,
                PartnerEmotion.Angry
            },
            { // ID: 2
                PartnerEmotion.Angry,
                PartnerEmotion.Cry, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 3
                PartnerEmotion.Cry,
                PartnerEmotion.Angry, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 4
                PartnerEmotion.Mask,
                PartnerEmotion.Mask,
                PartnerEmotion.Mask,
                PartnerEmotion.Mask
            },
            { // ID: 5
                PartnerEmotion.Normal,
                PartnerEmotion.Normal, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 6
                PartnerEmotion.Normal,
                PartnerEmotion.Cry, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 7
                PartnerEmotion.Normal,
                PartnerEmotion.Normal, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 8
                PartnerEmotion.Normal,
                PartnerEmotion.Angry, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 9
                PartnerEmotion.Normal,
                PartnerEmotion.Happy, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 10
                PartnerEmotion.Normal,
                PartnerEmotion.Happy, PartnerEmotion.Error, PartnerEmotion.Error
            },
            { // ID: 11
                PartnerEmotion.Normal,
                PartnerEmotion.Angry,
                PartnerEmotion.Normal, PartnerEmotion.Error
            },
            { // ID: 12
                PartnerEmotion.Normal,
                PartnerEmotion.Angry, PartnerEmotion.Error, PartnerEmotion.Error
            }
        };

        masterChoices = new string[,] {
            { // ID: 0
                "We'll have to think of something cool to do together!",
                "I have a lot of ideas; I hope you like them!",
                "I'm hopelessly clueless and shouldn't be here!"
            },
            { // ID: 1
                "Argyle?",
                "Hemaglobin?",
                "Nematode?"
            },
            { // ID: 2
                "Still nematode",
                "Sdafsf",
                ""
            },
            { // ID: 3
                "Nematode forever",
                "",
                ""
            },
            { // ID: 4
                "Bring on the crowds!",
                "",
                ""
            },
            { // ID: 5
                "",
                "",
                ""
            },
            { // ID: 6
                "",
                "",
                ""
            },
            { // ID: 7
                "",
                "",
                ""
            },
            { // ID: 8
                "",
                "",
                ""
            },
            { // ID: 9
                "",
                "",
                ""
            },
            { // ID: 10
                "",
                "",
                ""
            },
            { // ID: 11
                "",
                "",
                ""
            },
            { // ID: 12
                "",
                "",
                ""
            }
        };

        consequenceText = new string[,,] {
            { // ID: 0
                { "", "Oh, I'm plenty creative! This should be fun", "" },
                { "",  "Looking forward to it!", "" },
                { "", "Uhh . . . ", "" }
            },
            { // ID: 1
                { "", "Absquatulate!", "" },
                { "", "Hermeticism!", ""},
                { "", "Nudibranch!", ""}
            },
            { // ID: 2
                { "I am impatient with nematodes!", "Glad you stick to your priorities", "Nematodes, in the end, are comforting"},
                { "What am I to do when lonely and you're just mashing the keyboard?", "Dasdasds", "Good, I didn't want human company anyway" },
                { "", "", "" }
            },
            { // ID: 3
                { "And here I was hoping for something as reckless as me!", "Your nematodes are highly consistent", "At least I am not alone with the nematodes"},
                { "", "", "" },
                { "", "", "" }
            },
            { // ID: 4
                { "", "Use WASD to avoid the infected dots until time is up!", ""},
                { "", "", "" },
                { "", "", "" }
            }
        };
        
        consequenceEmotionByText = new PartnerEmotion[,,] {
            { // ID: 0
                { PartnerEmotion.Error, PartnerEmotion.Happy, PartnerEmotion.Error },
                { PartnerEmotion.Error, PartnerEmotion.Happy, PartnerEmotion.Error },
                { PartnerEmotion.Error, PartnerEmotion.Normal, PartnerEmotion.Error }
            },
            { // ID: 1
                { PartnerEmotion.Error, PartnerEmotion.Happy, PartnerEmotion.Error },
                { PartnerEmotion.Error, PartnerEmotion.Happy, PartnerEmotion.Error },
                { PartnerEmotion.Error, PartnerEmotion.Happy, PartnerEmotion.Error }
            },
            { // ID: 2
                { PartnerEmotion.Cry, PartnerEmotion.Cry, PartnerEmotion.Cry },
                { PartnerEmotion.Cry, PartnerEmotion.Cry, PartnerEmotion.Cry },
                { PartnerEmotion.Error, PartnerEmotion.Error, PartnerEmotion.Error }
            },
            { // ID: 3
                { PartnerEmotion.Angry, PartnerEmotion.Angry, PartnerEmotion.Angry },
                { PartnerEmotion.Error, PartnerEmotion.Error, PartnerEmotion.Error },
                { PartnerEmotion.Error, PartnerEmotion.Error, PartnerEmotion.Error }
            }
        };

        consequenceRomance = new int[,,] {
            { // ID: 0
                { 0, 50, 0 },
                { 0, 50, 0 },
                { 0, 50, 0 }
            },
            { // ID: 1
                { 0, 5, 0 },
                { 0, 5, 0 },
                { 0, 5, 0 }
            },
            { // ID: 2
                { -5, 5, 10 },
                { -15, -5, 10 },
                { 0, 0, 0 }
            },
            { // ID: 3
                { -10, 5, 10 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            }
        };

        consequenceStatus = new PartnerStatus[,,] {
            { // ID: 0
                { PartnerStatus.Error, PartnerStatus.Normal, PartnerStatus.Error },
                { PartnerStatus.Error, PartnerStatus.Normal, PartnerStatus.Error },
                { PartnerStatus.Error, PartnerStatus.Normal, PartnerStatus.Error }
            },
            { // ID: 1
                { PartnerStatus.Error, PartnerStatus.Impatient, PartnerStatus.Error },
                { PartnerStatus.Error, PartnerStatus.Scared, PartnerStatus.Error },
                { PartnerStatus.Error, PartnerStatus.Reckless, PartnerStatus.Error }
            },
            { // ID: 2
                { PartnerStatus.Lonely, PartnerStatus.Lonely, PartnerStatus.Lonely },
                { PartnerStatus.Antisocial, PartnerStatus.Antisocial, PartnerStatus.Antisocial },
                { PartnerStatus.Error, PartnerStatus.Error, PartnerStatus.Error }
            },
            { // ID: 3
                { PartnerStatus.Normal, PartnerStatus.Normal, PartnerStatus.Normal },
                { PartnerStatus.Error, PartnerStatus.Error, PartnerStatus.Error },
                { PartnerStatus.Error, PartnerStatus.Error, PartnerStatus.Error }
            }
        };

        badConsequenceTriggers = new PartnerStatus[,] {
            { // ID: 0
                PartnerStatus.Error,
                PartnerStatus.Error,
                PartnerStatus.Error
            },
            { // ID: 1
                PartnerStatus.Error,
                PartnerStatus.Error,
                PartnerStatus.Error
            },
            { // ID: 2
                PartnerStatus.Impatient,
                PartnerStatus.Lonely,
                PartnerStatus.Error
            },
            { // ID: 3
                PartnerStatus.Reckless,
                PartnerStatus.Error,
                PartnerStatus.Error
            }
        };

        goodConsequenceTriggers = new PartnerStatus[,] {
            { // ID: 0
                PartnerStatus.Error,
                PartnerStatus.Error,
                PartnerStatus.Error
            },
            { // ID: 1
                PartnerStatus.Error,
                PartnerStatus.Error,
                PartnerStatus.Error
            },
            { // ID: 2
                PartnerStatus.Scared,
                PartnerStatus.Antisocial,
                PartnerStatus.Error
            },
            { // ID: 3
                PartnerStatus.Lonely,
                PartnerStatus.Error,
                PartnerStatus.Error
            }
        };

        goesToMinigame = new bool[,] {
            { // ID: 0
                false,
                false,
                false
            },
            { // ID: 1
                false,
                false,
                false
            },
            { // ID: 2
                false,
                false,
                false
            },
            { // ID: 3
                false,
                false,
                false
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
                    // If gameplay gets as far as the end of the strings, the current one must hold the choice point
                    thereAreChoices[i, j] = true;
                }
                else
                {
                    thereAreChoices[i, j] = false;
                }

            }
            
        }




    }
}