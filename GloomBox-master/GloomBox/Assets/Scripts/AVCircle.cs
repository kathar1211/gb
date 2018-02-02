using UnityEngine;
using System.Collections;

public class AVCircle : MonoBehaviour {

    AudioSource m;
    public float[] samples = new float[512];
    public float[] samplesLeft = new float[512];
    public float[] samplesRight = new float[512];
    public float[] sampleBuffer = new float[64];
    public float[] bufferDecay = new float[64];
    public float[] bandBuffer = new float[64];
    public float[] bandBufferDecay = new float[64];

    float[] freqBands = new float[64];
    public GameObject cubePrefab;
    [SerializeField]
    float radius = 5f;
    GameObject[] cubeArray = new GameObject[512];
    float activeTime;

    // Use this for initialization
    void Start ()
    {
        m = GetComponentsInParent<AudioSource>()[0];
        for (int i = 0; i < 512; i++)
        {
            float angle = (i * Mathf.PI * 2f) / 512f;
            GameObject cubeInstance = (GameObject)Instantiate(cubePrefab);
            cubeInstance.transform.position = this.transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            cubeInstance.transform.Rotate(0, 0, angle * Mathf.Rad2Deg + 90f);
            cubeInstance.transform.parent = this.transform;
            cubeArray[i] = cubeInstance;
            cubeInstance.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        activeTime -= Time.deltaTime;
        if (activeTime < -0.1f)
        {
            for (int i = 0; i < 512; i++)
            {
                /*
                float newR = cubeArray[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.r;
                float newG = cubeArray[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.g;
                float newB = cubeArray[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.b;*/
                float newA = cubeArray[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color.a - 0.005f;
                cubeArray[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, newA);
            }
        }
        else
        {
            UpdateSpectrumData();
            UpdateSampleBuffer();
            //UpdateFreqBands();
            //UpdateBandBuffer();
            for (int i = 0; i < 512; i++)
            {
                float cubeScaleY = sampleBuffer[i % 64] * 200f;
                if (cubeScaleY < 0.5f)
                    cubeScaleY = 0.5f;
                if (cubeScaleY > 20f)
                    cubeScaleY = 20f;
                cubeArray[i].transform.localScale = new Vector3(1f, cubeScaleY, 1f);
                //cubeArray[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB((i % 512) / 512f, 0.75f, 1f);
                if ((5f - activeTime) > (i * 5f / 512f))
                {
                    //duration indicator
                    Color cubeColor = cubeArray[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
                    if (cubeColor.a > 0.25f)
                    {
                        cubeArray[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(cubeColor.r, cubeColor.g, cubeColor.b, 0.25f * cubeColor.a);
                    }
                }
            }
        }
        this.transform.Rotate(0, 0, Time.deltaTime * 10f);
        SetQuadState();
    }

    void UpdateSpectrumData()
    {
        m.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        m.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
        for(int i=0;i<512;i++)
        {
            samples[i] = (samplesLeft[i] + samplesRight[i])/2f;
        }
    }

    void UpdateFreqBands()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for(int i=0;i<8;i++)
        {
            float avg = 0f;
            //int sampleCount = (int) Mathf.Pow(2, i) * 2;
            if(i==16|| i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if(power==3)
                {
                    sampleCount -= 2;
                }
            }
            for(int j=0;j<sampleCount;j++)
            {
                avg += samples[count] * (count + 1f);
                count++;
            }
            avg /= count;
            freqBands[i] = avg;

        }
    }

    void UpdateSampleBuffer()
    {
        for(int i=0;i<64;i++)
        {
            if(sampleBuffer[i]<samples[i])
            {
                sampleBuffer[i] = samples[i];
                bufferDecay[i] = 0.0005f;
            }
            else if(sampleBuffer[i]>samples[i])
            {
                sampleBuffer[i] -= bufferDecay[i];
                bufferDecay[i] *= 1.2f;
            }
        }
    }

    void UpdateBandBuffer()
    {
        for (int i = 0; i < 64; i++)
        {
            if (bandBuffer[i] < freqBands[i])
            {
                bandBuffer[i] = freqBands[i];
                bandBufferDecay[i] = 0.0005f;
            }
            else if (bandBuffer[i] > freqBands[i])
            {
                bandBuffer[i] -= bandBufferDecay[i];
                bandBufferDecay[i] *= 1.2f;
            }
        }
    }

    void SetQuadState()
    {
        for(int i=0; i<512; i++)
        {
            if(cubeArray[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color.a > 0f)
            {
                cubeArray[i].SetActive(true);
            }
            else
            {
                cubeArray[i].SetActive(false);
            }
        }
    }

    public void Activate(float at, Color clr)
    {
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        activeTime = at;
        for(int i=0;i<512;i++)
        {
            cubeArray[i].SetActive(true);
            cubeArray[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(clr.r,clr.g,clr.b,1f);
            //cubeArray[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB((i % 512) / 512f, 1f, 1f);
        }
    }
}
