#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region BreakablePlatform Class
public class BreakablePlatform : MonoBehaviour
{
	#region Public Variables
    public Sprite breakingSprite;
	#endregion

	#region Private Variables
	AudioSource beginBreakingSound;
	AudioSource breakSound;
	bool broken = false;
	#endregion

	#region Private Variables Exposed in Inspector
	[SerializeField]
	float breakDelay;
	#endregion

	void Start()
	{
		beginBreakingSound = GetComponents<AudioSource>()[0];
		breakSound = GetComponents<AudioSource>()[1];
	}

	#region Event Handlers
	/// <summary>
	/// When an object stays in the collider of this object for more than a frame
	/// </summary>
	/// <param name="other">The collision event between this object and the colliding object</param>
	void OnCollisionStay2D(Collision2D other)
    {
		// Can't break multiple times
        if(!broken)
        {
			// If the colliding object is a heavy monster, break
            if(other.gameObject.CompareTag("Monster"))
            {
                if(other.gameObject.GetComponent<MonsterController>().hypergravity)
                {
                    broken = true;
                    StartCoroutine(Break());
                }
            }

			// Otherwise, if the colliding object is a full container, break
            else if(other.gameObject.CompareTag("Container"))
            {
                if(other.gameObject.GetComponentInChildren<ContainerController>().isFull)
                {
                    broken = true;
                    StartCoroutine(Break());
                }
            }
        }
    }
	#endregion

	#region Coroutines
	/// <summary>
	/// Breaks this platform after the specified delay
	/// </summary>
	IEnumerator Break()
	{
        // Play begin breaking sound and switch sprite
		beginBreakingSound.Play();
        gameObject.GetComponent<SpriteRenderer>().sprite = breakingSprite;

		// Wait for specified delay
        yield return new WaitForSeconds(breakDelay);

		breakSound.Play();
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;

		// Destroy the platform
		Destroy(gameObject, .75f);
	}
	#endregion
}
#endregion