#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region MonsterController Class
public class MonsterController : MonoBehaviour
{
	#region Monster State Variables
	public bool isBouncy;
    public bool isFloating;
    public bool isCrying;
	public bool gravityAttraction;
    public bool hypergravity;
    public bool reverseGravity;
	public bool facingRight = true;
	public bool soundPlayed = false;
	float effectTimer;
	float velocityCap;
	StatesOfWater tearState;
    public Animator anim;
	#endregion

	#region References to Other Pieces of Monster
	public GameObject tear;
    public Transform monsterHead;
    public Transform raycastPoint;
    Rigidbody2D rigidBody;
	SpriteRenderer spriteRenderer;
	#endregion
    
	#region Monster Wander Variables
	public bool isWandering;
    public float wanderSpeed;
    public float leftBoundary;
    public float rightBoundary;
    Vector3 wanderTo;
    Vector3 nextPos;
	#endregion  

	#region Heavy Metal Squash Variables
	float origXScale;
	float origYScale;
	bool isHeavyLooking;
	#endregion

	#region Private Variables Exposed in Inspector
	[SerializeField]
	float gravityAttractionRadius;
	[SerializeField]
	float resetTime;
	#endregion

	#region Properties
	float EffectTimer
	{
		get{ return effectTimer; }
	}

	void ResetEffectTimer()
	{
		effectTimer = resetTime;
	}
	#endregion

	/// <summary>
	/// Receive a genre of music from the player
	/// </summary>
	/// <param name="musicType">The type of music being received</param>
    public void ReceiveMusic(TypesOfMusic musicType)
    {
		// Switch to a different state based on what type of music monster is hearing
        switch(musicType)
        {
            case TypesOfMusic.Blues:
                SetCrying(StatesOfWater.Liquid);
                break;
			case TypesOfMusic.Choral:
				SetBouncy();
				//SetReverseGravity();
                break;
            case TypesOfMusic.HeavyMetal:
                SetHypergravity();
                break;
            case TypesOfMusic.BluesChoral:
                SetCrying(StatesOfWater.Gas);
                break;
            case TypesOfMusic.BluesHeavyMetal:
                SetCrying(StatesOfWater.Solid);
                break;
            case TypesOfMusic.ChoralHeavyMetal:
				SetReverseGravity();
                //SetGravityAttraction();
                break;
            default:
                break;
        }

        ResetEffectTimer();
    }


