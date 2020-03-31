using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolding : MonoBehaviour
{
    // Start is called before the first frame update
    public int romanceVal = 0;
    public DialogueDistributor.PartnerStatus statusVal = DialogueDistributor.PartnerStatus.Normal;
    public int ID = 0;
    public int lastMiniGameResult = -1;
    private void Awake()
    {
        statusVal = DialogueDistributor.PartnerStatus.Normal;
        ID = 0;
        lastMiniGameResult = -1;
    }
    void Start()
    {
        GameObject[] dataObjects = GameObject.FindGameObjectsWithTag("Data");
        if (dataObjects.Length > 1) 
        {
            for(int i = dataObjects.Length - 1; i >= 0; i--) 
            {
                if (dataObjects[i].GetComponent<DataHolding>().lastMiniGameResult == -1) 
                {
                    Destroy(dataObjects[i]);
                }
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
