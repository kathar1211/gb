#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region FrozenController Class
public class FrozenController : MonoBehaviour
{
	#region Public Variables
    public bool isFrozen;
	public bool soundPlayed;
    public int iceAmount = 0;
    public int FrozenThreshhold = 0;
	#endregion

    // Use this for initialization
	void Start()
    {
        isFrozen = false;
		soundPlayed = false;
	}

	/// <summary>
	/// Freeze the attached GameObject in whatever state it is currently in
	/// </summary>
    
    void Update()
    {
        if(iceAmount > FrozenThreshhold)
        {
            isFrozen = true;
			//Play sound once
			if (!soundPlayed) {
				
				AudioSource iceSound = GetComponents<AudioSource> () [0];
				iceSound.Play ();
				soundPlayed = true;
			}

            if(GetComponentsInChildren<SpriteRenderer>().Length>1)
            {
                GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
            }
        }
        else
        {
            isFrozen = false;
			soundPlayed = false;

            if(GetComponentsInChildren<SpriteRenderer>().Length>1)
            {
                GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            }
        }
    }

	public void Freeze()
    {
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //isFrozen = true;
        iceAmount++;
    }

    public void Defreeze()
    {
        iceAmount--;
    }
		
}
#endregion