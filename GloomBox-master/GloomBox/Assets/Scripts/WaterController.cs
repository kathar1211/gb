#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region WaterController Class
public class WaterController : MonoBehaviour
{
	#region Public Variables
    public float buoyancy;
    #endregion

    bool isFull;
    Transform museTrans;

    // Use this for initialization
    void Start()
    {
        museTrans = GameObject.FindGameObjectWithTag("Player").transform;

        //GetComponent<SpriteRenderer>().enabled = false;
        isFull = false;
        if (GetComponent<SpriteRenderer>().enabled)
        {
            //Debug.Log(GetComponent<WaterRenderer>().waterDepth);
            GetComponent<WaterRenderer>().waterDepth = GetComponent<BoxCollider2D>().bounds.size.y;
            //Debug.Log(GetComponent<WaterRenderer>().waterDepth);
            isFull = true;
        }

        GetComponent<WaterRenderer>().enabled = false;
    }
    void Update()
    {
        if(museTrans == null)
        {
            museTrans = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if(museTrans != null)
        {
            if(museTrans.position.x >= (transform.position.x - 20) && !GetComponent<WaterRenderer>().enabled)
            {
                GetComponent<WaterRenderer>().enabled = true;
            }

            if((museTrans.position.x - transform.position.x) > (GetComponent<BoxCollider2D>().bounds.size.x + 10))
            {
                if(GetComponent<WaterRenderer>().enabled)
                    GetComponent<WaterRenderer>().enabled = false;
            }
        }

        if (GetComponent<SpriteRenderer>().enabled)
            if (GetComponent<WaterRenderer>().waterDepth < GetComponent<BoxCollider2D>().bounds.size.y + 0.5f)
            {
                //Debug.Log(GetComponent<WaterRenderer>().waterDepth);
                GetComponent<WaterRenderer>().waterDepth += Time.deltaTime;
                //Debug.Log(GetComponent<WaterRenderer>().waterDepth);
            }
            else isFull = true;
    }

	#region Event Handlers
	/// <summary>
	/// Called when a collider stays touching this water
	/// </summary>
	/// <param name="other">The collider that is staying touching</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if(GetComponent<SpriteRenderer>().enabled)
        {
			// Floating platforms float
            if(other.CompareTag("FloatingPlatform"))
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3.2f * buoyancy);
            }

			// Light monsters float, heavy monsters sink
            else if(other.CompareTag("Monster") && !other.gameObject.GetComponent<MonsterController>().hypergravity)
            {
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, buoyancy);
            }

			// Players die if they touch water
            else if(other.CompareTag("Player"))
            {
                if(GetComponent<SpriteRenderer>().enabled && isFull)
                {
					StartCoroutine(other.gameObject.GetComponent<PlayerController>().BackToCheckPoint());
                }
            }
        }
    }
    
	/// <summary>
	/// Called when a collider enters this water
	/// </summary>
	/// <param name="other">The collider that entered</param>
    void OnTriggerEnter2D(Collider2D other)
    {
		// If a liquid tear touches water area, water becomes visible in that area
        if(other.CompareTag("Tear"))
        {
			if(other.gameObject.GetComponent<TearController>().state == StatesOfWater.Liquid)
			{
				GetComponent<SpriteRenderer>().enabled = true;
			}
        }
    }

	// Makes sure floating platforms don't bounce on top of water
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("FloatingPlatform"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
	#endregion
}
#endregion