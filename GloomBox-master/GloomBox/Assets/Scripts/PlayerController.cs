#region Using Directives
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
#endregion

#region PlayerController Class
public class PlayerController : MonoBehaviour
{
    #region Public Variables
    public float moveSpeed = 5.0f;
    public float jumpForce = 30.0f;
    public float musicRadius = 3f;
    public GameObject groundCheck;
    public AudioClip[] musicClips;
    public float inAirGravityScale = 3f;
    public Sprite idlePose;
    public Sprite playMusicPose;
    #endregion

    #region Objects Attached to Player
    [SerializeField]
    GameObject aoeCircle;
    [SerializeField]
    GameObject AVCircle;
    [SerializeField]
    GameObject mainCam;
    [SerializeField]
    GameObject fadeout;
	[SerializeField]
	GameObject background;
    //[SerializeField]
    //GameObject musicCooldown;
    [SerializeField]
    float musicCooldownTime;
    GameObject xButton;
    GameObject yButton;
    GameObject bButton;
    #endregion

    #region Camera Look Variables
    [SerializeField]
    float cameraBottomY;
    float cameraOrigY;
    [SerializeField]
    float cameraTopY;
    [SerializeField]
    float cameraLerpSpeed;
    [SerializeField]
    float cameraRightX;
    [SerializeField]
    float cameraLeftX;
    [SerializeField]
    float cameraLerpXTime;
    bool cameraMovingRight;
	Vector3 camOldPos;
	bool camSlidRight;
	bool camSlidLeft;
	bool frozenCamX;
	bool frozenCamY;
    #endregion

    #region Private Variables
    AudioSource musicClip;
    AudioSource backgroundBeat;
    AudioSource cutSound;
    AudioSource boomboxButtonPress;
    Rigidbody2D rigidBody;
    bool onMovingPlatform;
    Vector2 platformVelocity;
    Transform checkPoint;
    GameObject spawnPoint;
	bool isMoving;

    TypesOfMusic typeOfMusicPlaying;
    //float nextBluesTime = 0.0f;
    //float nextChoralTime = 0.0f;
    //float nextHeavyMetalTime = 0.0f;
    bool bluesQueued;
    bool choralQueued;
    bool heavyMetalQueued;
    float bluesQueuedTime;
    float choralQueuedTime;
    float heavyMetalQueuedTime;
    bool bluesEnabled;
    bool choralEnabled;
    bool heavyMetalEnabled;
    bool inputEnabled;
	bool bluesSec2;
	bool choralSec2;
	bool heavyMetalSec2;

    float idleTime = 0.1f;

    bool lookingForMashupInput = true;
    List<char> mashupKeypressList;

    bool jumpButtonReleased;
    public bool bouncing = false;
    bool justBounced;

    Coroutine cameraLerpCoroutine = null;
    Coroutine cameraLerpXCoroutine = null;
    bool movingCameraBack = false;

    Coroutine musicCooldownCoroutine = null;

    Animator anim;
    #endregion

    #region Properties
    public bool BluesEnabled
    {
        get { return bluesEnabled; }
    }

    public bool ChoralEnabled
    {
        get { return choralEnabled; }
    }

    public bool HeavyMetalEnabled
    {
        get { return heavyMetalEnabled; }
    }

