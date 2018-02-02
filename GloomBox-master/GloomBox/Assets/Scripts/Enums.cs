// A file that holds all global enums

// Enum containing all types of music
public enum TypesOfMusic
{
	None,
	Blues,
	Choral,
	HeavyMetal,
	BluesChoral,
	BluesHeavyMetal,
	ChoralHeavyMetal
};

// Enum containing states that water can be in
public enum StatesOfWater
{
    Liquid,
    Gas,
    Solid
};

// Enum containing types of input
public enum InputTypes
{
	Controller,
	Keyboard
};

// Enum containing all player inputs
public enum PlayerInputs
{
	Blues,
	Choral,
	HeavyMetal,
	Jump,
	Play,
	Stop,
	MoveLeft,
	MoveRight,
	PanCameraDown,
	PanCameraUp,
	Reset
};

// Enum containing all mouse buttons
public enum MouseButtons
{
	Left,
	Middle,
	Right
};

// Enum containing all possible directions that Muse could be moving to cause camera movement
public enum CameraDirections
{
	DownToUp,
	LeftToRight,
	RightToLeft,
	UpToDown
}