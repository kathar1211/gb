using UnityEngine;
using System.Collections;

public class MonsterSpawner : TriggerController {

    public bool isActive;
    [SerializeField]
    Vector3 SpawnOffset;
    [SerializeField]
    GameObject MonsterPrefab;
    [SerializeField]
    bool isMonsterWandering;
    [SerializeField]
    bool isMonsterFacingRight;
    [SerializeField]
    float monsterWanderSpeed;
    [SerializeField]
    float monsterLeftBoundary;
    [SerializeField]
    float monsterRightBoundary;
    [SerializeField]
    float SpawnDelay;
    float timecounter;
    

	// Use this for initialization
	void Start ()
    {

	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timecounter += Time.deltaTime;
        if(isActive && timecounter>=SpawnDelay)
        {
            timecounter = 0f;
            //Spawn Monster at specified position
            GameObject m = (GameObject)Instantiate(MonsterPrefab);
            m.transform.position = this.transform.position + SpawnOffset;
            MonsterController mc = m.GetComponent<MonsterController>();
            mc.facingRight = isMonsterFacingRight;
            mc.isWandering = isMonsterWandering;
            mc.wanderSpeed = monsterWanderSpeed;
            mc.leftBoundary = monsterLeftBoundary;
            mc.rightBoundary = monsterRightBoundary;
        }
	
	}

    public override void OnTrigger()
    {
        isActive = true;
    }

    public override void TriggerOff()
    {
        isActive = false;
    }
}