	// Use this for initialization
	void Start()
    {
		isBouncy = false;
        isCrying = false;
        isFloating = false;
		gravityAttraction = false;
		hypergravity = false;
        reverseGravity = false;
        tearState = StatesOfWater.Liquid;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wanderTo = new Vector3(transform.position.x + rightBoundary, transform.position.y, transform.position.z);
        nextPos = new Vector3(transform.position.x + leftBoundary, transform.position.y, transform.position.z);
        velocityCap = 5f;
        GetComponent<SpriteRenderer>().flipX = facingRight;

		origXScale = transform.localScale.x;
		origYScale = transform.localScale.y;
		isHeavyLooking = false;

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update()
    {
        rigidBody.AddForce(Vector2.zero);

		// If floating, handle float
        if(isFloating)
        {
            HandleFloat();
        }

		// If crying, handle crying
        if(isCrying)
        {
            HandleCrying();
        }

		// If wandering, move toward next wander position
        if(isWandering)
        {
            anim.SetBool("IsWalking", true);

            wanderTo = new Vector3(wanderTo.x, transform.position.y, transform.position.z);
            nextPos = new Vector3(nextPos.x, transform.position.y, transform.position.z);

            if(Vector3.Distance(transform.position, wanderTo) < 0.5)
            {
                Vector3 temp = wanderTo;
                wanderTo = nextPos;
                nextPos = temp;
                ToggleFlip();
            }

			// Wander using raycasting
            RaycastingWandering();

            Vector2 direction = wanderTo - nextPos;
            direction.Normalize();
            //transform.position = Vector3.MoveTowards(transform.position, wanderTo, wanderSpeed * Time.deltaTime);
            rigidBody.velocity = new Vector2(wanderSpeed * direction.x, rigidBody.velocity.y);
            if(Mathf.Abs(rigidBody.velocity.y) > velocityCap)
            {
                //rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y / rigidBody.velocity.y * velocityCap);
            }
        }
        else
        {
            anim.SetBool("IsIdle", true);
        }

		// If there's currently an effect on this monster, subtract from the effect timer
		if(effectTimer > 0)
		{
			effectTimer -= Time.deltaTime;
		}

		// Otherwise, reset this monster's state
		else
		{
			ResetState();
		}

		// Handle heavy metal squash
		if(hypergravity && !isHeavyLooking)
		{
			isHeavyLooking = true;
			transform.localScale = new Vector3(transform.localScale.x * 1.2f, transform.localScale.y * 0.7f, 1f);
			transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
        }
		else if(!hypergravity && isHeavyLooking)
		{
			isHeavyLooking = false;
			transform.localScale = new Vector3(origXScale, origYScale, 1f);
			transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
        }

		// If this monster is a gravity well, handle that
		if(gravityAttraction)
		{
			HandleGravityAttraction();
		}
	}

	#region Wandering Functions
	/// <summary>
	/// Wander based on a raycast in front of the monster's feet
	/// </summary>
    void RaycastingWandering()
    {
        Vector2 origin, direction;
        float distance;
        origin = new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.center.y - spriteRenderer.bounds.extents.y/2);//new Vector2(spriteRenderer.bounds.center.x, raycastPoint.position.y);
        direction = new Vector2(wanderTo.x - nextPos.x, 0);
        direction.Normalize();
        distance = spriteRenderer.bounds.extents.x;
        RaycastHit2D raycastHit = Physics2D.Raycast(origin, direction, distance, 1 << LayerMask.NameToLayer("Default"));

        if(raycastHit.collider != null)
        {
            if (raycastHit.collider.gameObject.CompareTag("Player"))
                return;
            if (!raycastHit.collider.isTrigger)
            {
                Vector3 temp = wanderTo;
                wanderTo = nextPos;
                nextPos = temp;
                ToggleFlip();
            }
        }
    }
	#endregion

	#region Monster State Functions
	#region Crying Functions
	/// <summary>
	/// Set this monster to cry
	/// </summary>
	/// <param name="waterState">The state of matter of the tears</param>
    public void SetCrying(StatesOfWater waterState)
    {
        if(!isCrying || tearState != waterState)
        {
            ResetState();
            isCrying = true;
            tearState = waterState;
        }
    }

