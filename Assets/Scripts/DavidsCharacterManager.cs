using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DavidsCharacterManager : MonoBehaviour
{
    public Sprite normal;
    public Sprite happy;
    public Sprite cry;
    public Sprite angry;
    public Sprite mask;

    public GameObject character;

    DialogueDistributor.PartnerEmotion status;
    PlayManager.BackAndForthState pointInConversation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check which list will be needed for the emotion
        pointInConversation = this.gameObject.GetComponent<PlayManager>().currentBackAndForthState;

        // Happy, cry, angry, normal, mask 
        switch (pointInConversation)
        {
            case PlayManager.BackAndForthState.Dialogue:
                status = this.gameObject.GetComponent<DialogueDistributor>().emotionByText[this.gameObject.GetComponent<DialogueDistributor>().scenarioID, this.gameObject.GetComponent<PlayManager>().currentTextIndex];
                break;
            case PlayManager.BackAndForthState.Consequence:

                if (this.gameObject.GetComponent<DialogueDistributor>().badConsequenceTriggers[this.gameObject.GetComponent<DialogueDistributor>().scenarioID, this.gameObject.GetComponent<ChoiceScript>().choiceMade - 1] == this.gameObject.GetComponent<DialogueDistributor>().statusValue)
                {
                    status = this.gameObject.GetComponent<DialogueDistributor>().consequenceEmotionByText[this.gameObject.GetComponent<DialogueDistributor>().scenarioID, this.gameObject.GetComponent<ChoiceScript>().choiceMade - 1, (int)DialogueDistributor.Modifier.Bad];
                }
                else if (this.gameObject.GetComponent<DialogueDistributor>().goodConsequenceTriggers[this.gameObject.GetComponent<DialogueDistributor>().scenarioID, this.gameObject.GetComponent<ChoiceScript>().choiceMade - 1] == this.gameObject.GetComponent<DialogueDistributor>().statusValue)
                {
                    status = this.gameObject.GetComponent<DialogueDistributor>().consequenceEmotionByText[this.gameObject.GetComponent<DialogueDistributor>().scenarioID, this.gameObject.GetComponent<ChoiceScript>().choiceMade - 1, (int)DialogueDistributor.Modifier.Good];
                }
                else
                {
                    status = this.gameObject.GetComponent<DialogueDistributor>().consequenceEmotionByText[this.gameObject.GetComponent<DialogueDistributor>().scenarioID, this.gameObject.GetComponent<ChoiceScript>().choiceMade - 1, (int)DialogueDistributor.Modifier.Neutral];
                }
                break;
            default:
                break;
        }

        if (status == DialogueDistributor.PartnerEmotion.Normal)
        {
            if (normal && character) 
            {
                character.GetComponent<Image>().sprite = normal;
            }
        }
        else if (status == DialogueDistributor.PartnerEmotion.Mask)
        {
            if (mask && character)
            {
                character.GetComponent<Image>().sprite = mask;
            }
        }
        else if (status == DialogueDistributor.PartnerEmotion.Happy)
        {
            if (happy && character)
            {
                character.GetComponent<Image>().sprite = happy;
            }
        }
        else if (status == DialogueDistributor.PartnerEmotion.Cry)
        {
            if (cry && character)
            {
                character.GetComponent<Image>().sprite = cry;
            }
        }
        else if (status == DialogueDistributor.PartnerEmotion.Angry) 
        {
            if (angry && character)
            {
                character.GetComponent<Image>().sprite = angry;
            }
        }
    }
}
