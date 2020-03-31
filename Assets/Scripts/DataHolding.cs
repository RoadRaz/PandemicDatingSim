using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolding : MonoBehaviour
{
    // Start is called before the first frame update
    public int romanceVal = 0;
    public DialogueDistributor.PartnerStatus statusVal = DialogueDistributor.PartnerStatus.Normal;
    public int ID = 0;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
