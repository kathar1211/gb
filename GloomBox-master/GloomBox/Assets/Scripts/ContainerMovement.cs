#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region ContainerMovement Class
public class ContainerMovement : MonoBehaviour
{
	#region Public Variables
    public Transform topLimit;
    public Animator anim;
    #endregion

    #region Private Variables
    Vector3 tlpos;
	#endregion
    
    // Use this for initialization
	void Start()
    {
		// Take the top limit transform and save it in a Vector3
        tlpos = topLimit.position;

        anim = GetComponent<Animator>();
        anim.Play("GlissIdle");
    }
	
	// Update is called once per frame
	void Update()
    {
		// TODO: Make a global clamp function
		// Clamp container movement within a boundary
	    if(transform.position.y > tlpos.y)
        {
            transform.position = tlpos;
        }
	}
}
#endregion