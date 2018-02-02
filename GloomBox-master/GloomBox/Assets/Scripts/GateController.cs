#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region GateController Class
public class GateController : TriggerController
{
	#region Public Variables
    public Vector3 targetPosition;
    public float movingspeed;
    public float delay;
    #endregion

    #region Private 
    Vector3 startPosition;
    bool triggerOn;
	bool soundPlayed;
	#endregion

	#region Properties
	public override void OnTrigger()
	{
        //base.OnTrigger();
        Invoke("DelayedTrigger", delay);
	}

	public override void TriggerOff()
	{
		//base.TriggerOff();
		triggerOn = false;
	}
	#endregion

	// Use this for initialization
	void Start()
    {
		soundPlayed = true;
        startPosition = transform.position;
        targetPosition = this.transform.position + targetPosition;
        triggerOn = false;
	}
	
	// Update is called once per frame
	void Update()
    {
		// If the associated switch is triggered and this gate isn't frozen, move toward open position
	    if(triggerOn)
        {
            if(!GetComponent<FrozenController>().isFrozen)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movingspeed * Time.deltaTime);
				soundPlayed = false;
            }
        }
		// If the associated switch isn't triggered and this gate isn't frozen, move toward closed position
        else
        {
            if(!GetComponent<FrozenController>().isFrozen)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, movingspeed * Time.deltaTime);
				//play gate sound when they drop
				if (transform.position == startPosition && !soundPlayed) {
					AudioSource gateClose = GetComponents<AudioSource> () [1];
					gateClose.Play();
					soundPlayed = true;
				}
				/*if (!soundPlayed) {
					AudioSource gateClose = GetComponents<AudioSource> () [1];
					gateClose.PlayDelayed (1.6f);
					soundPlayed = true;
				}*/
            }
		}
	
	}

    void DelayedTrigger()
    {
        triggerOn = true;
    }
}
#endregion