#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region TearController Class
public class TearController : MonoBehaviour
{
	#region Public Variables
    public StatesOfWater state;
    public float iceMeltingTime = 3f;
    public Sprite waterSprite;
    public Sprite iceSprite;
    public Sprite steamSprite;
	#endregion

	#region Private Variables
    float startTime;
    float iceStartTime;
    FrozenController frozenObj;
	#endregion

	// Called once, before Start
    void Awake()
    {
        startTime = Time.time;
        GetComponent<Rigidbody2D>().AddForce(Vector2.zero);
    }

	// Update is called once per frame
    void Update()
    {
        //ice melting
        if (GetComponent<Rigidbody2D>().isKinematic)
		{
            //See if it's time to melt
            if (Time.time - iceStartTime > iceMeltingTime)
            {
                frozenObj.Defreeze();
                Destroy(gameObject);
            }
            return;
		}

		// TODO: Expose this in monster somewhere
		// Tears only last for 10 seconds
        if(Time.time - startTime > 10f)
        {
            Destroy(gameObject);
        }
    }

	#region Event Handlers
	// When this tear collides with another object, it may disappear, change state, or freeze something
    void OnTriggerEnter2D(Collider2D other)
    {
		// Can't be triggered by monsters, other tears, or containers
        if(!other.CompareTag("Monster") &&
			!other.CompareTag("Tear") &&
			!other.CompareTag("MonsterHead") &&
			!other.CompareTag("Container") &&
            !other.CompareTag("DialogueTrigger"))
        {
			// Water is destroyed on contact with another surface
            if(state == StatesOfWater.Liquid)
            {
                Destroy(gameObject);
            }

            // Steam transitions ot water when it touches a surface
            else if(state == StatesOfWater.Gas)
            {
                TransToWater();
            }

            // Ice is destroyed if it touches something unfreezable
			// It freezes something that it touches if it is freezable
            else if(state == StatesOfWater.Solid)
            {
                if (GetComponent<Rigidbody2D>().isKinematic)
                {
                    return;
                }

                // If there's no FrozenController, the contacted object can't be frozen
                if (other.gameObject.GetComponent<FrozenController>() == null)
                {
                    Destroy(gameObject);
                    return;
                }

				// Otherwise, freeze that object
                else
                {
                    iceStartTime = Time.time;
                    GetComponent<Rigidbody2D>().isKinematic = true;
                    frozenObj = other.gameObject.GetComponent<FrozenController>();
                    frozenObj.Freeze();
                }
            }
        }
    }
	#endregion

	#region State Transition Functions
	/// <summary>
	/// Transition to steam state
	/// </summary>
    public void TransToGas()
    {
        state = StatesOfWater.Gas;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = -0.5f;
        GetComponent<SpriteRenderer>().sprite = steamSprite;
        transform.localScale = transform.localScale * 3f;
    }

	/// <summary>
	/// Transition to ice state
	/// </summary>
    public void TransToIce()
    {
        state = StatesOfWater.Solid;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<SpriteRenderer>().sprite = iceSprite;
        transform.localScale = transform.localScale * 3f;
    }

	/// <summary>
	/// Transition to water state
	/// </summary>
    public void TransToWater()
    {
        if(state != StatesOfWater.Gas)
            transform.localScale = transform.localScale * 2f;
        state = StatesOfWater.Liquid;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<SpriteRenderer>().sprite = waterSprite;
    }
	#endregion
}
#endregion