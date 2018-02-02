using UnityEngine;
using System.Collections;

public class BucketBreak : MonoBehaviour {

    public GameObject waterPlane;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //print(waterPlane.GetComponent<WaterplaneScript>().filling);
        if (waterPlane.GetComponent<WaterplaneScript>().filling)
        {
            Destroy(this.gameObject, 2.5f);
        }
	}
}
