#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region GroundCheckController Class
public class GroundCheckController : MonoBehaviour
{
	#region Public Variables
    public bool canJump;
    public LayerMask whatIsGround;
    public float groundRadius = 0.2f;
    public bool grounded = false;

    #endregion

    #region Event Handlers
    /// <summary>
    /// Called when a collider enters the ground check
    /// </summary>
    /// <param name="other">The collider that entered</param>

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(transform.position, groundRadius, whatIsGround);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
		// If the collider that entered isn't a trigger, the player can jump
        if(!other.isTrigger)
        {
            if(!other.CompareTag("MonsterHead"))
            {
                transform.parent.gameObject.GetComponent<PlayerController>().bouncing = false;
            }

            if (!canJump)
            {
                canJump = !canJump;
            }

            //When landed, reset gravity scale
            transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10f;

            Vector2 vel = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;

			// Prevent bouncing or going into the floor
            transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(vel.x, 0);
        }
    }

	/// <summary>
	/// Called when a collider stays in the ground check for more than one frame
	/// </summary>
	/// <param name="other">The collider that stayed</param>
    void OnTriggerStay2D(Collider2D other)
    {
		// If the collider that stayed isn't a tirgger, the player can jump
        if(!other.isTrigger)
        {
            if (!other.CompareTag("MonsterHead"))
            {
                transform.parent.gameObject.GetComponent<PlayerController>().bouncing = false;
            }

            if (!canJump)
            {
                canJump = !canJump;
            }

            //When landed, reset gravity scale
            transform.parent.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10f;
        }

        // If the collider that stayed is a moving platform, notify the player that they are on a moving platform
        if (other.CompareTag("MovingPlatform"))
        {
            transform.parent.gameObject.GetComponent<PlayerController>().setMovingPlatform(true, other.gameObject.GetComponent<Rigidbody2D>().velocity);
        }
    }

	/// <summary>
	/// Called when a collider exits the ground check
	/// </summary>
	/// <param name="other">The collider that exited</param>
    void OnTriggerExit2D(Collider2D other)
    {
		// If the collider that exited isn't a trigger, the player can't jump
        if(!other.isTrigger)
        {
            if(canJump)
            {
                StartCoroutine(DelayJump());
            }
        }

		// If the collider that exited is a moving platform, notify the player that they are no longer on a moving platform
        if(other.CompareTag("MovingPlatform"))
        {
            transform.parent.gameObject.GetComponent<PlayerController>().setMovingPlatform(false, Vector2.zero);
        }
    }

    //Make the jump forgivable
    IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(0.2f);
        canJump = false;
    }
	#endregion
}
#endregion