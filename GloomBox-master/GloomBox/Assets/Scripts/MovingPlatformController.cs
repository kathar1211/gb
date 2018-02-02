#region Using Directives
using UnityEngine;
using System.Collections;
#endregion

#region MovingPlatformController Class
public class MovingPlatformController : MonoBehaviour
{
	#region Public Variables
    public Vector2[] wayPoints;
    public float movingSpeed;
	#endregion

	#region Private Variables
    int currentPoint;
    int pointNum;
	#endregion
    
    // Use this for initialization
	void Start()
    {
        currentPoint = 0;
        pointNum = wayPoints.Length;
	}
	
	// Update is called once per frame
	void Update()
    {
		// Move toward next waypoint
	    if(wayPoints.Length != 0)
        {
            int nextPoint = (currentPoint + 1) % pointNum;
            Vector3 desPoint = new Vector3(wayPoints[nextPoint].x, wayPoints[nextPoint].y, 0);

            if(Vector3.Distance(desPoint, transform.position) > 0.5)
            {
                Vector2 vel = (desPoint - transform.position).normalized;
                GetComponent<Rigidbody2D>().velocity = movingSpeed * vel;
            }
            else
            {
                currentPoint++;
                currentPoint %= wayPoints.Length;
            }
        }
	}
}
#endregion