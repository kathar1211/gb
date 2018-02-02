using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    public float speed = 6.0F;
    public float airspeed = 3.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float musicradius;
    private Vector3 moveDirection = Vector3.zero;
    AudioSource ad;
    public AudioClip heavymetalsound;
    public AudioClip bluessound;
    public AudioClip choralsound;

    void Start()
    {
        ad = GetComponent<AudioSource>();
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);
        PlayMusic();
        //fade out sounds
        if((ad.isPlaying)&&(ad.volume > 0))
        {
            ad.volume -= Time.deltaTime*0.35f;
        }
        //Debug.Log(ad.isPlaying);
    }

    void PlayMusic()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            HeavyMetal();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Blues();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Choir();
    }

    void HeavyMetal()
    {
        ad.clip = heavymetalsound;
        ad.volume = 1.0f;
        ad.Play();
        Collider[] monsters = Physics.OverlapSphere(this.transform.position, musicradius);
        foreach(Collider m in monsters)
        {
            if (m.gameObject.CompareTag("Monster"))
            {
                StartCoroutine(m.GetComponent<MonsterScript>().SetText("I'm heavy!"));
                m.gameObject.GetComponent<Rigidbody>().mass = 10;
            }
        }
    }

    void Blues()
    {
        ad.clip = bluessound;
        ad.volume = 1.0f;
        ad.Play();
        Collider[] monsters = Physics.OverlapSphere(this.transform.position, musicradius);
        foreach (Collider m in monsters)
        {

            if (m.gameObject.CompareTag("Monster"))
            {
                StartCoroutine(m.GetComponent<MonsterScript>().SetText("I'm sad!"));

                ParticleSystem ps = m.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();

                StopCoroutine("Emit");
                StartCoroutine("Emit", m.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>());

                //make monster cry visually
                //m.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                //resetting particle system
                //cause connected waterplane to expand

                m.gameObject.GetComponent<MonsterScript>().Cry();
            }
        }
    }

    IEnumerator Emit(ParticleSystem ps)
    {
        for(int i = 0; i < 150; i++)
        {
            ps.Emit(1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Choir()
    {
        ad.clip = choralsound;
        ad.volume = 1.0f;
        ad.Play();
        Collider[] monsters = Physics.OverlapSphere(this.transform.position, musicradius);
        foreach (Collider m in monsters)
        {
            if (m.gameObject.CompareTag("Monster"))
            {
                StartCoroutine(m.GetComponent<MonsterScript>().SetText("I'm upside-down!"));
                m.gameObject.GetComponent<MonsterScript>().isgravityinverted = true;
                //Debug.Log("choirmonster");
            }
                
        }
    }
}