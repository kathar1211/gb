using UnityEngine;
using System.Collections;

public class WaterRenderer : MonoBehaviour {

    struct stWaterSpring
    {
        public float targetHeight;
        public Vector3 position;
        public float speed;
        public GameObject waterSpringObj;

        //update position info
        public void Update(float dampening, float tension)
        {
            float x = targetHeight - position.y;
            speed += tension * x - speed * dampening;
            if(Mathf.Abs(speed) > 2f)
            {
                speed = 0.1f * speed / Mathf.Abs(speed);
            }
            if (Mathf.Abs(position.y) < 5)
            position.y += speed;
        }

        //Update line renderer end point position after update the position info
        public void Draw()
        {
            waterSpringObj.GetComponent<LineRenderer>().SetPosition(1, position);
        }
    }

    public GameObject waterSpringGameObj;
    public float waterDepth;
    public int areaWidth = 250;
    public float tension = 0.01f;
    public float dampening = 0.001f;
    public float spread = 0.01f;
    private stWaterSpring[] springArray;
    
    float areaPosx;
    float areaPosy;
    float areaPosz;

    // Use this for initialization
    void Start()
    {
        //initialize water spring array
        //save water area position on clean code purpose
        areaWidth = (int)(GetComponent<BoxCollider2D>().bounds.size.x / 0.02f);
        if(!GetComponent<SpriteRenderer>().enabled)
        {
            waterDepth = 0.5f;//GetComponent<BoxCollider2D>().bounds.size.y;
        }

        areaPosx = transform.position.x;
        areaPosy = transform.position.y;
        areaPosz = transform.position.z;

        //generate the water spring array
        springArray = new stWaterSpring[areaWidth];
        //looping to set each spring struct
        for (int i = 0; i < areaWidth; i++)
        {
            springArray[i].targetHeight = areaPosy + waterDepth;
            //generate an instance spring obj for this struct
            springArray[i].waterSpringObj = Instantiate(waterSpringGameObj);
            LineRenderer lineRenderer = springArray[i].waterSpringObj.GetComponent<LineRenderer>();
            //set initial surface
            lineRenderer.SetPosition(0, new Vector3(areaPosx + i * 0.02f, areaPosy, areaPosz));
            lineRenderer.SetPosition(1, new Vector3(areaPosx + i * 0.02f, areaPosy + waterDepth, areaPosz));
            //initial speed zero
            springArray[i].speed = 0f;
            //position of surface point
            springArray[i].position = new Vector3(areaPosx + i * 0.02f, areaPosy + waterDepth, areaPosz);
        }
    }

    void UpdateWaterArea()
    {
        areaPosx = transform.position.x;
        areaPosy = transform.position.y;
        areaPosz = transform.position.z;
        //looping to update each spring struct
        for (int i = 0; i < areaWidth; i++)
        {
            //generate an instance spring obj for this struct
            LineRenderer lineRenderer = springArray[i].waterSpringObj.GetComponent<LineRenderer>();
            //update surface height
            lineRenderer.SetPosition(0, new Vector3(areaPosx + i * 0.02f, areaPosy, areaPosz));
            lineRenderer.SetPosition(1, new Vector3(areaPosx + i * 0.02f, areaPosy + waterDepth, areaPosz));
            //position of surface point
            springArray[i].position = new Vector3(areaPosx + i * 0.02f, springArray[i].position.y + areaPosy + waterDepth - springArray[i].targetHeight, areaPosz);

            springArray[i].targetHeight = areaPosy + waterDepth;
        }
        //new targetHeight
    }

    void HandleSpringPhysics()
    {
        //Update spring height first
        for (int i = 0; i < areaWidth; i++)
            springArray[i].Update(dampening, tension);

        float[] lDeltas = new float[areaWidth];
        float[] rDeltas = new float[areaWidth];

        //calculate spring pull neighbours
        for (int j = 0; j < 100; j++)
        {
            for (int i = 0; i < areaWidth; i++)
            {
                if (i > 0)
                {
                    lDeltas[i] = spread * (springArray[i].position.y - springArray[i - 1].position.y);
                    springArray[i - 1].speed += lDeltas[i];
                    springArray[i - 1].position.y += lDeltas[i];
                }

                if (i < areaWidth - 1)
                {
                    rDeltas[i] = spread * (springArray[i].position.y - springArray[i + 1].position.y);
                    springArray[i + 1].speed += rDeltas[i];
                    springArray[i + 1].position.y += rDeltas[i];
                }
            }
        }

        for (int i = 0; i < areaWidth; i++)
        {
            if (i > 0)
            {
                springArray[i - 1].position.y += lDeltas[i];
            }

            if (i < areaWidth - 1)
            {
                springArray[i + 1].position.y += rDeltas[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //waterDepth = GetComponent<BoxCollider2D>().bounds.size.y;
        //Update water area while depth be changed
        if (springArray[0].targetHeight != areaPosy + waterDepth || areaPosx != transform.position.x || areaPosy != transform.position.y)
        {
            UpdateWaterArea();
        }

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    springArray[areaWidth/2].speed = -1f;
        //}

        if(Mathf.Abs(springArray[0].speed) < 0.0001f)
        {
            springArray[0].speed = -1 * (float)areaWidth / 3000;
        }

        if(Mathf.Abs(springArray[areaWidth-1].speed) < 0.0001f)
        {
            springArray[areaWidth - 1].speed = -1 * (float)areaWidth / 3000;
        }

        HandleSpringPhysics();
        
        //Render a new frame of water
        for (int i = 0; i < areaWidth; i++)
        {
            springArray[i].Draw();
        }
    }
}
