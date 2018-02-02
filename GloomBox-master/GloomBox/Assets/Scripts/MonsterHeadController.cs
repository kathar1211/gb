#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region MonsterHeadController Class
public class MonsterHeadController : MonoBehaviour
{
	#region Public Variables
    public float bounceForce;
	#endregion

	#region Event Handlers
	/// <summary>
	/// Handle what happens when the player stays collided with the monster's head
	/// </summary>
	/// <param name="other">The collision that contains the monster's head and another GameObject</param>
    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().bouncing = true;
            // If the player's height is lower than the monster's head height, kill the player
            if (other.gameObject.transform.position.y < transform.position.y)
            {
				StartCoroutine(other.gameObject.GetComponent<PlayerController>().BackToCheckPoint());
                return;
            }
            //if(GetComponent<Rigidbody2D>().velocity.y != 0)
            //other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);

			// If the player touches the top of the enemy's head, give it a bounce force up
			else
			{
				if(other.gameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
				{
					AudioSource bounce1 = GetComponents<AudioSource>()[0];
					AudioSource bounce2 = GetComponents<AudioSource>()[1];
					bounce2.Play ();
                    GetComponentInParent<MonsterController>().anim.SetTrigger("IsBouncing");

                    if(transform.parent.gameObject.GetComponent<MonsterController>().isBouncy)
					{
                        if(!other.gameObject.GetComponent<PlayerController>().JustBounced)
                        {

                            other.gameObject.GetComponent<PlayerController>().JustBounced = true;
                            other.gameObject.GetComponent<PlayerController>().HandleJump(true, new Vector2(0f, bounceForce));
						
                        }
                    }
					else
					{
                        if (!other.gameObject.GetComponent<PlayerController>().JustBounced)
                        {
                            other.gameObject.GetComponent<PlayerController>().JustBounced = true;
                            other.gameObject.GetComponent<PlayerController>().HandleJump(true, new Vector2(0f, bounceForce / 2f));
                        }
					}
                }
			}
        }
    }

	/// <summary>
	/// Handle what happens when the player collides with the monster's head
	/// </summary>
	/// <param name="other">The collision that contains the monster's head and another GameObject</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
			// If the monster has its gravity reversed, push the player off the side of the monster
			if(transform.parent.gameObject.GetComponent<MonsterController>().reverseGravity)
			{
				if(other.gameObject.GetComponent<PlayerController>().CheckWallRight())
				{
					other.gameObject.GetComponent<PlayerController>().HandleJump(true, new Vector2(-2f * bounceForce, bounceForce / 3f));
				}
				else
				{
					other.gameObject.GetComponent<PlayerController>().HandleJump(true, new Vector2(2f * bounceForce, bounceForce / 3f));
				}
			}

			// If the player's height is lower than the monster's head height, kill the player
            if(other.gameObject.transform.position.y < transform.position.y)
            {
				StartCoroutine(other.gameObject.GetComponent<PlayerController>().BackToCheckPoint());
                return;
            }

            //other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);
        }
    }
	#endregion
}
#endregion