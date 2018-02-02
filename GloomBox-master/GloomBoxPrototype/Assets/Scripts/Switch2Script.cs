using UnityEngine;
using System.Collections;

public class Switch2Script : MonoBehaviour {

    // Use this for initialization
    public GameObject retractingPlatform;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.name.Contains("water"))
            retractingPlatform.GetComponent<RetractingPlatformScript>().isRetracting = true;
    }
}
