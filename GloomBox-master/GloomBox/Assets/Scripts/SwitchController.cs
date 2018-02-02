#region Using Directives
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

#region SwitchController Class
public class SwitchController : MonoBehaviour
{
	#region Public Variables
    public TriggerController[] triggerObjects;
    public bool moveUp;
    public bool Hold;
    public float holdDelay;
    public Sprite pressedSwitch;
	bool soundPlayed=false;
	#endregion

	#region Private Variables
    bool isTriggered;
    Vector3 targetPosition;
    Vector3 startPosition;
    float deactivateTime;
	Sprite origSprite;
	#endregion

	// Use this for initialization
	void Start()
    {
		soundPlayed = false;
        isTriggered = false;
        //GetComponent<SpriteRenderer>().color = Color.red;
        startPosition = transform.position;

		// Reposition based on whether this switch moves up or down when it's pressed
		if(moveUp)
		{
			targetPosition = transform.position + new Vector3(0, GetComponent<BoxCollider2D>().size.y, 0);
		}
		else
		{
			targetPosition = transform.position - new Vector3(0, GetComponent<BoxCollider2D>().size.y, 0);
		}

		origSprite = GetComponent<SpriteRenderer>().sprite;
    }
	
	// Update is called once per frame
	void Update()
    {
		
		// If this switch is triggered, move toward its triggered position
	    if(isTriggered)
        {
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2f * Time.deltaTime);
        }

		// Otherwise, move back toward the original position and tell the trigger objects that this switch is no longer active
        else
        {
            if(Time.time > deactivateTime && !GetComponent<FrozenController>().isFrozen)
            {
                //GetComponent<SpriteRenderer>().color = Color.red;
                //transform.position = Vector3.MoveTowards(transform.position, startPosition, 2f * Time.deltaTime);
				GetComponent<SpriteRenderer>().sprite = origSprite;

                foreach(TriggerController t in triggerObjects)
                {
                    t.TriggerOff();
					soundPlayed = false;
                }
            }
        }

        if(Hold)
        {
			if(isTriggered)
			{
				deactivateTime = Time.time + holdDelay;
			}

            isTriggered = false;
        }
	}

	#region Event Handlers
	/// <summary>
	/// Handler for when a collider stays touching this switch
	/// </summary>
	/// <param name="other">The collider touching this switch</param>
    void OnTriggerStay2D(Collider2D other)
    {
		// Don't do anything if this switch is frozen
		if(GetComponent<FrozenController>().isFrozen)
		{
			return;
		}

		// If the touching collider belongs to a player, activate the switch
        if(other.CompareTag("Player"))
        {
            Activate();
            isTriggered = true;
        }

		// If the touching collider belongs to a heavy monster, activate the switch
        else if(other.CompareTag("Monster"))
        {
            if(other.gameObject.GetComponent<MonsterController>().hypergravity)
            {
                Activate();
                isTriggered = true;
            }
        }

		// If the touching collider belongs to a full container, activate the switch
        else if(other.CompareTag("Container"))
        {
            if(other.GetComponentInChildren<ContainerController>().isFull)
            {
				Activate();
                isTriggered = true;
            }
        }
    }
	#endregion

	#region Switch Activation
	/// <summary>
	/// Activate this switch
	/// </summary>
    void Activate()
    {
		// Don't do anything if this switch is already triggered
		if(isTriggered)
		{
			return;
		}

        isTriggered = true;

		if (!soundPlayed) {
			AudioSource switchPress = GetComponents<AudioSource> () [1];
			switchPress.Play ();
			soundPlayed = true;
		}
	
        //GetComponent<SpriteRenderer>().color = Color.green;
        GetComponent<SpriteRenderer>().sprite = pressedSwitch;

        foreach(TriggerController t in triggerObjects)
        {
            t.OnTrigger();
        }
    }
	#endregion
}
#endregion