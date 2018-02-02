using UnityEngine;
using System.Collections;

public class CrackedPlatformScript : MonoBehaviour {

    public GameObject attachedmonster;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionStay(Collision other)
    {
        //print("collision");
        if((other.gameObject.CompareTag("Monster"))||(other.gameObject.CompareTag("Bucket")))
        {
            //if(other.gameObject.GetComponentInChildren<WaterplaneScript>() != null)
            //{
            //    if(other.gameObject.GetComponentInChildren<WaterplaneScript>().filling)
            //    {
            //        Destroy(this.gameObject);
            //    }
            //}
            if (other.gameObject.GetComponent<Rigidbody>().mass > 5)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
