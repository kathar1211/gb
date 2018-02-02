using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScrollingCredits : MonoBehaviour
{
	[SerializeField]
	GameObject fadeout;
	[SerializeField]
	Camera mainCam;
	[SerializeField]
	Vector3 scrollVector;

	float atEndCount;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if(transform.position.y < 133f)
		{
			transform.Translate(scrollVector);
		}
		else
		{
			atEndCount += Time.deltaTime;

			fadeout.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f + Mathf.Max(0f, ((atEndCount - 3f) / 3f)));
			mainCam.GetComponent<AudioSource>().volume = 1f - Mathf.Max(0f, ((atEndCount - 3f) / 3f));

			if(atEndCount > 8f)
			{
				SceneManager.LoadScene("MainMenu");
			}
		}

		if(Input.anyKeyDown)
		{
			if(transform.position.y < 133f)
			{
				transform.position = new Vector3(0f, 133f, 0f);
			}
			else
			{
				SceneManager.LoadScene("MainMenu");
			}
		}
	}
}