using UnityEngine;
using System.Collections;

public class TapeCollectible : MonoBehaviour
{
	GameObject muse;
	public TypesOfMusic type;

	[SerializeField]
	AudioClip bluesCollection;
	[SerializeField]
	AudioClip choralCollection;
	[SerializeField]
	AudioClip metalCollection;

	// Use this for initialization
	void Start()
	{
		muse = GameObject.Find("Muse");

		switch(type)
		{
			case TypesOfMusic.Blues:
				if(muse.GetComponent<PlayerController>().BluesEnabled)
				{
					Destroy(this.gameObject);
				}
				else
				{
					GetComponents<AudioSource>()[1].clip = bluesCollection;
				}
				break;
			case TypesOfMusic.Choral:
				if(muse.GetComponent<PlayerController>().ChoralEnabled)
				{
					Destroy(this.gameObject);
				}
				else
				{
					GetComponents<AudioSource>()[1].clip = choralCollection;
				}
				break;
			case TypesOfMusic.HeavyMetal:
				if(muse.GetComponent<PlayerController>().HeavyMetalEnabled)
				{
					Destroy(this.gameObject);
				}
				else
				{
					GetComponents<AudioSource>()[1].clip = metalCollection;
				}
				break;
			default:
				break;
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(Vector3.up, 0.8f);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			GetComponents<AudioSource>()[0].Play();
			GetComponents<AudioSource>()[1].Play();
			muse.GetComponent<PlayerController>().SetGenreEnabledOrDisabled(type, true);
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
			GetComponentInChildren<MeshRenderer>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponentInChildren<BoxCollider>().enabled = false;
			Destroy(this.gameObject, 5f);
		}
	}
}