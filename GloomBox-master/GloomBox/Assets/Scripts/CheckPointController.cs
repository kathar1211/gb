#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region CheckPointController Class
public class CheckPointController : MonoBehaviour
{
    [SerializeField]
    Sprite checkpointActivated;
	bool played=false;

	#region Event Handlers
	// When the player collides with this object, activate the checkpoint
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
			AudioSource fireLit = GetComponents<AudioSource> () [0];
            other.gameObject.GetComponent<PlayerController>().SetCheckPoint(transform);


            // TODO
            // Unnecessary load here? Could potentially switch to a publicly exposed sprite variable
            //GetComponent<SpriteRenderer>().sprite = Resources.Load("checkpoint", typeof(Sprite)) as Sprite;
            GetComponent<SpriteRenderer>().sprite = checkpointActivated;
			if (!played) {
				fireLit.Play ();
				played = true;
			}
        }
    }
	#endregion
}
#endregion