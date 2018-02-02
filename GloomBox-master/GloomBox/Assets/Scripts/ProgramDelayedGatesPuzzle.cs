using UnityEngine;
using System.Collections;

public class ProgramDelayedGatesPuzzle : MonoBehaviour {

    TriggerController[] dgates = new TriggerController[40];
    public GameObject Switch;

	// Use this for initialization
	void Start ()
    {
        int i = 0;
        dgates = Switch.GetComponent<SwitchController>().triggerObjects;
	    foreach(TriggerController dgate in dgates)
        {
            if(dgate.GetComponent<GateController>())
            {
                dgate.GetComponent<GateController>().delay = 0.06f * i;
            }
            i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