	/// <summary>
	/// If this monster is already crying, create tears
	/// </summary>
    public void HandleCrying()
    {
        GameObject tearObject = Instantiate(tear);

        if (tearState == StatesOfWater.Gas)
        {
            tearObject.GetComponent<TearController>().TransToGas();
			//Play sound once
			if (!soundPlayed) {
				AudioSource steamSound = GetComponents<AudioSource> () [0];
				steamSound.Play();
				soundPlayed = true;
			}
            anim.SetBool("IsSteaming", true);
        }
        else if (tearState == StatesOfWater.Solid)
        {
            tearObject.GetComponent<TearController>().TransToIce();
            anim.SetBool("IsFreezing", true);
        }
        else
        {
            tearObject.GetComponent<TearController>().TransToWater();
            anim.SetBool("IsCrying", true);
        }

        tearObject.transform.position = monsterHead.gameObject.GetComponent<BoxCollider2D>().bounds.center;
        tearObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(0f, 5.0f));
    }
	#endregion



	#region Bouncy Functions
	public void SetBouncy()
	{
		ResetState();
		isBouncy = true;
        anim.SetBool("IsChoral", true);
    }
	#endregion

	#region Floating Functions
	/// <summary>
	/// Set this monster to float
	/// </summary>
    public void SetFloat()
    {
        if(!isFloating)
        {
            ResetState();
            isFloating = true;
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(0f, 1.0f);
        }
    }

	/// <summary>
	/// If this monster is floating, make it hover
	/// </summary>
    void HandleFloat()
    {
        if (rigidBody.velocity != Vector2.zero)
        {
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, Vector2.zero, Time.deltaTime);
        }
    }
	#endregion

	#region Heavy Functions
	/// <summary>
	/// Sets this monster to be heavy
	/// </summary>
    public void SetHypergravity()
    {
        if(!hypergravity || reverseGravity)
        {
            ResetState();
            anim.SetBool("IsHeavy", true);
            hypergravity = true;
            rigidBody.gravityScale = 5f;
        }
    }
	#endregion

	#region Reverse Gravity Functions
	/// <summary>
	/// Sets this monster to have its gravity reversed
	/// </summary>
    public void SetReverseGravity()
    {
        if(!reverseGravity)
        {
            ResetState();
            reverseGravity = true;
            Invoke("DelayedHyperGravity", 0.3f);

            //Insert animation trigger for mashup here
            anim.SetBool("IsReverseGravity", true);
            rigidBody.gravityScale = -5f;
        }
    }

    void DelayedHyperGravity()
    {
        hypergravity = true;
    }

	#endregion

	#region Gravity Attraction Functions
	/// <summary>
	/// Sets this monster as a gravity well
	/// </summary>
	public void SetGravityAttraction()
	{
		if(!gravityAttraction)
		{
			ResetState();
			gravityAttraction = true;
		}
	}

	/// <summary>
	/// If this monster is a gravity well, attract other monsters and the player
	/// </summary>
	void HandleGravityAttraction()
	{
		Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, gravityAttractionRadius);

		int monsterCount = 1;
		foreach(Collider2D c in objects)
		{
			if(c.gameObject.tag == "Monster" && c.gameObject != gameObject)
			{
				monsterCount++;
				PullObjectToward(c);
			}
		}

		foreach(Collider2D c in objects)
		{
			if(c.gameObject.tag == "Player")
			{
				PullObjectToward(c, monsterCount);
			}
		}
	}

	/// <summary>
	/// Pulls an object toward this monster
	/// Used for gravity well
	/// </summary>
	/// <param name="c">The collider to pull toward this monster</param>
	/// <param name="monsterCount">The number of monsters that are pulling the other collider at once</param>
	void PullObjectToward(Collider2D c, int monsterCount = 1)
	{
		float distance = Vector2.Distance(transform.position, c.transform.position);

		Vector2 pullDirection = transform.position - c.transform.position;
		pullDirection.Normalize();


		if(c.gameObject.tag == "Player" && Time.timeScale != 0)
		{
			c.attachedRigidbody.AddForce(((50f / monsterCount) / distance) * pullDirection);
		}
		else
		{
			c.attachedRigidbody.AddForce((2f / distance) * pullDirection);
		}
	}
	#endregion

	/// <summary>
	/// Reset the state of this monster
	/// </summary>
	public void ResetState()
	{
		isBouncy = false;
		isCrying = false;
		isFloating = false;
		gravityAttraction = false;
		hypergravity = false;
        reverseGravity = false;
		soundPlayed = false;
        anim.SetBool("IsChoral", false);
        anim.SetBool("IsCrying", false);
        anim.SetBool("IsHeavy", false);
        anim.SetBool("IsReverseGravity", false);
        anim.SetBool("IsFreezing", false);
        anim.SetBool("IsSteaming", false);
        tearState = StatesOfWater.Liquid;
		rigidBody.gravityScale = 1f;
	}
	#endregion
		
	#region Event Handlers
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerController>().groundCheck.transform.position.y <= transform.position.y)
            {
                StartCoroutine(other.gameObject.GetComponent<PlayerController>().BackToCheckPoint());
            }
        }
    }
	#endregion

	#region Visuals Functions
    /// <summary>
    /// Toggles NPC sprite flip
    /// </summary>
    void ToggleFlip()
    {
        if(facingRight)
        {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = facingRight;
        }
        else
        {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = facingRight;
        }
    }
    #endregion
}
#endregion