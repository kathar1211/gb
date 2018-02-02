using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StillSwapper : MonoBehaviour
{

    public GameObject[] Stills;
    private int stillCounter = 0;
    private bool interactable = true;


	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			if(stillCounter > 0)
			{
				if(interactable)
				{
					--stillCounter;
					Stills[stillCounter].GetComponent<SpriteRenderer>().enabled = true;

					if(Stills[stillCounter].GetComponentsInChildren<SpriteRenderer>().Length > 1)
					{
						Stills[stillCounter].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
					}

					//StartCoroutine(DelayInput());
				}
			}
		}
	    else if(Input.anyKeyDown)
        {
            if(stillCounter < Stills.Length)
            {
                if(interactable)
                {
                    Stills[stillCounter].gameObject.GetComponent<SpriteRenderer>().enabled = false;

                    if (Stills[stillCounter].gameObject.GetComponentsInChildren<SpriteRenderer>().Length > 1)
                    {
                        Stills[stillCounter].GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                    }

                    ++stillCounter;

					if (stillCounter == 3) {
						AudioSource Closebox = GetComponents<AudioSource> () [1];
						Closebox.Play ();
					} else if (stillCounter == 4) {
						AudioSource oldMusic = GetComponents<AudioSource> () [3];
						AudioSource buttonP = GetComponents<AudioSource> () [2];
						AudioSource newMusic = GetComponents<AudioSource> () [4];
						buttonP.Play ();
						oldMusic.Stop ();
						newMusic.Play ();
					} else if (stillCounter == 5) {
						AudioSource shimmer = GetComponents<AudioSource> () [5];
						shimmer.Play ();
					}
                  

                    //StartCoroutine(DelayInput());

                }
            }
            
			if(stillCounter == Stills.Length)
            {
                StartCoroutine(Flash());
            }
        }
	}

    IEnumerator DelayInput()
    {
        interactable = false;
        yield return new WaitForSeconds(0.3f);
        interactable = true;
    }

    IEnumerator Flash()
    {
        float whiteValue = 1;
        while (whiteValue > 0)
        {
            whiteValue -= 0.01f;
            GetComponent<Camera>().backgroundColor = new Color(whiteValue, whiteValue, whiteValue);
            yield return null;
        }

		//Fade out bg music
		AudioSource newMusic = GetComponents<AudioSource> () [4];
		float startVolume = newMusic.volume;
		float FadeTime=3;

		while (newMusic.volume > 0) {
			newMusic.volume -= startVolume * Time.deltaTime / FadeTime;

			yield return null;
		}

		newMusic.Stop ();
		newMusic.volume = startVolume;

        SceneManager.LoadScene("Level0_Tutorial");
    }
}