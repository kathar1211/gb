#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region FloatingPlatformController Class
public class FloatingPlatformController : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.zero);                
	}

    void OnCollisionStay2D( Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 10f;
        }
    }
}
#endregion