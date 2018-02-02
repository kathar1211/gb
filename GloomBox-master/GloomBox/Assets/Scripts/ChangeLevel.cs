using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeLevel : MonoBehaviour
{
	[SerializeField]
	string nextLevelName;

	GameObject muse;

	// Use this for initialization
	void Start()
	{
		muse = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.Rotate(Vector3.forward, 1f);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			StartCoroutine("LoadNextLevel");
		}
	}

	IEnumerator LoadNextLevel()
	{
		muse.GetComponent<PlayerController>().InputEnabled = false;
		muse.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		muse.GetComponent<Animator>().SetBool("IsRunning", false);
        if(muse.GetComponent<Animator>().GetBool("IsJumping") == true)
        {
            muse.GetComponent<Animator>().SetBool("IsJumping", false);
            //yield return new WaitForSeconds(1f);
            
        }
        muse.GetComponent<Animator>().SetBool("IsWinning", true);
		yield return new WaitForSeconds(3f);
        muse.GetComponent<Animator>().SetBool("IsWinning", false);
        LevelManager.ClearData();
		SceneManager.LoadScene(nextLevelName);
	}
}