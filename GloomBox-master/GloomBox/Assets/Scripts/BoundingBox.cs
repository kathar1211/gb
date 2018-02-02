using UnityEngine;
using System.Collections;

public class BoundingBox : MonoBehaviour
{
	Bounds b;
	GameObject muse;

	// Use this for initialization
	void Start()
	{
		muse = GameObject.FindGameObjectWithTag("Player");
		b = GetComponent<BoxCollider2D>().bounds;
		GetComponent<BoxCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(!b.Contains(muse.transform.position))
		{
			GameObject.Find("LevelManager").GetComponent<LevelManager>().ResetLevel();
		}
	}

	/*void OnTriggerExit2D(Collider2D other)
	{
		print(other.name);
		if(other.gameObject.CompareTag("Player"))
		{
			GameObject.Find("LevelManager").GetComponent<LevelManager>().ResetLevel();
		}
	}*/
}