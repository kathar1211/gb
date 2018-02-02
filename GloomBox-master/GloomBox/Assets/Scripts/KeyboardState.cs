using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardState : MonoBehaviour
{
	#region Keyboard Lists
	public static List<KeyCode> keysPressed;
	List<KeyCode> prevKeysPressed;
	public static List<KeyCode> keysJustPressed;
	static List<KeyCode> keysJustReleased;
	static List<KeyCode> keysToCheck;
	#endregion

	#region Mouse Button Lists
	public static List<MouseButtons> mouseButtonsPressed;
	List<MouseButtons> prevMouseButtonsPressed;
	public static List<MouseButtons> mouseButtonsJustPressed;
	static List<MouseButtons> mouseButtonsJustReleased;
	static List<MouseButtons> mouseButtonsToCheck;
	#endregion

	#region Controller Button Lists
	public static List<KeyCode> controllerButtonsPressed;
	List<KeyCode> prevControllerButtonsPressed;
	public static List<KeyCode> controllerButtonsJustPressed;
	static List<KeyCode> controllerButtonsJustReleased;
	static List<KeyCode> controllerButtonsToCheck;
	#endregion

	#region Controller Axis Lists
	public static List<string> controllerAxesActive;
	List<string> prevControllerAxesActive;
	public static List<string> controllerAxesJustActivated;
	static List<string> controllerAxesJustReleased;
	static List<string> controllerAxesToCheck;
	#endregion

	// Use this for initialization
	void Start()
	{
		keysPressed = new List<KeyCode>();
		prevKeysPressed = new List<KeyCode>();
		keysJustPressed = new List<KeyCode>();
		keysJustReleased = new List<KeyCode>();
		keysToCheck = new List<KeyCode>() {KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K,
					  KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
					  KeyCode.Y, KeyCode.Z, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Escape, KeyCode.Tab,
					  KeyCode.CapsLock, KeyCode.Return, KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftAlt,
					  KeyCode.RightAlt, KeyCode.Backslash, KeyCode.Slash, KeyCode.LeftBracket, KeyCode.RightBracket, KeyCode.Quote,KeyCode.Comma, KeyCode.Period,
					  KeyCode.Semicolon, KeyCode.Space, KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
					  KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9};

		mouseButtonsPressed = new List<MouseButtons>();
		prevMouseButtonsPressed = new List<MouseButtons>();
		mouseButtonsJustPressed = new List<MouseButtons>();
		mouseButtonsJustReleased = new List<MouseButtons>();
		mouseButtonsToCheck = new List<MouseButtons>() {MouseButtons.Left, MouseButtons.Middle, MouseButtons.Right};

		controllerButtonsPressed = new List<KeyCode>();
		prevControllerButtonsPressed = new List<KeyCode>();
		controllerButtonsJustPressed = new List<KeyCode>();
		controllerButtonsJustReleased = new List<KeyCode>();
		controllerButtonsToCheck = new List<KeyCode> {KeyCode.JoystickButton0, KeyCode.JoystickButton1, KeyCode.JoystickButton2, KeyCode.JoystickButton3,
								   KeyCode.JoystickButton4, KeyCode.JoystickButton5, KeyCode.JoystickButton6, KeyCode.JoystickButton7, KeyCode.JoystickButton8,
								   KeyCode.JoystickButton9};

		controllerAxesActive = new List<string>();
		prevControllerAxesActive = new List<string>();
		controllerAxesJustActivated = new List<string>();
		controllerAxesJustReleased = new List<string>();
		controllerAxesToCheck = new List<string> {"LSXAxis", "LSYAxis", "RSXAxis", "RSYAxis", "DPadXAxis", "DPadYAxis", "LeftTrigger", "RightTrigger"};
	}
	
	// Update is called once per frame
	void Update()
	{
		//print(controllerButtonsJustPressed[0]);

		// Clear currently active lists
		keysPressed.Clear();
		keysJustPressed.Clear();
		keysJustReleased.Clear();

		mouseButtonsPressed.Clear();
		mouseButtonsJustPressed.Clear();
		mouseButtonsJustReleased.Clear();

		controllerButtonsPressed.Clear();
		controllerButtonsJustPressed.Clear();
		controllerButtonsJustReleased.Clear();

		controllerAxesActive.Clear();
		controllerAxesJustActivated.Clear();
		controllerAxesJustReleased.Clear();

		//Check keys currently down
		foreach(KeyCode key in keysToCheck)
		{
			if(GetKeyDown(key))
			{
				keysPressed.Add(key);

				if(!prevKeysPressed.Contains(key))
				{
					keysJustPressed.Add(key);
				}
			}
		}

		foreach(KeyCode key in prevKeysPressed)
		{
			if(!keysPressed.Contains(key))
			{
				keysJustReleased.Add(key);
			}
		}

		// Check mouse buttons currently down
		foreach(MouseButtons button in mouseButtonsToCheck)
		{
			if(GetMouseButtonDown(button))
			{
				mouseButtonsPressed.Add(button);

				if(!prevMouseButtonsPressed.Contains(button))
				{
					mouseButtonsJustPressed.Add(button);
				}
			}
		}

		foreach(MouseButtons button in prevMouseButtonsPressed)
		{
			if(!mouseButtonsPressed.Contains(button))
			{
				mouseButtonsJustReleased.Add(button);
			}
		}

		// Check buttons currently down
		foreach(KeyCode button in controllerButtonsToCheck)
		{
			if(GetButtonDown(button))
			{
				controllerButtonsPressed.Add(button);

				if(!prevControllerButtonsPressed.Contains(button))
				{
					controllerButtonsJustPressed.Add(button);
				}
			}
		}

		foreach(KeyCode button in prevControllerButtonsPressed)
		{
			if(!controllerButtonsPressed.Contains(button))
			{
				controllerButtonsJustReleased.Add(button);
			}
		}

		// Check axes currently active
		foreach(string axis in controllerAxesToCheck)
		{
			if(GetAxisActive(axis, 0.3f))
			{
				controllerAxesActive.Add(axis);

				if(!prevControllerAxesActive.Contains(axis))
				{
					controllerAxesJustActivated.Add(axis);
				}
			}
		}

		foreach(string axis in prevControllerAxesActive)
		{
			if(!controllerAxesActive.Contains(axis))
			{
				controllerAxesJustReleased.Add(axis);
			}
		}

		// Set previous lists to the current state
		prevKeysPressed = new List<KeyCode>(keysPressed);
		prevMouseButtonsPressed = new List<MouseButtons>(mouseButtonsPressed);
		prevControllerButtonsPressed = new List<KeyCode>(controllerButtonsPressed);
		prevControllerAxesActive = new List<string>(controllerAxesActive);
	}

	#region Private Helper Methods
	/// <summary>
	/// Checks Unity's input handler to see whether a specific key is down
	/// </summary>
	/// <returns><c>true</c>, if the key is pressed, <c>false</c> otherwise.</returns>
	/// <param name="key">The KeyCode to check</param>
	bool GetKeyDown(KeyCode key)
	{
		if(Input.GetKey(key))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Checks Unity's input handler to see whether a specific mouse button is down
	/// </summary>
	/// <returns><c>true</c>, if the mouse button is pressed, <c>false</c> otherwise.</returns>
	/// <param name="key">The MouseButton to check</param>
	bool GetMouseButtonDown(MouseButtons button)
	{
		// Translate from MouseButtons to integers, the way Unity likes to handle mouse input
		int buttonNumber = (button == MouseButtons.Left) ? 0 : (button == MouseButtons.Right) ? 1 : 2;

		if(Input.GetMouseButton(buttonNumber))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Checks Unity's input handler to see whether a specific button is down
	/// </summary>
	/// <returns><c>true</c>, if button is pressed, <c>false</c> otherwise.</returns>
	/// <param name="button">The name of the button to check</param>
	bool GetButtonDown(KeyCode button)
	{
		if(Input.GetKey(button))
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Checks Unity's input handler to see whether a specific axis is active
	/// </summary>
	/// <returns><c>true</c>, if axis is active, <c>false</c> otherwise.</returns>
	/// <param name="axis">The name of the axis to check</param>
	/// <param name="deadZone">The maximum value that the axis shouldn't be active</param>
	bool GetAxisActive(string axis, float deadZone)
	{
		if(Mathf.Abs(Input.GetAxis(axis)) > deadZone)
		{
			return true;
		}

		return false;
	}
	#endregion
	
	#region Keyboard Accessor Methods
	/// <summary>
	/// Returns whether a specific key is pressed
	/// </summary>
	/// <returns><c>true</c> if the key specified is pressed; otherwise, <c>false</c>.</returns>
	/// <param name="key">The KeyCode to check</param>
	public static bool IsKeyPressed(KeyCode key)
	{
		if(keysPressed.Contains(key))
		{
			return true;
		}

		if(!keysToCheck.Contains(key))
		{
			Debug.LogWarning("A key you're checking, " + key + ", isn't on the list of keysToCheck. Please add it if you'd like it to be input.");
		}

		return false;
	}

	/// <summary>
	/// Returns whether a key has just been pressed this frame
	/// </summary>
	/// <returns><c>true</c> if the key has been pressed for the first time this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The KeyCode to check</param>
	public static bool IsKeyJustPressed(KeyCode key)
	{
		if(keysJustPressed.Contains(key))
		{
			return true;
		}

		if(!keysToCheck.Contains(key))
		{
			Debug.LogWarning("A key you're checking, " + key + ", isn't on the list of keysToCheck. Please add it if you'd like it to be input.");
		}

		return false;
	}

	/// <summary>
	/// Returns whether a key has just been released this frame
	/// </summary>
	/// <returns><c>true</c> if the key has been released this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The KeyCode to check</param>
	public static bool IsKeyJustReleased(KeyCode key)
	{
		if(keysJustReleased.Contains(key))
		{
			return true;
		}

		if(!keysToCheck.Contains(key))
		{
			Debug.LogWarning("A key you're checking, " + key + ", isn't on the list of keysToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	#endregion

	#region Mouse Button Accessor Methods
	/// <summary>
	/// Returns whether a specific mouse button is pressed
	/// </summary>
	/// <returns><c>true</c> if the mouse button specified is pressed; otherwise, <c>false</c>.</returns>
	/// <param name="key">The mouse button to check</param>
	public static bool IsMouseButtonPressed(MouseButtons button)
	{
		if(mouseButtonsPressed.Contains(button))
		{
			return true;
		}
		
		if(!mouseButtonsToCheck.Contains(button))
		{
			Debug.LogWarning("A mouse button you're checking, " + button + ", isn't on the list of mouseButtonsToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	
	// <summary>
	/// Returns whether a mouse button has just been pressed this frame
	/// </summary>
	/// <returns><c>true</c> if the mouse button has been pressed for the first time this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The mouse button to check</param>
	public static bool IsMouseButtonJustPressed(MouseButtons button)
	{
		if(mouseButtonsJustPressed.Contains(button))
		{
			return true;
		}
		
		if(!mouseButtonsToCheck.Contains(button))
		{
			Debug.LogWarning("A mouse button you're checking, " + button + ", isn't on the list of mouseButtonsToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	
	/// <summary>
	/// Returns whether a mouse button has just been released this frame
	/// </summary>
	/// <returns><c>true</c> if the mouse button has been released this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The mouse button to check</param>
	public static bool IsMouseButtonJustReleased(MouseButtons button)
	{
		if(mouseButtonsJustReleased.Contains(button))
		{
			return true;
		}
		
		if(!mouseButtonsToCheck.Contains(button))
		{
			Debug.LogWarning("A mouse button you're checking, " + button + ", isn't on the list of mouseButtonsToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	#endregion

	#region Controller Button Accessor Methods
	/// <summary>
	/// Returns whether a specific button is pressed
	/// </summary>
	/// <returns><c>true</c> if the button specified is pressed; otherwise, <c>false</c>.</returns>
	/// <param name="key">The name of the button to check</param>
	public static bool IsButtonPressed(KeyCode button)
	{
		if(controllerButtonsPressed.Contains(button))
		{
			return true;
		}

		if(!controllerButtonsToCheck.Contains(button))
		{
			Debug.LogWarning("A button you're checking, " + button + ", isn't on the list of buttonsToCheck. Please add it if you'd like it to be input.");
		}

		return false;
	}

	// <summary>
	/// Returns whether a button has just been pressed this frame
	/// </summary>
	/// <returns><c>true</c> if the button has been pressed for the first time this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The name of the button to check</param>
	public static bool IsButtonJustPressed(KeyCode button)
	{
		if(controllerButtonsJustPressed.Contains(button))
		{
			return true;
		}

		if(!controllerButtonsToCheck.Contains(button))
		{
			Debug.LogWarning("A button you're checking, " + button + ", isn't on the list of buttonsToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}

	/// <summary>
	/// Returns whether a button has just been released this frame
	/// </summary>
	/// <returns><c>true</c> if the button has been released this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The name of the button to check</param>
	public static bool IsButtonJustReleased(KeyCode button)
	{
		if(controllerButtonsJustReleased.Contains(button))
		{
			return true;
		}

		if(!controllerButtonsToCheck.Contains(button))
		{
			Debug.LogWarning("A button you're checking, " + button + ", isn't on the list of controllerButtonsToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	#endregion

	#region Controller Axis Accessor Methods
	/// <summary>
	/// Returns whether a specific axis is active
	/// </summary>
	/// <returns><c>true</c> if the axis specified is active; otherwise, <c>false</c>.</returns>
	/// <param name="key">The name of the axis to check</param>
	public static bool IsAxisActive(string axis)
	{
		if(controllerAxesActive.Contains(axis))
		{
			return true;
		}

		if(!controllerAxesToCheck.Contains(axis))
		{
			Debug.LogWarning("An axis you're checking, " + axis + ", isn't on the list of controllerAxesToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}

	/// <summary>
	/// Returns the value of a specified axis
	/// </summary>
	/// <returns>The axis value</returns>
	/// <param name="axis">The axis to check</param>
	public static float GetAxisValue(string axis)
	{
		if(controllerAxesActive.Contains(axis))
		{
			return Input.GetAxis(axis);
		}

		if(!controllerAxesToCheck.Contains(axis))
		{
			Debug.LogWarning("An axis you're checking, " + axis + ", isn't on the list of controllerAxesToCheck. Please add it if you'd like it to be input.");
		}

		return 0f;
	}
	
	// <summary>
	/// Returns whether an axis has just been activated this frame
	/// </summary>
	/// <returns><c>true</c> if the axis has been activated for the first time this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The name of the axis to check</param>
	public static bool IsAxisJustActivated(string axis)
	{
		if(controllerAxesJustActivated.Contains(axis))
		{
			return true;
		}

		if(!controllerAxesToCheck.Contains(axis))
		{
			Debug.LogWarning("An axis you're checking, " + axis + ", isn't on the list of controllerAxesToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	
	/// <summary>
	/// Returns whether an axis has just been released this frame
	/// </summary>
	/// <returns><c>true</c> if the axis has been released this frame; otherwise, <c>false</c>.</returns>
	/// <param name="key">The name of the axis to check</param>
	public static bool IsAxisJustReleased(string axis)
	{
		if(controllerAxesJustReleased.Contains(axis))
		{
			return true;
		}

		if(!controllerAxesToCheck.Contains(axis))
		{
			Debug.LogWarning("An axis you're checking, " + axis + ", isn't on the list of controllerAxesToCheck. Please add it if you'd like it to be input.");
		}
		
		return false;
	}
	#endregion
}