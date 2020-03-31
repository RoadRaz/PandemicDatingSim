using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DavidsCharacterManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DialogueDistributor.PartnerEmotion status = this.gameObject.GetComponent<DialogueDistributor>().emot;
        //happy, cry, angry, normal, mask
        //if(status == DialogueDistributor.PartnerStatus.)
    }
}
