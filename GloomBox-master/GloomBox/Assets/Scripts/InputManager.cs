using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
	public static string cBlues;
	public static string cChoral;
	public static string cMetal;
	public static string cJump;
	public static string cPlay;
	public static string cStop;
	public static string cLeft;
	public static string cRight;
	public static string cDown;
	public static string cUp;
	public static string cReset;

	public static string kBlues;
	public static string kChoral;
	public static string kMetal;
	public static string kJump;
	public static string kPlay;
	public static string kStop;
	public static string kLeft;
	public static string kRight;
	public static string kDown;
	public static string kUp;
	public static string kReset;

	public static Dictionary<string, string> controllerAxisToString;
	public static Dictionary<KeyCode, string> controllerButtonToString;
	public static Dictionary<KeyCode, string> keyToString;
	public static Dictionary<string, string> stringToControllerAxis;
	public static Dictionary <string, KeyCode> stringToControllerButton;
	public static Dictionary<string, InputTypes> stringToInputType;
	public static Dictionary<string, KeyCode> stringToKey;
	public static Dictionary<string, PlayerInputs> stringToPlayerInput;

	// Use this for initialization
	void Awake()
	{
		stringToControllerAxis = new Dictionary<string, string>();
		stringToControllerAxis.Add("lsx", "LSXAxis");
		stringToControllerAxis.Add("lsy", "LSYAxis");
		stringToControllerAxis.Add("rsx", "RSXAxis");
		stringToControllerAxis.Add("rsy", "RSYAxis");
		stringToControllerAxis.Add("dpadx", "DPadXAxis");
		stringToControllerAxis.Add("dpady", "DPadYAxis");
		stringToControllerAxis.Add("lt", "LeftTrigger");
		stringToControllerAxis.Add("rt", "RightTrigger");

		controllerAxisToString = new Dictionary<string, string>();

		foreach(string s in stringToControllerAxis.Keys)
		{
			controllerAxisToString.Add(stringToControllerAxis[s], s);
		}

		stringToControllerButton = new Dictionary<string, KeyCode>();
		stringToControllerButton.Add("a", KeyCode.JoystickButton0);
		stringToControllerButton.Add("b", KeyCode.JoystickButton1);
		stringToControllerButton.Add("x", KeyCode.JoystickButton2);
		stringToControllerButton.Add("y", KeyCode.JoystickButton3);
		stringToControllerButton.Add("lb", KeyCode.JoystickButton4);
		stringToControllerButton.Add("rb", KeyCode.JoystickButton5);
		stringToControllerButton.Add("back", KeyCode.JoystickButton6);
		stringToControllerButton.Add("start", KeyCode.JoystickButton7);
		stringToControllerButton.Add("l3", KeyCode.JoystickButton8);
		stringToControllerButton.Add("r3", KeyCode.JoystickButton9);

		controllerButtonToString = new Dictionary<KeyCode, string>();

		foreach(string s in stringToControllerButton.Keys)
		{
			controllerButtonToString.Add(stringToControllerButton[s], s);
		}

		stringToInputType = new Dictionary<string, InputTypes>();
		stringToInputType.Add("c", InputTypes.Controller);
		stringToInputType.Add("k", InputTypes.Keyboard);

		stringToKey = new Dictionary<string, KeyCode>();
		stringToKey.Add("a", KeyCode.A);
		stringToKey.Add("b", KeyCode.B);
		stringToKey.Add("c", KeyCode.C);
		stringToKey.Add("d", KeyCode.D);
		stringToKey.Add("e", KeyCode.E);
		stringToKey.Add("f", KeyCode.F);
		stringToKey.Add("g", KeyCode.G);
		stringToKey.Add("h", KeyCode.H);
		stringToKey.Add("i", KeyCode.I);
		stringToKey.Add("j", KeyCode.J);
		stringToKey.Add("k", KeyCode.K);
		stringToKey.Add("l", KeyCode.L);
		stringToKey.Add("m", KeyCode.M);
		stringToKey.Add("n", KeyCode.N);
		stringToKey.Add("o", KeyCode.O);
		stringToKey.Add("p", KeyCode.P);
		stringToKey.Add("q", KeyCode.Q);
		stringToKey.Add("r", KeyCode.R);
		stringToKey.Add("s", KeyCode.S);
		stringToKey.Add("t", KeyCode.T);
		stringToKey.Add("u", KeyCode.U);
		stringToKey.Add("v", KeyCode.V);
		stringToKey.Add("w", KeyCode.W);
		stringToKey.Add("x", KeyCode.X);
		stringToKey.Add("y", KeyCode.Y);
		stringToKey.Add("z", KeyCode.Z);
		stringToKey.Add("0", KeyCode.Alpha0);
		stringToKey.Add("1", KeyCode.Alpha1);
		stringToKey.Add("2", KeyCode.Alpha2);
		stringToKey.Add("3", KeyCode.Alpha3);
		stringToKey.Add("4", KeyCode.Alpha4);
		stringToKey.Add("5", KeyCode.Alpha5);
		stringToKey.Add("6", KeyCode.Alpha6);
		stringToKey.Add("7", KeyCode.Alpha7);
		stringToKey.Add("8", KeyCode.Alpha8);
		stringToKey.Add("9", KeyCode.Alpha9);
		stringToKey.Add("tab", KeyCode.Tab);
		stringToKey.Add("capslock", KeyCode.CapsLock);
		stringToKey.Add("lshift", KeyCode.LeftShift);
		stringToKey.Add("lctrl", KeyCode.LeftControl);
		stringToKey.Add("lalt", KeyCode.LeftAlt);
		stringToKey.Add("space", KeyCode.Space);
		stringToKey.Add("ralt", KeyCode.RightAlt);
		stringToKey.Add("rshift", KeyCode.RightShift);
		stringToKey.Add("rctrl", KeyCode.RightControl);
		stringToKey.Add("uarrow", KeyCode.UpArrow);
		stringToKey.Add("larrow", KeyCode.LeftArrow);
		stringToKey.Add("darrow", KeyCode.DownArrow);
		stringToKey.Add("rarrow", KeyCode.RightArrow);
		stringToKey.Add("comma", KeyCode.Comma);
		stringToKey.Add("period", KeyCode.Period);
		stringToKey.Add("fslash", KeyCode.Slash);
		stringToKey.Add("semicolon", KeyCode.Semicolon);
		stringToKey.Add("quote", KeyCode.Quote);
		stringToKey.Add("enter", KeyCode.Return);
		stringToKey.Add("lbracket", KeyCode.LeftBracket);
		stringToKey.Add("rbracket", KeyCode.RightBracket);
		stringToKey.Add("bquote", KeyCode.BackQuote);
		stringToKey.Add("hyphen", KeyCode.Minus);
		stringToKey.Add("equals", KeyCode.Equals);
		stringToKey.Add("backspace", KeyCode.Backspace);
		stringToKey.Add("esc", KeyCode.Escape);
		stringToKey.Add("f1", KeyCode.F1);
		stringToKey.Add("f2", KeyCode.F2);
		stringToKey.Add("f3", KeyCode.F3);
		stringToKey.Add("f4", KeyCode.F4);
		stringToKey.Add("f5", KeyCode.F5);
		stringToKey.Add("f6", KeyCode.F6);
		stringToKey.Add("f7", KeyCode.F7);
		stringToKey.Add("f8", KeyCode.F8);
		stringToKey.Add("f9", KeyCode.F9);
		stringToKey.Add("f10", KeyCode.F10);
		stringToKey.Add("f11", KeyCode.F11);
		stringToKey.Add("f12", KeyCode.F12);
		stringToKey.Add("prtscr", KeyCode.Print);
		stringToKey.Add("insert", KeyCode.Insert);
		stringToKey.Add("delete", KeyCode.Delete);

		keyToString = new Dictionary<KeyCode, string>();

		foreach(string s in stringToKey.Keys)
		{
			keyToString.Add(stringToKey[s], s);
		}

		stringToPlayerInput = new Dictionary<string, PlayerInputs>();
		stringToPlayerInput.Add("blues", PlayerInputs.Blues);
		stringToPlayerInput.Add("choral", PlayerInputs.Choral);
		stringToPlayerInput.Add("metal", PlayerInputs.HeavyMetal);
		stringToPlayerInput.Add("jump", PlayerInputs.Jump);
		stringToPlayerInput.Add("play", PlayerInputs.Play);
		stringToPlayerInput.Add("stop", PlayerInputs.Stop);
		stringToPlayerInput.Add("left", PlayerInputs.MoveLeft);
		stringToPlayerInput.Add("right", PlayerInputs.MoveRight);
		stringToPlayerInput.Add("down", PlayerInputs.PanCameraDown);
		stringToPlayerInput.Add("up", PlayerInputs.PanCameraUp);
		stringToPlayerInput.Add("reset", PlayerInputs.Reset);
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	/// <summary>
	/// Sets mapping for a particular key
	/// </summary>
	/// <param name="inputType">The input type, controller or keyboard</param>
	/// <param name="input">The input that is being set</param>
	/// <param name="newKey">The new key that will be mapped to that input</param>
	public static void SetMapping(string inputType, string input, string newKey)
	{
		InputTypes type = stringToInputType[inputType];
		PlayerInputs playerInput = stringToPlayerInput[input];

		switch(playerInput)
		{
			case PlayerInputs.Blues:
				if(type == InputTypes.Controller)
				{
					cBlues = newKey;
				}
				else
				{
					kBlues = newKey;
				}				
				break;
			case PlayerInputs.Choral:
				if(type == InputTypes.Controller)
				{
					cChoral = newKey;
				}
				else
				{
					kChoral = newKey;
				}
				break;
			case PlayerInputs.HeavyMetal:
				if(type == InputTypes.Controller)
				{
					cMetal = newKey;
				}
				else
				{
					kMetal = newKey;
				}
				break;
			case PlayerInputs.Jump:
				if(type == InputTypes.Controller)
				{
					cJump = newKey;
				}
				else
				{
					kJump = newKey;
				}
				break;
			case PlayerInputs.Play:
				if(type == InputTypes.Controller)
				{
					cPlay = newKey;
				}
				else
				{
					kPlay = newKey;
				}
				break;
			case PlayerInputs.Stop:
				if(type == InputTypes.Controller)
				{
					cStop = newKey;
				}
				else
				{
					kStop = newKey;
				}
				break;
			case PlayerInputs.MoveLeft:
				if(type == InputTypes.Controller)
				{
					cLeft = newKey;
				}
				else
				{
					kLeft = newKey;
				}
				break;
			case PlayerInputs.MoveRight:
				if(type == InputTypes.Controller)
				{
					cRight = newKey;
				}
				else
				{
					kRight = newKey;
				}
				break;
			case PlayerInputs.PanCameraDown:
				if(type == InputTypes.Controller)
				{
					cDown = newKey;
				}
				else
				{
					kDown = newKey;
				}
				break;
			case PlayerInputs.PanCameraUp:
				if(type == InputTypes.Controller)
				{
					cUp = newKey;
				}
				else
				{
					kUp = newKey;
				}
				break;
			case PlayerInputs.Reset:
				if(type == InputTypes.Controller)
				{
					cReset = newKey;
				}
				else
				{
					kReset = newKey;
				}
				break;
			default:
				break;
		}
	}
}