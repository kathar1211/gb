using UnityEngine;
using System.Collections;

public class MonsterScript : MonoBehaviour {

    public GameObject wp;
    public bool isgravityinverted;
    Vector3 initpos;
    public float leftlimit;
    public float rightlimit;
    bool ismovingright = true;
    public float speed = 0.1f;
	// Use this for initialization
	void Start ()
    {
        initpos = this.transform.position;
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isgravityinverted)
        {            
            this.GetComponent<Rigidbody>().AddForce(0, 50f*this.GetComponent<Rigidbody>().mass, 0);
        }
        Wander();
	}

    public void Cry()
    {
        if((wp.name == "waterplane (2)")&&(this.isgravityinverted))
        {
            wp.GetComponent<WaterplaneScript>().filling = true;
        }
        else if(wp.name != "waterplane (2)")
            wp.GetComponent<WaterplaneScript>().filling = true;
    }

    public void Wander()
    {
        //Debug.Log("wander");
        if(ismovingright)
        {
            if(this.transform.position.x < initpos.x + rightlimit)
            {
                this.transform.position += new Vector3(speed, 0, 0);
            }
            else
            {
                ismovingright = false;
            }
        }
        else
        {
            if(this.transform.position.x > initpos.x - leftlimit)
            {
                this.transform.position += new Vector3(-1 * speed, 0, 0);
            }
            else
            {
                ismovingright = true;
            }
        }
    }

    public IEnumerator SetText(string text)
    {
        GetComponentInChildren<TextMesh>().text = text;
        yield return new WaitForSeconds(1f);
        GetComponentInChildren<TextMesh>().text = "";
    }

}
