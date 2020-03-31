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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DialogueDistributor.PartnerEmotion status = this.gameObject.GetComponent<DialogueDistributor>().emotionByText[this.gameObject.GetComponent<DialogueDistributor>().scenarioID,this.gameObject.GetComponent<PlayManager>().currentTextIndex];
        //happy, cry, angry, normal, mask 
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
