using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RemapKeys : MonoBehaviour
{
	public List<Button> changeButtonList;

	Dictionary<int, string> changeButtonToInput;
	bool changingInput;
	bool changingInputIsAxis;
	bool inputEnabled;
	int selectedChangeButton;
	InputTypes type;

	string filepath = "Resources/keymap.txt";

	// Use this for initialization
	void Start()
	{
		changeButtonToInput = new Dictionary<int, string>();
		changeButtonToInput.Add(0, "left");
		changeButtonToInput.Add(1, "right");
		changeButtonToInput.Add(2, "jump");
		changeButtonToInput.Add(3, "metal");
		changeButtonToInput.Add(4, "blues");
		changeButtonToInput.Add(5, "choral");
		changeButtonToInput.Add(6, "play");
		changeButtonToInput.Add(7, "stop");
		changeButtonToInput.Add(8, "reset");
		changeButtonToInput.Add(9, "up");
		changeButtonToInput.Add(10, "down");

		changingInput = false;
		inputEnabled = true;
		selectedChangeButton = 0;

		if(Input.GetJoystickNames().Length == 0)
		{
			type = InputTypes.Keyboard;
		}
		else
		{
			type = (Input.GetJoystickNames()[0] != "") ? InputTypes.Controller : InputTypes.Keyboard;
		}

		if(File.Exists(filepath))
		{
			LoadKeymap(type);
		}
		else
		{
			CreateKeymap(true);
		}
	}

	void CreateKeymap(bool firstTime)
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
			for(int i = 0; i < System.Enum.GetValues(typeof(InputTypes)).Length; i++)
			{
				for(int j = 0; j < System.Enum.GetValues(typeof(PlayerInputs)).Length; j++)
				{
					string toWrite = "";

					// Set input type, controller or keyboard
					switch(i)
					{
						case (int)InputTypes.Controller:
							toWrite += "c:";
							break;
						case (int)InputTypes.Keyboard:
							toWrite += "k:";
							break;
						default:
							break;
					}

					// Set the type of player input
					switch(j)
					{
						case (int)PlayerInputs.Blues:
							toWrite += "blues:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "x";
								}
								else
								{
									toWrite += InputManager.cBlues;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "larrow";
								}
								else
								{
									toWrite += InputManager.kBlues;
								}
							}

							break;
						case (int)PlayerInputs.Choral:
							toWrite += "choral:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "y";
								}
								else
								{
									toWrite += InputManager.cChoral;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "uarrow";
								}
								else
								{
									toWrite += InputManager.kChoral;
								}
							}

							break;
						case (int)PlayerInputs.HeavyMetal:
							toWrite += "metal:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "b";
								}
								else
								{
									toWrite += InputManager.cMetal;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "rarrow";
								}
								else
								{
									toWrite += InputManager.kMetal;
								}
							}

							break;
						case (int)PlayerInputs.Jump:
							toWrite += "jump:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "a";
								}
								else
								{
									toWrite += InputManager.cJump;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "w";
								}
								else
								{
									toWrite += InputManager.kJump;
								}
							}

							break;
						case (int)PlayerInputs.Play:
							toWrite += "play:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "rt";
								}
								else
								{
									toWrite += InputManager.cPlay;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "space";
								}
								else
								{
									toWrite += InputManager.kPlay;
								}
							}

							break;
						case (int)PlayerInputs.Stop:
							toWrite += "stop:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "lt";
								}
								else
								{
									toWrite += InputManager.cStop;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "darrow";
								}
								else
								{
									toWrite += InputManager.kStop;
								}
							}

							break;
						case (int)PlayerInputs.MoveLeft:
							toWrite += "left:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "lsx";
								}
								else
								{
									toWrite += InputManager.cLeft;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "a";
								}
								else
								{
									toWrite += InputManager.kLeft;
								}
							}

							break;
						case (int)PlayerInputs.MoveRight:
							toWrite += "right:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "lsx";
								}
								else
								{
									toWrite += InputManager.cRight;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "d";
								}
								else
								{
									toWrite += InputManager.kRight;
								}
							}

							break;
						case (int)PlayerInputs.PanCameraDown:
							toWrite += "down:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "rsy";
								}
								else
								{
									toWrite += InputManager.cDown;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "q";
								}
								else
								{
									toWrite += InputManager.kDown;
								}
							}

							break;
						case (int)PlayerInputs.PanCameraUp:
							toWrite += "up:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "rsy";
								}
								else
								{
									toWrite += InputManager.cUp;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "e";
								}
								else
								{
									toWrite += InputManager.kUp;
								}
							}

							break;
						case (int)PlayerInputs.Reset:
							toWrite += "reset:";

							if(i == (int)InputTypes.Controller)
							{
								if(firstTime)
								{
									toWrite += "back";
								}
								else
								{
									toWrite += InputManager.cReset;
								}
							}
							else
							{
								if(firstTime)
								{
									toWrite += "r";
								}
								else
								{
									toWrite += InputManager.kReset;
								}
							}

							break;
						default:
							break;
					}

					sw.WriteLine(toWrite);
				}
			}
		}

		LoadKeymap(type);
	}

	void LoadKeymap(InputTypes type)
	{
		if(type == InputTypes.Keyboard && SceneManager.GetActiveScene().name == "RemapKeys")
		{
			GameObject.Find("Title").GetComponent<Text>().text = "Modify Keyboard Input";
		}
			
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
					// In this example, I split it into arguments based on colon
					string[] entries = line.Split(':');
					if(entries.Length > 0)
					{
						InputManager.SetMapping(entries[0], entries[1], entries[2]);

						if(SceneManager.GetActiveScene().name == "RemapKeys")
						{
							if(type == InputTypes.Controller && entries[0] == "c")
							{
								string t;

								switch(entries[1])
								{
									case "left":
										t = changeButtonList[0].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[0].GetComponentInParent<Text>().text = t;
										break;
									case "right":
										t = changeButtonList[1].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[1].GetComponentInParent<Text>().text = t;
										break;
									case "jump":
										t = changeButtonList[2].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[2].GetComponentInParent<Text>().text = t;
										break;
									case "metal":
										t = changeButtonList[3].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[3].GetComponentInParent<Text>().text = t;
										break;
									case "blues":
										t = changeButtonList[4].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[4].GetComponentInParent<Text>().text = t;
										break;
									case "choral":
										t = changeButtonList[5].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[5].GetComponentInParent<Text>().text = t;
										break;
									case "play":
										t = changeButtonList[6].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[6].GetComponentInParent<Text>().text = t;
										break;
									case "stop":
										t = changeButtonList[7].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[7].GetComponentInParent<Text>().text = t;
										break;
									case "reset":
										t = changeButtonList[8].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[8].GetComponentInParent<Text>().text = t;
										break;
									case "up":
										t = changeButtonList[9].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[9].GetComponentInParent<Text>().text = t;
										break;
									case "down":
										t = changeButtonList[10].GetComponentInParent<Text>().text;

										if(InputManager.stringToControllerAxis.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToControllerAxis[entries[2]];
										}
										else if(InputManager.stringToControllerButton.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + entries[2];
										}

										changeButtonList[10].GetComponentInParent<Text>().text = t;
										break;
									default:
										break;
								}
							}
							else if(type == InputTypes.Keyboard && entries[0] == "k")
							{
								string t;

								switch(entries[1])
								{
									case "left":
										t = changeButtonList[0].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[0].GetComponentInParent<Text>().text = t;
										break;
									case "right":
										t = changeButtonList[1].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[1].GetComponentInParent<Text>().text = t;
										break;
									case "jump":
										t = changeButtonList[2].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[2].GetComponentInParent<Text>().text = t;
										break;
									case "metal":
										t = changeButtonList[3].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[3].GetComponentInParent<Text>().text = t;
										break;
									case "blues":
										t = changeButtonList[4].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[4].GetComponentInParent<Text>().text = t;
										break;
									case "choral":
										t = changeButtonList[5].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[5].GetComponentInParent<Text>().text = t;
										break;
									case "play":
										t = changeButtonList[6].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[6].GetComponentInParent<Text>().text = t;
										break;
									case "stop":
										t = changeButtonList[7].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[7].GetComponentInParent<Text>().text = t;
										break;
									case "reset":
										t = changeButtonList[8].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[8].GetComponentInParent<Text>().text = t;
										break;
									case "up":
										t = changeButtonList[9].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[9].GetComponentInParent<Text>().text = t;
										break;
									case "down":
										t = changeButtonList[10].GetComponentInParent<Text>().text;

										if(InputManager.stringToKey.ContainsKey(entries[2]))
										{
											t = t.Split(':')[0] + ": " + InputManager.stringToKey[entries[2]];
										}

										changeButtonList[10].GetComponentInParent<Text>().text = t;
										break;
									default:
										break;
								}
							}
						}
					}
				}
			} while(line != null);

			// Done reading, close the reader and return true to broadcast success    
			sr.Close();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if(SceneManager.GetActiveScene().name == "RemapKeys")
		{
			if(changingInput && inputEnabled)
			{
				if(type == InputTypes.Controller)
				{
					if(changingInputIsAxis && KeyboardState.controllerAxesActive.Count != 0)
					{
						InputManager.SetMapping("c", changeButtonToInput[selectedChangeButton], InputManager.controllerAxisToString[KeyboardState.controllerAxesActive[0]]);
						CreateKeymap(false);
						changeButtonList[selectedChangeButton].GetComponentInChildren<Text>().text = "Change";
						changingInput = false;
						string t = changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text;
						t = t.Split(':')[0] + ": " + KeyboardState.controllerAxesActive[0];
						changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text = t;
						StartCoroutine("Cooldown");
					}

					if(!changingInputIsAxis && KeyboardState.controllerButtonsPressed.Count != 0)
					{
						InputManager.SetMapping("c", changeButtonToInput[selectedChangeButton], InputManager.controllerButtonToString[KeyboardState.controllerButtonsPressed[0]]);
						CreateKeymap(false);
						changeButtonList[selectedChangeButton].GetComponentInChildren<Text>().text = "Change";
						changingInput = false;
						string t = changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text;
						t = t.Split(':')[0] + ": " + InputManager.controllerButtonToString[KeyboardState.controllerButtonsPressed[0]];
						changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text = t;
						StartCoroutine("Cooldown");
					}
				}
				else
				{
					if(KeyboardState.keysPressed.Count != 0)
					{
						InputManager.SetMapping("k", changeButtonToInput[selectedChangeButton], InputManager.keyToString[KeyboardState.keysPressed[0]]);
						CreateKeymap(false);
						changeButtonList[selectedChangeButton].GetComponentInChildren<Text>().text = "Change";
						changingInput = false;
						string t = changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text;
						t = t.Split(':')[0] + ": " + KeyboardState.keysPressed[0];
						changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text = t;
						StartCoroutine("Cooldown");
					}
				}
			}

			if(!changingInput && inputEnabled && Input.GetAxis("Submit") > 0.2f)
			{
				if(selectedChangeButton == 11)
				{
					SceneManager.LoadScene("MainMenu");
				}
				else
				{
					changingInput = true;

					if(type == InputTypes.Controller)
					{
						if(selectedChangeButton == 0 || selectedChangeButton == 1 || selectedChangeButton == 6 || selectedChangeButton == 7 || selectedChangeButton == 9 || selectedChangeButton == 10)
						{
							changingInputIsAxis = true;
						}
						else
						{
							changingInputIsAxis = false;
						}
					}

					changeButtonList[selectedChangeButton].GetComponentInChildren<Text>().text = "Press Any Key To Set";
					string t = changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text;
					t = t.Split(':')[0] + ": ?";
					changeButtonList[selectedChangeButton].GetComponentInParent<Text>().text = t;
					StartCoroutine("Cooldown");
				}
			}

			if(!changingInput && inputEnabled)
			{
				if(selectedChangeButton > 0 && Input.GetAxis("Vertical") > 0.2f)
				{
					selectedChangeButton--;
					StartCoroutine("Cooldown");
				}
				else if(selectedChangeButton < (changeButtonList.Count - 1) && Input.GetAxis("Vertical") < -0.2f)
				{
					selectedChangeButton++;
					StartCoroutine("Cooldown");
				}
			}

			changeButtonList[selectedChangeButton].Select();
		}
	}

	IEnumerator Cooldown()
	{
		inputEnabled = false;
		yield return new WaitForSeconds(0.2f);
		inputEnabled = true;
	}
}