    public bool BluesQueued
    {
        set
        {
            bluesQueued = value;

            if(bluesQueued)
            {
                bluesQueuedTime = Time.time;
                //xButton.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
                xButton.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            }
            else
            {
				//xButton.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                xButton.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public bool ChoralQueued
    {
        set
        {
            choralQueued = value;

            if(choralQueued)
            {
                choralQueuedTime = Time.time;
                //yButton.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
                yButton.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                //yButton.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                yButton.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public bool HeavyMetalQueued
    {
        set
        {
            heavyMetalQueued = value;

            if(heavyMetalQueued)
            {
                heavyMetalQueuedTime = Time.time;
				//bButton.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
                bButton.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                //bButton.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                bButton.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public bool JustBounced
    {
        get { return justBounced; }
        set
        {
            justBounced = value;

            if(justBounced)
            {
                StartCoroutine(ResetJustBounced());
            }
        }
    }

	public bool FrozenCamX
	{
		get{ return frozenCamX; }
		set{ frozenCamX = value; }
	}

	public bool FrozenCamY
	{
		get{ return frozenCamY; }
		set{ frozenCamY = value; }
	}

	public bool InputEnabled
	{
		set{ inputEnabled = value; }
	}
    #endregion

	public void FindUIButtons()
	{
		//Get references to UI Buttons and disable them
		xButton = GameObject.Find("X_GUI");
		yButton = GameObject.Find("Y_GUI");
		bButton = GameObject.Find("B_GUI");
		xButton.SetActive(false);
		yButton.SetActive(false);
		bButton.SetActive(false);
	}

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        musicClip = GetComponents<AudioSource>()[0];
        backgroundBeat = GetComponents<AudioSource>()[1];
        cutSound = GetComponents<AudioSource>()[2];
        boomboxButtonPress = GetComponents<AudioSource>()[3];
        typeOfMusicPlaying = TypesOfMusic.None;

        //generate a spawnPoint
        spawnPoint = new GameObject();
        spawnPoint.transform.position = transform.position;
        checkPoint = spawnPoint.transform;
        anim = GetComponent<Animator>();

        aoeCircle.transform.localScale *= musicRadius;

        mashupKeypressList = new List<char>();
        inputEnabled = true;

        cameraOrigY = mainCam.transform.localPosition.y;
		camOldPos = mainCam.transform.position;

		if(SceneManager.GetActiveScene().name != "Level0_Tutorial")
		{
			SetGenreEnabledOrDisabled(TypesOfMusic.Blues, true);
			SetGenreEnabledOrDisabled(TypesOfMusic.Choral, true);
			SetGenreEnabledOrDisabled(TypesOfMusic.HeavyMetal, true);
			//bluesEnabled = choralEnabled = heavyMetalEnabled = true;
		}

        boomboxButtonPress.Play();
		StartCoroutine(PlayMusicPose());
    }

    // Update is called once per frame
    void Update()
	{
        if(inputEnabled)
        {
            idleTime += Time.deltaTime;

            if(idleTime >= 0.05f)
            {
                anim.SetBool("IsIdle", true);
            }
            else
            {
                anim.SetBool("IsIdle", false);
            }

            //jump animation
            if(rigidBody.velocity.y > 0)
            {
                anim.SetFloat("yDirection", 1f);
            }
            else if(rigidBody.velocity.y < -10)
            {
                anim.SetFloat("yDirection", -1f);
            }

            if(KeyboardState.controllerAxesActive.Count != 0 || KeyboardState.controllerButtonsPressed.Count != 0 || KeyboardState.keysPressed.Count != 0)
            {
                idleTime = 0f;
            }

            HandleMovement();
            HandleMonsters();
        }

		HandleMusic();

        rigidBody.AddForce(Vector2.zero);
		camOldPos = mainCam.transform.position;
    }

    #region Input Handlers
    /// <summary>
    /// Handle input having to do with movement
    /// </summary>
    void HandleMovement()
    {
        // Move horizontally
        if(KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cRight]) > 0.3f || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kRight]))
        {
			isMoving = true;
			camSlidLeft = false;

			//if(GetComponentInChildren<Camera>().gameObject.GetComponentInChildren<MeshCollider>().bounds.Contains(transform.position))
			//if(transform.position.x < mainCam.GetComponentInChildren<MeshCollider>().bounds.max.x && !camSlidRight)
			if(frozenCamX && frozenCamY)
			{
				mainCam.transform.position = new Vector3(camOldPos.x, camOldPos.y, mainCam.transform.position.z);
			}
			else if(frozenCamX)
			{
				mainCam.transform.position = new Vector3(camOldPos.x, mainCam.transform.position.y, mainCam.transform.position.z);
			}
			else if(frozenCamY)
			{
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, camOldPos.y, mainCam.transform.position.z);
			}
			else
			{
				background.transform.Translate(-0.001f * GetComponent<Rigidbody2D>().velocity);

				/*if(!camSlidRight)
				{
					if(cameraLerpXCoroutine != null)
					{
						StopCoroutine(cameraLerpXCoroutine);
					}

					cameraLerpXCoroutine = StartCoroutine(SlideCamera(true));
				}*/
			}

            rigidBody.velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            this.GetComponent<SpriteRenderer>().flipX = false;
            anim.SetBool("IsRunning", true);

            if(cameraLerpXCoroutine == null)
            {
                //cameraLerpXCoroutine = StartCoroutine(LerpCameraX(true, cameraLerpXTime));
            }
            else if(cameraMovingRight == false)
            {
                //StopCoroutine(cameraLerpXCoroutine);
                //cameraLerpXCoroutine = StartCoroutine(LerpCameraX(true, cameraLerpXTime));
            }
        }
        else if(KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cLeft]) < -0.3f || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kLeft]))
        {
			isMoving = true;
			camSlidRight = false;

			//if(transform.position.x > mainCam.GetComponentInChildren<MeshCollider>().bounds.min.x && !camSlidLeft)
			if(frozenCamX && frozenCamY)
			{
				mainCam.transform.position = new Vector3(camOldPos.x, camOldPos.y, mainCam.transform.position.z);
			}
			else if(frozenCamX)
			{
				mainCam.transform.position = new Vector3(camOldPos.x, mainCam.transform.position.y, mainCam.transform.position.z);
			}
			else if(frozenCamY)
			{
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, camOldPos.y, mainCam.transform.position.z);
			}
			else
			{
				background.transform.Translate(-0.001f * GetComponent<Rigidbody2D>().velocity);
				/*if(!camSlidLeft)
				{
					if(cameraLerpXCoroutine != null)
					{
						StopCoroutine(cameraLerpXCoroutine);
					}

					cameraLerpXCoroutine = StartCoroutine(SlideCamera(false));
				}*/
			}

