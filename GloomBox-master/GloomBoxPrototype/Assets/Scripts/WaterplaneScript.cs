using UnityEngine;
using System.Collections;

public class WaterplaneScript : MonoBehaviour {
    public bool filling;
    public float delay;
    float timecounter = 0f;
    public float maxscale = 6;
    public Vector3 fillIncr = new Vector3(0, 0.1f, 0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(filling)
        {
            timecounter += Time.deltaTime;
            if(timecounter>delay)
                FillUp();
        }
	
	}

    void FillUp()
    {
        if(this.transform.localScale.y < maxscale)
            this.transform.localScale += fillIncr;
        else
        {
            if (this.transform.parent != null)
                this.transform.parent.GetComponent<Rigidbody>().mass = 10;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("FloatPlatform"))
        {
            other.GetComponent<Rigidbody>().AddForce(0, 0.1f, 0);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FloatPlatform"))
        {
            other.GetComponent<Rigidbody>().AddForce(0, 0.8f, 0);
        }
    }
}
