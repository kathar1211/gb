using UnityEngine;
using System.Collections;

public class SignpostScript : MonoBehaviour
{
    SpriteRenderer SignImage;
    GameObject Muse;

    Vector3 targetPosition;
    Vector3 targetScale;

	// Use this for initialization
	void Start()
    {
        // Find child object stored in parent transform
        SignImage = this.transform.Find("SignImage").GetComponent<SpriteRenderer>();
        //getting default signImage stuff
        targetPosition = SignImage.gameObject.transform.localPosition;
        targetScale = SignImage.gameObject.transform.localScale;

        // Setting default transparency to maximum
        SignImage.color = new Color(1f, 1f, 1f, 0f);
        SignImage.gameObject.transform.position = new Vector3(0f, 0f, 0f);
        SignImage.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        Muse = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update()
    {
        float transparency = 1.75f - Vector3.Magnitude(this.transform.position - Muse.transform.position) * 0.4f;
        SignImage.color = new Color(1f, 1f, 1f, transparency);
        float lerpFactor = 1.2f - 0.1f * Vector3.Magnitude(this.transform.position - Muse.transform.position);
        if(lerpFactor > 1f)
        {
            lerpFactor = 1f;
        }
        SignImage.gameObject.transform.localPosition = Vector3.Lerp(Vector3.zero, targetPosition, lerpFactor);
        SignImage.gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, lerpFactor);
    }
}