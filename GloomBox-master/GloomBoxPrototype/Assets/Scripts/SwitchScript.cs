using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {

    public GameObject gate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter()
    {
        gate.GetComponent<GateScript>().islifting = true;
    }
    
}

