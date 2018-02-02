#region Using Directives
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

#region ContainerController Class
public class ContainerController : MonoBehaviour
{
	#region Private Variables
    List<GameObject> particlesInside;
	#endregion

	#region Public Variables
    public bool isFull;
	#endregion

    // Use this for initialization
	void Start()
    {
        particlesInside = new List<GameObject>();
        isFull = false;
	}
	
	// Update is called once per frame
	void Update()
    {
		// C# delegate wizardry
		// Loop through the list and remove all null objects
		// This is done to clear out particles that have removed themselves from the container and the list
        particlesInside.RemoveAll((o) => o == null);

		// Full containers must have at least 10 particles inside them
		if(particlesInside.Count >= 5)
		{
			isFull = true;
		}
		else
		{
			isFull = false;
		}
        
	}

	#region Event Handlers
	// If a tear enters the container, make it a physics object and add it to the list
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Tear"))
        {
            other.gameObject.GetComponent<Collider2D>().isTrigger = false;
            particlesInside.Add(other.gameObject);
        }
    }

	// If a tear exits the container, make it a non-physics object and remove it from the list
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Tear"))
        {
            other.gameObject.GetComponent<Collider2D>().isTrigger = true;
            particlesInside.Remove(other.gameObject);
        }
    }
	#endregion
}
#endregion