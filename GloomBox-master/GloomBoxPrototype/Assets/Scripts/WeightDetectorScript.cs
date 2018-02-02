using UnityEngine;
using System.Collections;

public class WeightDetectorScript : MonoBehaviour {

    public GameObject monster;
    public GameObject otherHalf;
    public float downspeed;
    float lowerlimit;
	// Use this for initialization
	void Start ()
    {
        lowerlimit = this.transform.position.y - 2;
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if((monster.GetComponent<Rigidbody>().mass > 5)&&(this.transform.position.y>lowerlimit))
        {
            this.transform.position = this.transform.position + new Vector3(0,-1*downspeed);
            otherHalf.transform.position = otherHalf.transform.position + new Vector3(0, downspeed);
        }
	
	}
}
