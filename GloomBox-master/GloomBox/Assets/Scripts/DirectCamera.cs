using UnityEngine;
using System.Collections;

public class DirectCamera : MonoBehaviour
{
	/*[SerializeField]
	float beginX;
	[SerializeField]
	float beginY;
	[SerializeField]
	float beginZoom;
	[SerializeField]
	CameraDirections direction;*/
	[SerializeField]
	float endX;
	[SerializeField]
	float endY;
	[SerializeField]
	float endZoom;
	[SerializeField]
	bool freezeWorldX;
	[SerializeField]
	float freezeWorldXThreshold;
	[SerializeField]
	bool freezeWorldY;
	[SerializeField]
	float freezeWorldYThreshold;
	[SerializeField]
	float time;

	bool activated;
	Bounds b;
	//float bottomY;
	Camera c;
	//float leftX;
	GameObject muse;
	//Vector3[] origScales;
	//float rightX;
	//SpriteRenderer[] scalables;
	//float topY;

	// Use this for initialization
	void Start()
	{
		b = GetComponent<MeshCollider>().bounds;
		muse = GameObject.FindGameObjectWithTag("Player");
		c = muse.GetComponentInChildren<Camera>();

		/*bottomY = b.min.y;
		leftX = b.min.x;
		rightX = b.max.x;
		topY = b.max.y;

		scalables = c.GetComponentsInChildren<SpriteRenderer>();
		origScales = new Vector3[scalables.Length];

		for(int i = 0; i < scalables.Length; i++)
		{
			origScales[i] = scalables[i].gameObject.transform.localScale;
		}*/
	}
	
	// Update is called once per frame
	void Update()
	{
		if(b.Contains(muse.transform.position))
		{
			activated = true;

			if(freezeWorldY && c.transform.position.y <= freezeWorldYThreshold)
			{
				muse.GetComponent<PlayerController>().FrozenCamY = true;
				c.GetComponent<AutoCamera>().StopAllCoroutines();
				//print(c.transform.position.y);
			}

			if(freezeWorldX && c.transform.position.x >= freezeWorldXThreshold)
			{
				muse.GetComponent<PlayerController>().FrozenCamX = true;
				c.GetComponent<AutoCamera>().StopAllCoroutines();
				//print(c.transform.position.y);
			}

			c.GetComponent<AutoCamera>().TriggerCameraMotion(endX, endY, endZoom, time, name);

			/*float frozenY = 0f;

			if(freezeWorldY)
			{
				frozenY = c.transform.position.y;
			}

			switch(direction)
			{
				case CameraDirections.DownToUp:
					float nextX = Mathf.SmoothStep(beginX, endX, (muse.transform.position.y - bottomY) / (topY - bottomY));
					float nextY = Mathf.SmoothStep(beginY, endY, (muse.transform.position.y - bottomY) / (topY - bottomY));
					c.transform.localPosition = new Vector3(nextX, nextY, c.transform.localPosition.z);
					c.orthographicSize = Mathf.SmoothStep(beginZoom, endZoom, ((muse.transform.position.y - bottomY) / (topY - bottomY)));
					break;
				case CameraDirections.LeftToRight:
					nextX = Mathf.SmoothStep(beginX, endX, (muse.transform.position.x - leftX) / (rightX - leftX));
					nextY = Mathf.SmoothStep(beginY, endY, (muse.transform.position.x - leftX) / (rightX - leftX));
					c.transform.localPosition = new Vector3(nextX, nextY, c.transform.localPosition.z);
					c.orthographicSize = Mathf.SmoothStep(beginZoom, endZoom, ((muse.transform.position.x - leftX) / (rightX - leftX)));
					break;
				case CameraDirections.RightToLeft:
					nextX = Mathf.SmoothStep(beginX, endX, (rightX - muse.transform.position.x) / (rightX - leftX));
					nextY = Mathf.SmoothStep(beginY, endY, (rightX - muse.transform.position.x) / (rightX - leftX));
					c.transform.localPosition = new Vector3(nextX, nextY, c.transform.localPosition.z);
					c.orthographicSize = Mathf.SmoothStep(beginZoom, endZoom, ((rightX - muse.transform.position.x) / (rightX - leftX)));
					break;
				case CameraDirections.UpToDown:
					nextX = Mathf.SmoothStep(beginX, endX, (topY - muse.transform.position.y) / (topY - bottomY));
					nextY = Mathf.SmoothStep(beginY, endY, (topY - muse.transform.position.y) / (topY - bottomY));
					c.transform.localPosition = new Vector3(nextX, nextY, c.transform.localPosition.z);
					c.orthographicSize = Mathf.SmoothStep(beginZoom, endZoom, ((topY - muse.transform.position.y) / (topY - bottomY)));
					break;
			}

			if(beginZoom != endZoom)
			{
				for(int i = 0; i < scalables.Length; i++)
				{
					//scalables[i].gameObject.transform.localScale = origScales[i] * (c.orthographicSize / 5.5f);
				}
			}

			if(freezeWorldY)
			{
				c.transform.position = new Vector3(c.transform.position.x, frozenY, c.transform.position.z);
			}*/
		}
		else
		{
			if(activated)
			{
				activated = false;
				c.GetComponent<AutoCamera>().ResetCameraToDefault(name);
				muse.GetComponent<PlayerController>().FrozenCamX = false;
				muse.GetComponent<PlayerController>().FrozenCamY = false;
			}
		}
	}
}