            rigidBody.velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            this.GetComponent<SpriteRenderer>().flipX = true;
            anim.SetBool("IsRunning", true);

            if(cameraLerpXCoroutine == null)
            {
                //cameraLerpXCoroutine = StartCoroutine(LerpCameraX(false, cameraLerpXTime));
            }
            else if(cameraMovingRight == true)
            {
                //StopCoroutine(cameraLerpXCoroutine);
                //cameraLerpXCoroutine = StartCoroutine(LerpCameraX(false, cameraLerpXTime));
            }
        }
        else
        {
			if(frozenCamX && frozenCamY)
			{
				mainCam.transform.position = new Vector3(camOldPos.x, camOldPos.y, mainCam.transform.position.z);
			}
			else if(frozenCamX)
			{
				mainCam.transform.position = new Vector3(camOldPos.x, mainCam.transform.position.y, mainCam.transform.position.z);
			}
			else if(frozenCamY)
			{
				mainCam.transform.position = new Vector3(mainCam.transform.position.x, camOldPos.y, mainCam.transform.position.z);
			}

            rigidBody.velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
            anim.SetBool("IsRunning", false);

            if(cameraLerpXCoroutine != null)
			{
				isMoving = false;

                //StopCoroutine(cameraLerpXCoroutine);
                //cameraLerpXCoroutine = null;
            }
        }

        // Camera look
		/*if((KeyboardState.IsAxisJustActivated(InputManager.stringToControllerAxis[InputManager.cUp]) && (KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cUp]) < -0.3f)) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kUp]))
        {
            anim.SetBool("LookingUp", true);

            movingCameraBack = false;

            if (cameraLerpCoroutine != null)
            {
                StopCoroutine(cameraLerpCoroutine);
            }

            cameraLerpCoroutine = StartCoroutine(LerpCamera(cameraTopY, 1f));
        }
		else if((KeyboardState.IsAxisJustActivated(InputManager.stringToControllerAxis[InputManager.cDown]) && (KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cDown]) > 0.3f)) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kDown]))
        {
            anim.SetBool("LookingDown", true);

            movingCameraBack = false;

            if (cameraLerpCoroutine != null)
            {
                StopCoroutine(cameraLerpCoroutine);
            }

            cameraLerpCoroutine = StartCoroutine(LerpCamera(cameraBottomY, 1.5f));
        }
        else
        {
            if (!(KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cUp]) < -0.3f) &&
               !KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kUp]) &&
               !(KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cDown]) > 0.3f) &&
               !KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kDown]))
            {
                anim.SetBool("LookingUp", false);
                anim.SetBool("LookingDown", false);
            }

            if (mainCam.transform.localPosition.y != cameraOrigY && !movingCameraBack &&
               !(KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cUp]) < -0.3f) &&
               !KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kUp]) &&
               !(KeyboardState.GetAxisValue(InputManager.stringToControllerAxis[InputManager.cDown]) > 0.3f) &&
               !KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kDown]))
            {
                movingCameraBack = true;

                if (cameraLerpCoroutine != null)
                {
                    StopCoroutine(cameraLerpCoroutine);
                }

                cameraLerpCoroutine = StartCoroutine(LerpCamera(cameraOrigY, 0.5f));
            }
            else if (mainCam.transform.localPosition.y == cameraOrigY)
            {
                movingCameraBack = false;
            }
        }*/

        // Move with moving platform
        if(onMovingPlatform)
        {
            rigidBody.velocity += new Vector2(platformVelocity.x, GetComponent<Rigidbody2D>().velocity.y);
        }

        // Jump
        if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cJump]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kJump])) && jumpButtonReleased)
        {
            if (!bouncing)
            {
                jumpButtonReleased = false;
                HandleJump(false, new Vector2(0, jumpForce));
            }
        }

        if(!(KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cJump]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kJump])))
        {
            rigidBody.gravityScale = 10f;
            jumpButtonReleased = true;
        }
        //falling speed
        if(rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = inAirGravityScale;
        }

        // Animation stuff
        //anim.SetBool("IsJumping", !groundCheck.GetComponent<GroundCheckController>().canJump);
        anim.SetBool("IsJumping", !groundCheck.GetComponent<GroundCheckController>().grounded);
        anim.SetFloat("vSpeed", rigidBody.velocity.y);

        //Reset level
        if(KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cReset]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kReset]))
        {
            GameObject.Find("LevelManager").GetComponent<LevelManager>().ResetLevel();
        }
    }

    /// <summary>
    /// Handle input having to do with jump
    /// </summary>
    /// <param name="bounceoff">If set to <c>true</c> this is a bounce off of a monster's head</param>
    /// <param name="jumpforce">The jump force on this rigidbody</param>
    public void HandleJump(bool bounceoff, Vector2 jumpforce)
    {
        if(!bounceoff)
        {
            rigidBody.gravityScale = inAirGravityScale;
        }

        if(groundCheck.GetComponent<GroundCheckController>().canJump)
        {
            groundCheck.GetComponent<GroundCheckController>().canJump = false;
            if(bounceoff)
            {
                rigidBody.AddForce(jumpforce);
            }
            else
            {
                rigidBody.AddForce(new Vector2(0f, jumpForce));
            }
        }
    }

    /// <summary>
    /// Handle input having to do with music
    /// </summary>
    void HandleMusic()
    {
        // Music and aoeCircle fadeout
        if((musicClip.isPlaying) && (musicClip.volume > 0))
        {
            musicClip.volume -= Time.deltaTime * (1 / musicCooldownTime);

            aoeCircle.SetActive(true);
            Color temp = aoeCircle.GetComponent<SpriteRenderer>().color;
            temp.a = musicClip.volume;
            aoeCircle.GetComponent<SpriteRenderer>().color = temp;
        }
        else
        {
            aoeCircle.SetActive(false);
            backgroundBeat.volume = 1f;
        }

		if(inputEnabled)
		{
			// Mashup input
			if(KeyboardState.IsAxisJustActivated(InputManager.stringToControllerAxis[InputManager.cPlay]) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kPlay]))
			{
				PlayMashup();
			}

			if(KeyboardState.IsAxisJustActivated(InputManager.stringToControllerAxis[InputManager.cStop]) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kStop]))
			{
				StopMusic();
				BluesQueued = ChoralQueued = HeavyMetalQueued = false;
				/*if (typeOfMusicPlaying != TypesOfMusic.None)*/
            
				AVCircle.GetComponent<AVCircle>().Activate(0, new Color(0f, 0f, 0f));
            

			}

			/*else
		{
			if(lookingForMashupInput)
			{
				//lookingForMashupInput = false;
				PlayMashup();
			}
		}*/

			// Single ability input
			if(!lookingForMashupInput)
			{
				if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cMetal]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kMetal])) /*&& Time.time > nextHeavyMetalTime*/ && heavyMetalEnabled)
				{
					PlayMusic(TypesOfMusic.HeavyMetal);
				}

				if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cBlues]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kBlues])) /*&& Time.time > nextBluesTime*/ && bluesEnabled)
				{
					PlayMusic(TypesOfMusic.Blues);
				}

				if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cChoral]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kChoral])) /*&& Time.time > nextChoralTime*/ && choralEnabled)
				{
					PlayMusic(TypesOfMusic.Choral);
				}
			}
			else
			{
				ReceiveMashupInput();
			}
		}
    }

    /// <summary>
    /// Handle input for mashup abilities
    /// </summary>
    void ReceiveMashupInput()
    {
        if((KeyboardState.IsButtonJustPressed(InputManager.stringToControllerButton[InputManager.cMetal]) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kMetal])) && heavyMetalEnabled)
        {
            HeavyMetalQueued = !heavyMetalQueued;
            DequeueOldest();
        }

        if((KeyboardState.IsButtonJustPressed(InputManager.stringToControllerButton[InputManager.cBlues]) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kBlues])) && bluesEnabled)
        {
            BluesQueued = !bluesQueued;
            DequeueOldest();
        }

        if((KeyboardState.IsButtonJustPressed(InputManager.stringToControllerButton[InputManager.cChoral]) || KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kChoral])) && choralEnabled)
        {
            ChoralQueued = !choralQueued;
            DequeueOldest();
        }

        //		if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cMetal]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kMetal])) /*&& Time.time > nextHeavyMetalTime*/ && heavyMetalEnabled)
        //		{
        //			mashupKeypressList.Add('h');
        //		}
        //
        //		if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cBlues]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kBlues])) /*&& Time.time > nextBluesTime*/ && bluesEnabled)
        //		{
        //			mashupKeypressList.Add('b');
        //		}
        //
        //		if((KeyboardState.IsButtonPressed(InputManager.stringToControllerButton[InputManager.cChoral]) || KeyboardState.IsKeyPressed(InputManager.stringToKey[InputManager.kChoral])) /*&& Time.time > nextChoralTime*/ && choralEnabled)
        //		{
        //			mashupKeypressList.Add('c');
        //		}
    }

    void DequeueOldest()
    {
        if(bluesQueued && choralQueued && heavyMetalQueued)
        {
            if (bluesQueuedTime < choralQueuedTime)
            {
                if (bluesQueuedTime < heavyMetalQueuedTime)
                {
                    BluesQueued = false;
                }
                else
                {
                    HeavyMetalQueued = false;
                }
            }
            else
            {
                if (choralQueuedTime < heavyMetalQueuedTime)
                {
                    ChoralQueued = false;
                }
                else
                {
                    HeavyMetalQueued = false;
                }
            }
        }
    }
    #endregion

    #region Send Music to Monsters
    /// <summary>
    /// Send the music that is currently playing to monsters within the music radius
    /// </summary>
    void HandleMonsters()
    {
        if (typeOfMusicPlaying != TypesOfMusic.None)
        {
            Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, musicRadius, 1 << LayerMask.NameToLayer("NPC"));

            foreach (Collider2D col2D in monsters)
            {
                if (col2D.gameObject.name.Contains("Monster"))
                {
                    col2D.gameObject.GetComponent<MonsterController>().ReceiveMusic(typeOfMusicPlaying);
                }
            }
        }
    }
    #endregion

    #region Musical Functions
    void PlayMusic(TypesOfMusic musicType)
    {
        if (musicType == TypesOfMusic.None)
        {
            return;
        }

        cutSound.Play();
        boomboxButtonPress.Play();
        StartCoroutine(PlayMusicPose());
        StartCoroutine(FadeSound(backgroundBeat, 0f, true));

		switch(musicType)
		{
			case TypesOfMusic.Blues:
				if(bluesSec2)
				{
					PlayClip(musicClips[7]);
					bluesSec2 = !bluesSec2;
				}
				else
				{
					PlayClip(musicClips[(int)musicType]);
					bluesSec2 = !bluesSec2;
				}
				break;
			case TypesOfMusic.Choral:
				if(choralSec2)
				{
					PlayClip(musicClips[9]);
					choralSec2 = !choralSec2;
				}
				else
				{
					PlayClip(musicClips[(int)musicType]);
					choralSec2 = !choralSec2;
				}
				break;
			case TypesOfMusic.HeavyMetal:
				if(heavyMetalSec2)
				{
					PlayClip(musicClips[8]);
					heavyMetalSec2 = !heavyMetalSec2;
				}
				else
				{
					PlayClip(musicClips[(int)musicType]);
					heavyMetalSec2 = !heavyMetalSec2;
				}
				break;
			default:
				PlayClip(musicClips[(int)musicType]);
				break;
		}

        typeOfMusicPlaying = musicType;

        // Update cooldown time
        /*switch(musicType)
        {
            case TypesOfMusic.Blues:
                nextBluesTime = Time.time + musicCooldownTime;
                break;
            case TypesOfMusic.Choral:
                nextChoralTime = Time.time + musicCooldownTime;
                break;
            case TypesOfMusic.HeavyMetal:
                nextHeavyMetalTime = Time.time + musicCooldownTime;
                break;
            case TypesOfMusic.BluesChoral:
                nextBluesTime = Time.time + musicCooldownTime;
                nextChoralTime = Time.time + musicCooldownTime;
                break;
            case TypesOfMusic.BluesHeavyMetal:
                nextBluesTime = Time.time + musicCooldownTime;
                nextHeavyMetalTime = Time.time + musicCooldownTime;
                break;
            case TypesOfMusic.ChoralHeavyMetal:
                nextChoralTime = Time.time + musicCooldownTime;
                nextHeavyMetalTime = Time.time + musicCooldownTime;
                break;
            default:
                break;
        }*/
    }

    void StopMusic()
    {
		if(musicClip.isPlaying)
		{
			cutSound.Play();
			musicClip.Stop();
			boomboxButtonPress.Play();
			StartCoroutine(PlayMusicPose());
		}

        typeOfMusicPlaying = TypesOfMusic.None;
    }

    /// <summary>
    /// Play a mashup audio cue
    /// </summary>
    void PlayMashup()
    {
        PlayMusic(HandleMashupInput());
        if ((typeOfMusicPlaying != TypesOfMusic.None)&&(bluesQueued||choralQueued||heavyMetalQueued))
        {
            switch (typeOfMusicPlaying)
            {
                case TypesOfMusic.Blues:
                    AVCircle.GetComponent<AVCircle>().Activate(5f, new Color(0f, 0f, 1f));
                    break;
                case TypesOfMusic.Choral:
                    AVCircle.GetComponent<AVCircle>().Activate(5f, new Color(1f, 1f, 0f));
                    break;
                case TypesOfMusic.HeavyMetal:
                    AVCircle.GetComponent<AVCircle>().Activate(5f, new Color(1f, 0f, 0f));
                    break;
                case TypesOfMusic.BluesChoral:
                    AVCircle.GetComponent<AVCircle>().Activate(5f, new Color(0f, 1f, 0f));
                    break;
                case TypesOfMusic.BluesHeavyMetal:
                    AVCircle.GetComponent<AVCircle>().Activate(5f, new Color(1f, 0f, 1f));
                    break;
                case TypesOfMusic.ChoralHeavyMetal:
                    AVCircle.GetComponent<AVCircle>().Activate(5f, new Color(1f, 0.5f, 0.15f));
                    break;
            }
        }
        BluesQueued = ChoralQueued = HeavyMetalQueued = false;
        mashupKeypressList.Clear();
    }

    /// <summary>
    /// Handle input having to do with mashups
    /// </summary>
    /// <returns>The mashup input.</returns>
    TypesOfMusic HandleMashupInput()
    {
        if (bluesQueued)
        {
            if (choralQueued)
            {
                return TypesOfMusic.BluesChoral;
            }
            else if (heavyMetalQueued)
            {
                return TypesOfMusic.BluesHeavyMetal;
            }
            else
            {
                return TypesOfMusic.Blues;
            }
        }
        else if (choralQueued)
        {
            if (heavyMetalQueued)
            {
                return TypesOfMusic.ChoralHeavyMetal;
            }
            else
            {
                return TypesOfMusic.Choral;
            }
        }
        else if (heavyMetalQueued)
        {
            return TypesOfMusic.HeavyMetal;
        }
        else
        {
            return TypesOfMusic.None;
        }

        // Make sure the list of keypresses has at least 2 elements
        // Otherwise, there can't be a mashup
        /*if(mashupKeypressList.Count >= 2)
		{
			char lastChar = mashupKeypressList[mashupKeypressList.Count - 1];
			char secondLastChar = mashupKeypressList[mashupKeypressList.Count - 2];

			int i = 3;
			bool foundDifferentChars = false;

			// Find final two different characters
			while(lastChar == secondLastChar)
			{
				if(mashupKeypressList.Count < i)
				{
					break;
				}

				secondLastChar = mashupKeypressList[mashupKeypressList.Count - i];

				if(lastChar != secondLastChar)
				{
					foundDifferentChars = true;
				}

				i++;
			}

			// Play mashup music based on characters found
			if(foundDifferentChars)
			{
				if((lastChar == 'b' && secondLastChar == 'c') || (lastChar == 'c' && secondLastChar == 'b'))
				{
					return TypesOfMusic.BluesChoral;
				}
				else if((lastChar == 'b' && secondLastChar == 'h') || (lastChar == 'h' && secondLastChar == 'b'))
				{
					return TypesOfMusic.BluesHeavyMetal;
				}
				else if((lastChar == 'c' && secondLastChar == 'h') || (lastChar == 'h' && secondLastChar == 'c'))
				{
					return TypesOfMusic.ChoralHeavyMetal;
				}
			}
		}*/
    }

    /// <summary>
    /// Plays an audio clip
    /// </summary>
    /// <param name="clip">The clip to play</param>
    void PlayClip(AudioClip clip)
    {
        musicClip.clip = clip;
        musicClip.volume = 0f;
        StartCoroutine(FadeSound(musicClip, 1f, false));
        musicClip.Play();

        if (musicCooldownCoroutine != null)
        {
            StopCoroutine(musicCooldownCoroutine);
        }
        musicCooldownCoroutine = StartCoroutine(MusicCooldown(musicCooldownTime));
    }

    /// <summary>
    /// Cooldown the player's music abilities
    /// </summary>
    /// <param name="time">The time to cool down</param>
    IEnumerator MusicCooldown(float time)
    {
        //musicCooldown.GetComponent<Renderer>().enabled = true;
        float rate = 1 / time;
        float i = 0;

        // Cut out the cooldown circle with time
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            //musicCooldown.GetComponent<Renderer>().material.SetFloat("_Cutoff", i);
            yield return 0;
        }

        typeOfMusicPlaying = TypesOfMusic.None;
    }

    public void SetGenreEnabledOrDisabled(TypesOfMusic genre, bool enabled)
    {
		if(enabled && (musicClip != null))
		{
			if(musicClip.isPlaying)
			{
				musicClip.Stop();
				AVCircle.GetComponent<AVCircle>().Activate(0f, new Color(0f, 0f, 0f));
			}
		}

        switch(genre)
        {
			case TypesOfMusic.Blues:
				xButton.SetActive(enabled);
                bluesEnabled = enabled;
                break;
            case TypesOfMusic.Choral:
				yButton.SetActive(enabled);
                choralEnabled = enabled;
                break;
            case TypesOfMusic.HeavyMetal:
				bButton.SetActive(enabled);
                heavyMetalEnabled = enabled;
                break;
            default:
                break;
        }
    }

    IEnumerator PlayMusicPose()
    {
        anim.SetBool("UsedAbility", true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("UsedAbility", false);
    }
    #endregion

    #region Moving Platform Functions
    /// <summary>
    /// Set the player onto or off of a moving platform
    /// </summary>
    /// <param name="isOn">If set to <c>true</c> is on the platform</param>
    /// <param name="platVel">Platform velocity</param>
    public void setMovingPlatform(bool isOn, Vector2 platVel)
    {
        onMovingPlatform = isOn;
        platformVelocity = platVel;
    }
    #endregion

    #region Checkpoint Functions
    /// <summary>
    /// Set the player's last checkpoint
    /// </summary>
    /// <param name="checkPointTrans">Checkpoint transform</param>
    public void SetCheckPoint(Transform checkPointTrans)
    {
        checkPoint = checkPointTrans;
    }

    /// <summary>
    /// Gets the player's last checkpoint
    /// </summary>
    /// <returns>The transform of the checkpoint</returns>
    public Transform GetCheckPoint()
    {
        return checkPoint;
    }

    /// <summary>
    /// Sends the player back to the last checkpoint
    /// Sort of like death, but not named that way
    /// </summary>
    public IEnumerator BackToCheckPoint()
    {
        // Freeze movement
        rigidBody.velocity = Vector2.zero;
        inputEnabled = false;
        Time.timeScale = 0f;

        // Fade to black
        Color c = fadeout.GetComponent<SpriteRenderer>().color;
        while (c.a < 1f)
        {
            if (musicClip.isPlaying)
            {
                musicClip.volume -= 0.03f;
            }

            if (backgroundBeat.isPlaying)
            {
                backgroundBeat.volume -= 0.03f;
            }

            c.a = c.a + 0.03f;
            fadeout.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a);
            yield return new WaitForSeconds(0f);
        }

        // Set everything back to initial values
        transform.position = checkPoint.position;
        rigidBody.velocity = Vector2.zero;
        anim.SetBool("IsRunning", false);
		anim.SetBool("IsJumping", false);
        this.GetComponent<SpriteRenderer>().flipX = false;
        Input.ResetInputAxes();
        aoeCircle.SetActive(false);
        //musicCooldown.GetComponent<Renderer>().enabled = false;
        typeOfMusicPlaying = TypesOfMusic.None;

        /*
		// Reset monster state as well
		GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
		foreach(GameObject m in monsters)
		{
			if(m.name.Contains("Monster"))
			{
				m.GetComponent<MonsterController>().ResetState();
			}
		}*/

        // Fade back in from black
        while (c.a > 0f)
        {
            c.a = c.a - 0.06f;
            fadeout.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a);
            yield return new WaitForSeconds(0f);
        }

        GameObject.Find("LevelManager").GetComponent<LevelManager>().ResetLevel();

        // Allow movement again
        inputEnabled = true;
        rigidBody.velocity = Vector2.zero;
        Time.timeScale = 1f;

        // Reset music ability cooldowns
        /*nextBluesTime = Time.time;
		nextChoralTime = Time.time;
		nextHeavyMetalTime = Time.time;*/

        StopAllCoroutines();
    }
    #endregion

    #region Camera Control Functions
    /// <summary>
    /// Lerps the camera toward a particular Y value
    /// </summary>
    /// <param name="yToLerpTo">The Y position to lerp toward</param>
    IEnumerator LerpCamera(float yToLerpTo, float timeToGetThere)
    {
        float startTime = Time.time;
        Vector3 startPos = mainCam.transform.localPosition;

        while (Time.time < startTime + timeToGetThere)
        {
            mainCam.transform.localPosition = new Vector3(mainCam.transform.localPosition.x, Mathf.SmoothStep(startPos.y, yToLerpTo, (Time.time - startTime) / timeToGetThere), mainCam.transform.localPosition.z);
            yield return new WaitForSeconds(0f);
        }

        /*bool multipliedSpeed = false;

		// Move faster back toward origin
		if(yToLerpTo == cameraOrigY || (yToLerpTo == cameraTopY && pos.y < cameraOrigY) || (yToLerpTo == cameraBottomY && pos.y > cameraOrigY))
		{
			cameraLerpSpeed *= 1.5f;
			multipliedSpeed = true;
		}

		// If already there, do nothing
		if(pos.y == yToLerpTo)
		{
			return;
		}

		//Otherwise, move smoothly toward target
		else if(pos.y < yToLerpTo)
		{
			float t = (yToLerpTo - cameraOrigY) / (pos.y - cameraOrigY);

			pos.y = Mathf.SmoothStep(cameraOrigY, yToLerpTo, t);
			/*if(Mathf.Abs(pos.y - yToLerpTo) >= cameraLerpSpeed)
			{
				pos.y += cameraLerpSpeed;
			}
			else
			{
				pos.y = yToLerpTo;
			}*/
        /*}
		else if(pos.y > yToLerpTo)
		{
			float t = (cameraOrigY - yToLerpTo) / (cameraOrigY - pos.y);

			pos.y = Mathf.SmoothStep(cameraOrigY, yToLerpTo, t);
		}
			
		if(multipliedSpeed)
		{
			cameraLerpSpeed /= 1.5f;
		}

		// Actually move the camera to the calculated position
		mainCam.transform.localPosition = pos;*/
    }

    /*IEnumerator LerpCameraX(bool isMovingRight, float timeToGetThere)
    {
        float startTime = Time.time;
        Vector3 startPos = mainCam.transform.localPosition;
        cameraMovingRight = isMovingRight;
        if (isMovingRight)
        {
            while (Time.time < startTime + timeToGetThere)
            {
                mainCam.transform.localPosition = new Vector3(Mathf.SmoothStep(startPos.x, cameraRightX, (Time.time - startTime) / timeToGetThere), mainCam.transform.localPosition.y, mainCam.transform.localPosition.z);
                yield return new WaitForSeconds(0f);
            }
        }
        else
        {
            {
                while (Time.time < startTime + timeToGetThere)
                {
                    mainCam.transform.localPosition = new Vector3(Mathf.SmoothStep(startPos.x, cameraLeftX, (Time.time - startTime) / timeToGetThere), mainCam.transform.localPosition.y, mainCam.transform.localPosition.z);
                    yield return new WaitForSeconds(0f);
                }
            }
        }
    }*/

	IEnumerator SlideCamera(bool right)
	{
		/*camSlidLeft = !right;
		camSlidRight = right;

		if(right)
		{
			while(transform.position.x > (mainCam.GetComponentInChildren<MeshCollider>().bounds.min.x + 1f))
			{
				if(isMoving)
				{
					mainCam.transform.position = new Vector3(mainCam.transform.position.x + 0.2f, mainCam.transform.position.y, mainCam.transform.position.z);
				}

				yield return new WaitForSeconds(0f);
			}
		}
		else
		{
			while(transform.position.x < (mainCam.GetComponentInChildren<MeshCollider>().bounds.max.x - 1f))
			{
				if(isMoving)
				{
					mainCam.transform.position = new Vector3(mainCam.transform.position.x - 0.2f, mainCam.transform.position.y, mainCam.transform.position.z);
				}

				yield return new WaitForSeconds(0f);
			}
		}*/
		return null;
	}
    #endregion

	IEnumerator FadeSound(AudioSource sound, float targetVolume, bool jumpTarget)
    {
		if (jumpTarget) 
		{
			sound.volume = targetVolume;
			yield return null;
		}
        while (sound.volume != targetVolume)
        {
            if (Mathf.Abs(sound.volume - targetVolume) < 0.01f)
            {
                sound.volume = targetVolume;
                break;
            }

            if (targetVolume > sound.volume)
            {
                sound.volume += 0.02f;
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                sound.volume -= 0.02f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    IEnumerator ResetJustBounced()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0f);
        }

        justBounced = false;
    }

	public bool CheckWallRight()
	{
		Vector2 origin, direction;
		float distance;
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		origin = new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.center.y - spriteRenderer.bounds.extents.y/2);//new Vector2(spriteRenderer.bounds.center.x, raycastPoint.position.y);
		direction = new Vector2(1, 0);
		direction.Normalize();
		distance = spriteRenderer.bounds.extents.x;
		RaycastHit2D raycastHit = Physics2D.Raycast(origin, direction, distance, 1 << LayerMask.NameToLayer("Default"));

		if(raycastHit.collider != null)
		{
			if(!raycastHit.collider.isTrigger)
			{
				return true;
			}
		}

		return false;
	}

    public void FreezeInput()
    {
        inputEnabled = false;
        rigidBody.velocity = Vector2.zero;
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsJumping", false);
    }

    public void UnfreezeInput()
    {
        inputEnabled = true;
        //rigidBody.velocity = Vector2.zero;
        //anim.SetBool("IsRunning", false);
    }
}
#endregion