using UnityEngine;
using System.Collections;

public class GateScript : MonoBehaviour {

    public bool islifting;
    float initalt;
	// Use this for initialization
	void Start () {
        initalt = transform.position.y;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if( (islifting) && (this.transform.position.y<(initalt+2)))
        {
            transform.position+=new Vector3(0,0.1f,0);
        }
	
	}
}
