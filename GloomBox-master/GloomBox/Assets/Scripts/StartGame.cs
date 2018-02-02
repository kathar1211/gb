#region Using Directives
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#endregion

#region StartGame Class
public class StartGame : MonoBehaviour
{
	#region Public Variables
    public string levelName;
	#endregion

	// Update is called once per frame
	void Update()
    {
		// Load level on any key press
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
#endregion