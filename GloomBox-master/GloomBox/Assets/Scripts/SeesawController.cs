#region Using Directives
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

#region SeesawController Class
public class SeesawController : MonoBehaviour
{
	#region Public Variables
    public float weightThreshhold;
	public List<GameObject> objectsList = new List<GameObject>();
	#endregion

	#region Private Variables
	float currentWeight;
    Vector3 centerPoint;
    bool triggeredByContainer;
	#endregion

    // Use this for initialization
    void Start()
    {
        centerPoint =  GetComponent<SpriteRenderer>().bounds.center;
        triggeredByContainer = false;
	}
	
	// Update is called once per frame
	void Update()
    {
        currentWeight = 0f;

		//See if weight is enough to tip seesaw
        foreach(GameObject obj in objectsList)
        {
            Vector3 colCenter = obj.GetComponent<SpriteRenderer>().bounds.center;
            colCenter -= centerPoint;
            if(colCenter.x * obj.GetComponent<Rigidbody2D>().gravityScale > 0)
            {

                currentWeight += Mathf.Abs(obj.GetComponent<Rigidbody2D>().mass * obj.GetComponent<Rigidbody2D>().gravityScale);
            }
            else
            {
                currentWeight -= Mathf.Abs(obj.GetComponent<Rigidbody2D>().mass * obj.GetComponent<Rigidbody2D>().gravityScale);
            }
        }
			
        if(!triggeredByContainer)
        {
            if(Mathf.Abs(currentWeight) >= weightThreshhold)
            {
                GetComponent<Rigidbody2D>().freezeRotation = false;
            }
            else
            {
                GetComponent<Rigidbody2D>().freezeRotation = true;
            }
        }

	}

	#region Event Handlers
	// When an object touches the collider of this seesaw, add it ot the list of objects
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Player"))
        {
            objectsList.Add(other.gameObject);
        }
    }

	// If a full container stays touching the collider of this seesaw, unfreeze rotation
    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Container"))
        {
            if(other.gameObject.GetComponentInChildren<ContainerController>().isFull)
            {
                triggeredByContainer = true;
                GetComponent<Rigidbody2D>().freezeRotation = false;
            }
            else
            {
                triggeredByContainer = false;
                GetComponent<Rigidbody2D>().freezeRotation = true;
            }
        }
    }

	// When an object stops touching the collider of this seesaw, remove it from the list of objects
    void OnCollisionExit2D(Collision2D other)
    {
        objectsList.Remove(other.gameObject);
    }
	#endregion
}
#endregion