#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region Parallax Class
public class Parallax : MonoBehaviour
{
	#region Public Variables
    /* https://www.youtube.com/watch?v=QkisHNmcK7Y */
    public float backgroundSize;
    public float parallaxSpeed;
    public bool scrolling, parallax;
	#endregion

	#region Private Variables
    Transform cameraTransform;
    Transform[] layers;
    float viewZone = 10;
    int leftIndex;
    int rightIndex;
    float lastCameraX;
	#endregion

	// Use this for initialization
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);

            leftIndex = 0;
            rightIndex = layers.Length - 1;
        }
    }

	// Update is called once per frame
	void Update()
    {
		// If parallaxing, actually do the parallax
        if(parallax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }
        
		lastCameraX = cameraTransform.position.x;

		// If scrolling, check to see if the player's position warrants the movement of a background section
        if(scrolling)
        {
            if(cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            {
                ScrollLeft();
            }
            if(cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
            {
                ScrollRight();
            }
        }
    }

	#region Scroll Functions
	/// <summary>
	/// Scroll the parallax left
	/// </summary>
	private void ScrollLeft()
	{
		//  int lastRight = rightIndex;
		layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
		leftIndex = rightIndex;
		rightIndex--;

		if(rightIndex < 0)
		{
			rightIndex = layers.Length - 1;
		}
	}

	/// <summary>
	/// Scroll the parallax right
	/// </summary>
	private void ScrollRight()
	{
		// int lastLeft = leftIndex;
		layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x - backgroundSize);
		rightIndex = leftIndex;
		leftIndex++;

		if(leftIndex == layers.Length)
		{
			leftIndex = 0;
		}
	}
	#endregion
}
#endregion