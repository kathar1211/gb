using UnityEngine;
using System.Collections;

public class RetractingPlatformScript : MonoBehaviour {

    // Use this for initialization
    public bool isRetracting = false;
    Vector3 initpos;
	void Start ()
    {
        initpos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isRetracting)
        {
            if (this.transform.position.x > initpos.x - 5)
                this.transform.position += new Vector3(-0.01f,0,0);
        }
	
	}
}
