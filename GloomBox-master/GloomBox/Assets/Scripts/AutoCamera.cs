using UnityEngine;
using System.Collections;

public class AutoCamera : MonoBehaviour
{
	[SerializeField]
	float defaultTime;
	[SerializeField]
	float defaultX;
	[SerializeField]
	float defaultY;
	[SerializeField]
	float defaultZoom;

	Coroutine cameraMoveCoroutine = null;
	string lastTriggeredBy;
	GameObject muse;
	float nextX;
	float nextY;
	float nextZoom;
	float origTime;
	float prevX;
	float prevY;
	float prevZoom;

	// Use this for initialization
	void Start()
	{
		muse = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	IEnumerator SmoothCameraMovement(float toX, float toY, float toZoom, float time)
	{
		origTime = time;
		prevX = transform.localPosition.x;
		prevY = transform.localPosition.y;
		prevZoom = GetComponent<Camera>().orthographicSize;

		while(time > 0)
		{
			float currentX = prevX;
			float currentY = prevY;

			if(!muse.GetComponent<PlayerController>().FrozenCamX)
			{
				currentX = Mathf.SmoothStep(prevX, toX, (origTime - time) / origTime);
			}

			if(!muse.GetComponent<PlayerController>().FrozenCamY)
			{
				currentY = Mathf.SmoothStep(prevY, toY, (origTime - time) / origTime);
			}

			transform.localPosition = new Vector3(currentX, currentY, transform.localPosition.z);
			GetComponent<Camera>().orthographicSize = Mathf.SmoothStep(prevZoom, toZoom, (origTime - time) / origTime);

			time -= Time.deltaTime;
			yield return null;
		}
	}

	public void ResetCameraToDefault(string triggerName)
	{
		if(triggerName == lastTriggeredBy)
		{
			lastTriggeredBy = "";

			if(cameraMoveCoroutine != null)
			{
				StopCoroutine(cameraMoveCoroutine);
			}

			cameraMoveCoroutine = StartCoroutine(SmoothCameraMovement(defaultX, defaultY, defaultZoom, defaultTime));
		}
	}

	public void TriggerCameraMotion(float endX, float endY, float endZoom, float time, string triggerName)
	{
		if(lastTriggeredBy != triggerName)
		{
			if(cameraMoveCoroutine != null)
			{
				StopCoroutine(cameraMoveCoroutine);
			}

			lastTriggeredBy = triggerName;
			cameraMoveCoroutine = StartCoroutine(SmoothCameraMovement(endX, endY, endZoom, time));
		}
	}
}