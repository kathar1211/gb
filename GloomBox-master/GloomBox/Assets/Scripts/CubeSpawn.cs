using UnityEngine;
using System.Collections;

public class CubeSpawn : MonoBehaviour {

    public GameObject cubePrefab;
    [SerializeField]
    float radius = 5f;
    GameObject[] cubeArray = new GameObject[512];

	// Use this for initialization
	void Start ()
    {
        for(int i=0; i<512; i++)
        {
            float angle = (i * Mathf.PI * 2f) / 512f;
            GameObject cubeInstance = (GameObject)Instantiate(cubePrefab);
            cubeInstance.transform.position = this.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            cubeInstance.transform.Rotate(0,0,angle*Mathf.Rad2Deg+90f);
            cubeInstance.transform.parent = this.transform;
            cubeArray[i] = cubeInstance;
        }
	
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
