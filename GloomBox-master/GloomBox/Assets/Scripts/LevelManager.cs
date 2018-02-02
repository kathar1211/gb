#region Using Directives
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
#endregion

#region LevelManager Class
public class LevelManager : MonoBehaviour
{
	#region Private Variables
	static string filepath;
    GameObject muse;
	#endregion

	// Start is used for initialization
	void Awake()
	{
        muse = GameObject.FindGameObjectWithTag("Player");
		muse.GetComponent<PlayerController>().FindUIButtons();
		filepath = "Resources/resetData.txt";
		LoadLevel();
	}

	void Update()
	{
		if(KeyboardState.keysPressed.Contains(KeyCode.LeftShift) || KeyboardState.keysPressed.Contains(KeyCode.RightShift))
		{
			if(KeyboardState.keysJustPressed.Contains(KeyCode.Alpha1))
			{
				ClearData();
				SceneManager.LoadScene("Level0_Tutorial");
			}
			else if(KeyboardState.keysJustPressed.Contains(KeyCode.Alpha2))
			{
				ClearData();
				SceneManager.LoadScene("Level1_DockEntrance");
			}
			else if(KeyboardState.keysJustPressed.Contains(KeyCode.Alpha3))
			{
				ClearData();
				SceneManager.LoadScene("Level2_DockMain");
			}
			else if(KeyboardState.keysJustPressed.Contains(KeyCode.Alpha4))
			{
				ClearData();
				SceneManager.LoadScene("Level3_Cathedral");
			}
			else if(KeyboardState.keysJustPressed.Contains(KeyCode.Alpha0))
			{
				ClearData();
				SceneManager.LoadScene("MainMenu");
			}
		}
	}

	#region File Loading and Unloading
	/// <summary>
	/// Load a level from the specified filepath
	/// </summary>
	public void LoadLevel()
	{
		// Make sure the file exists
		if(File.Exists(filepath))
		{
			string line;

			// Create a new StreamReader, tell it which file to read and what encoding the file was saved as
			StreamReader sr = new StreamReader(filepath);

			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using(sr)
			{
				// While there's lines left in the text file, do this:
				do
				{
					line = sr.ReadLine();

					if(line != null)
					{
						// Do whatever you need to do with the text line, it's a string now
						// In this example, I split it into arguments based on comma
						// deliniators, then send that array to DoStuff()
						string[] entries = line.Split(',');
						if(entries.Length > 0)
						{
							muse.transform.position = new Vector3(float.Parse(entries[0]), float.Parse(entries[1]), float.Parse(entries[2]));
							muse.GetComponent<PlayerController>().SetGenreEnabledOrDisabled(TypesOfMusic.Blues, System.Convert.ToBoolean(int.Parse(entries[3])));
							muse.GetComponent<PlayerController>().SetGenreEnabledOrDisabled(TypesOfMusic.HeavyMetal, System.Convert.ToBoolean(int.Parse(entries[4])));
							muse.GetComponent<PlayerController>().SetGenreEnabledOrDisabled(TypesOfMusic.Choral, System.Convert.ToBoolean(int.Parse(entries[5])));
						}
					}
				} while(line != null);

				// Done reading, close the reader and return true to broadcast success    
				sr.Close();
			}
		}
	}

	/// <summary>
	/// Begin the reset process by writing the file that will be used to reload the level
	/// </summary>
	public void ResetLevel()
	{
		// If there's no Resources folder yet, make one
		if(!Directory.Exists("Resources"))
		{
			Directory.CreateDirectory("Resources");
		}
			
		File.Create(filepath).Dispose();
		StreamWriter sw = new StreamWriter(filepath);

		// Write the player's last checkpoint position to the file
		using(sw)
		{
			Vector3 position = muse.GetComponent<PlayerController>().GetCheckPoint().position;
			int bluesEnabled = System.Convert.ToInt32(muse.GetComponent<PlayerController>().BluesEnabled);
			int heavyMetalEnabled = System.Convert.ToInt32(muse.GetComponent<PlayerController>().HeavyMetalEnabled);
			int choralEnabled = System.Convert.ToInt32(muse.GetComponent<PlayerController>().ChoralEnabled);
			string toWrite = position[0] + "," + position[1] + "," + position[2] + "," + bluesEnabled + "," + heavyMetalEnabled + "," + choralEnabled;
			sw.WriteLine(toWrite);
		}

		// Reload the current scene
		// When it reloads, it will pull the player position from the file that was just written
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	/// <summary>
	/// Delete the file when the application quits
	/// No save state as of yet
	/// </summary>
	void OnApplicationQuit()
	{
		// If the load file exists, delete it
		if(File.Exists(filepath))
		{
			File.Delete(filepath);
		}

		// If the Resources folder exists, delete it recursively
		// Removed because keymap also uses the Resources folder
		/*if(Directory.Exists("Resources"))
		{
			Directory.Delete("Resources", true);
		}*/
	}

	public static void ClearData()
	{
		// If the load file exists, delete it
		if(File.Exists(filepath))
		{
			File.Delete(filepath);
		}
	}
	#endregion
}
#endregion