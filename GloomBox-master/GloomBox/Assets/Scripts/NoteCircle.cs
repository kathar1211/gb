using UnityEngine;
using System.Collections;

public class NoteCircle : MonoBehaviour {

    float radius = 0f;
    float angleOffset = 0f;
    [SerializeField]
    float maxRadius = 0f;
    int maxChildren = 0;
    [SerializeField]
    float duration = 0f;
    float timecounter = 0f;

	// Use this for initialization
	void Start ()
    {
        foreach(Transform child in transform)
        {
            if(child != transform)
            {
                maxChildren++;
            }
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        timecounter += Time.deltaTime;
        if (true)
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                if (child != transform)
                {
                    i++;
                    //child is your child transform
                    float angle = angleOffset + (i * Mathf.PI * 2f / maxChildren);
                    Vector3 newPos = this.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                    child.position = newPos;
                    if (radius < maxRadius)
                    {
                        radius += 0.005f;
                    }
                    angleOffset += 0.002f;
                    if((timecounter)>(duration*i/maxChildren))
                    {
                        //child.gameObject.SetActive(false);
                        child.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                    }
                }
            }
        }
        if(timecounter - duration > 0f)
        {
            foreach(Transform child in transform)
            {
                if(child != transform)
                {
                    radius -= 0.02f;
                    if (radius < 0)
                    {
                       child.gameObject.SetActive(false);
                    }
                }
            }
        }       	
	}
}
