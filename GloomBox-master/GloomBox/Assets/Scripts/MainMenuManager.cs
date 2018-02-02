#region Using Directives
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
#endregion

#region MainMenuManager Class
public class MainMenuManager : MonoBehaviour
{
	#region Private Variables
    //private int numberOfTapes = 3;
    Vector3[] tapeStartPositions = new Vector3[5];
    private float startTime;
    //Vector3 startArrow;
    //Vector3 endArrow;
   // bool canGoUp = true;
   // bool canGoDown = true;
	bool interactable = true;
    private int locator = 0;
    public Animator playAnim;

	[SerializeField]
	GameObject[] tapes;
	int[] distances = new int[5];

	GameObject playTape;
	GameObject controlsTape;
	GameObject configTape;
	GameObject creditsTape;
	GameObject quitTape;
	GameObject playButton;
    #endregion

    #region Public Variables
    public int distancePlayTape;
    public int distanceControlsTape;
    public int distanceConfigTape;
    public int distanceQuitTape;
    public int distancecreditsTape;
    public float minimum = 10.0f;
    public float maximum = 20.0f;
    public float duration = 0.5f;
    #endregion

    // Start is used for initialization
    void Start()
    {
		playAnim = GetComponent<Animator>();
        startTime = Time.time;

		playTape = tapes[0];
		controlsTape = tapes[1];
		quitTape = tapes[4];
		configTape = tapes[2];
		creditsTape = tapes[3];
		playButton = GameObject.Find("PlayButton");

        // Positions of tapes at the start
        // Adjust positions accordingly to tape locations in game
		tapeStartPositions[0] = playTape.transform.position;
		tapeStartPositions[1] = controlsTape.transform.position;
		tapeStartPositions[2] = configTape.transform.position;
        tapeStartPositions[3] = creditsTape.transform.position;
        tapeStartPositions[4] = quitTape.transform.position;

		distances[0] = distancePlayTape;
		distances[1] = distanceControlsTape;
		distances[2] = distanceConfigTape;
		distances[3] = distancecreditsTape;
		distances[4] = distanceQuitTape;

        //startArrow = tapeStartPositions[0];
        //endArrow = tapeStartPositions[tapeStartPositions.Length-1];

        playTape.transform.position = new Vector3(tapeStartPositions[0].x + distancePlayTape, tapeStartPositions[0].y, tapeStartPositions[0].z);

	}

    // Update is called once per frame
    void Update()
    {        
        HandleInput();
        
        /*if (canGoUp)
        {
            GameObject.Find("NoUpArrow").GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GameObject.Find("NoUpArrow").GetComponent<SpriteRenderer>().enabled = true;
        }

        if(canGoDown)
        {
            GameObject.Find("NoDownArrow").GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GameObject.Find("NoDownArrow").GetComponent<SpriteRenderer>().enabled = true;
        } */
    }

	#region Scene Loading and Unloading
    // Loads scene on button press
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
	#endregion

	#region Input Management
    public void HandleInput()
    {
		float t = (Time.time - startTime) / duration;

		switch(locator)
		{
			case 0:
				playTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[0].x, tapeStartPositions[0].x + distancePlayTape, t), tapeStartPositions[0].y, tapeStartPositions[0].z);
				break;
			case 1:
				controlsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[1].x, tapeStartPositions[1].x + distanceControlsTape, t), tapeStartPositions[1].y, tapeStartPositions[1].z);
				break;
			case 2:
				configTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[2].x, tapeStartPositions[2].x + distanceConfigTape, t), tapeStartPositions[2].y, tapeStartPositions[2].z);
				break;
			case 3:
				creditsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[3].x, tapeStartPositions[3].x + distancecreditsTape, t), tapeStartPositions[3].y, tapeStartPositions[3].z);
				break;
			case 4:
				quitTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[4].x, tapeStartPositions[4].x + distanceQuitTape, t), tapeStartPositions[4].y, tapeStartPositions[4].z); 
				break;
			default:
				break;
		}

		for(int i = 0; i < 5; i++)
		{
			if(i != locator)
			{
				if(tapes[i].transform.position != tapeStartPositions[i])
				{
					tapes[i].transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[i].x  + distances[i], tapeStartPositions[i].x, t), tapeStartPositions[i].y, tapeStartPositions[i].z);
				}
			}
		}

        if(interactable)
		{
			// Scroll up
			if(Input.GetAxis("Vertical") > 0.2f)
			{
                startTime = Time.time;
                StartCoroutine(DelayInput());
                int selectableTape = GetLocator();
                selectableTape = selectableTape - 1;
				AudioSource tapeSelect = GetComponents<AudioSource>()[1];
				tapeSelect.Play();

				if(selectableTape < 0)
				{
					selectableTape = 4;
				}

				SetLocator(selectableTape);

                /*switch(selectableTape)
                {
                    case 0:
						tapeSelect.Play();
						//playTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[0].x, tapeStartPositions[0].x + distancePlayTape, t), tapeStartPositions[0].y, tapeStartPositions[0].z);
						/*playTape.transform.position = tapeStartPositions[0];
						controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        /*selectableTape = 0;
                        break;
                    case 1:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
						controlsTape.transform.position = tapeStartPositions[1];
                        //controlsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[1].x, tapeStartPositions[1].x + distanceControlsTape, t), tapeStartPositions[1].y, tapeStartPositions[1].z);
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        /*selectableTape = 1;
                        break;
                    case 2:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
						configTape.transform.position = tapeStartPositions[2];
                        //configTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[2].x, tapeStartPositions[2].x + distanceConfigTape, t), tapeStartPositions[2].y, tapeStartPositions[2].z);
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        /*selectableTape = 2;
                        break;
                    case 3:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        //creditsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[3].x, tapeStartPositions[3].x + distancecreditsTape, t), tapeStartPositions[3].y, tapeStartPositions[3].z);
						creditsTape.transform.position = tapeStartPositions[3];
						quitTape.transform.position = tapeStartPositions[4];*/
                        /*selectableTape = 3;
                        break;
                    case 4:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];
                        //quitTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[4].x, tapeStartPositions[4].x + distanceQuitTape, t), tapeStartPositions[4].y, tapeStartPositions[4].z); 
						quitTape.transform.position = tapeStartPositions[4];*/
						/*selectableTape = 4;
                        break;
                    default:
						/*tapeSelect.Play();
						playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[3].x, tapeStartPositions[3].x + distanceQuitTape, t), tapeStartPositions[3].y, tapeStartPositions[3].z);
                        quitTape.transform.position = tapeStartPositions[4];
                        selectableTape = 3;*/
                        /*break;
                }*/
				
                /*
                if (GameObject.Find("PlayTape").transform.position != startArrow)
                {
                    StartCoroutine(DelayInput());

                    Vector3 temp = tapeStartPositions[3];

                    for (int i = (numberOfTapes - 1); i >= 0; i--)
                    {
                        tapeStartPositions[i + 1] = tapeStartPositions[i];
                    }

                    tapeStartPositions[0] = temp;

                    GameObject.Find("PlayTape").transform.position = tapeStartPositions[0];
					GameObject.Find("ControlsTape").transform.position = tapeStartPositions[3];
                    GameObject.Find("ConfigTape").transform.position = tapeStartPositions[2];
                    GameObject.Find("QuitTape").transform.position = tapeStartPositions[1];
                    TurnOnArrows(); 
                } */

            }

           /* if (Vector3.Distance(GameObject.Find("PlayTape").transform.position, endArrow) == 0)
            {
                canGoUp = true;
                canGoDown = false;
            }*/

            // Scroll down
            if(Input.GetAxis("Vertical") < -0.2f)
			{
                startTime = Time.time;
                StartCoroutine(DelayInput());
                int selectableTape2 = GetLocator();

                selectableTape2 = (selectableTape2 + 1) % 5;
				AudioSource tapeSelect = GetComponents<AudioSource>()[1];

                switch(selectableTape2)
                {
                    case 0:
						tapeSelect.Play();
						//playTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[0].x, tapeStartPositions[0].x + distancePlayTape, t), tapeStartPositions[0].y, tapeStartPositions[0].z);
                        /*controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        selectableTape2 = 0;
                        break;
                    case 1:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        //controlsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[1].x, tapeStartPositions[1].x + distanceControlsTape, t), tapeStartPositions[1].y, tapeStartPositions[1].z);
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        selectableTape2 = 1;
                        break;
                    case 2:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
                        //configTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[2].x, tapeStartPositions[2].x + distanceConfigTape, t), tapeStartPositions[2].y, tapeStartPositions[2].z);
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        selectableTape2 = 2;
                        break;
                    case 3:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        //creditsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[3].x, tapeStartPositions[3].x + distancecreditsTape, t), tapeStartPositions[3].y, tapeStartPositions[3].z);
                        quitTape.transform.position = tapeStartPositions[4];*/
                        selectableTape2 = 3;
                        break;
                    case 4:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = tapeStartPositions[1];
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];*/
                        //quitTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[4].x, tapeStartPositions[4].x + distanceQuitTape, t), tapeStartPositions[4].y, tapeStartPositions[4].z); 
                        selectableTape2 = 4;
                        break;
                    default:
						tapeSelect.Play();
						/*playTape.transform.position = tapeStartPositions[0];
                        controlsTape.transform.position = new Vector3(Mathf.SmoothStep(tapeStartPositions[1].x, tapeStartPositions[1].x + distanceControlsTape, t), tapeStartPositions[1].y, tapeStartPositions[1].z);
                        configTape.transform.position = tapeStartPositions[2];
                        creditsTape.transform.position = tapeStartPositions[3];
                        quitTape.transform.position = tapeStartPositions[4];*/
                        selectableTape2 = 1;
                        break;
                }

                SetLocator(selectableTape2);
                /*if (GameObject.Find("PlayTape").transform.position != endArrow)
                {
                    StartCoroutine(DelayInput());

                    Vector3 temp = tapeStartPositions[0];

                    for (int i = 1; i <= numberOfTapes; i++)
                    {
                        tapeStartPositions[i - 1] = tapeStartPositions[i];
                    }

                    tapeStartPositions[3] = temp;

                    GameObject.Find("PlayTape").transform.position = tapeStartPositions[0];
					GameObject.Find("ControlsTape").transform.position = tapeStartPositions[3];
                    GameObject.Find("ConfigTape").transform.position = tapeStartPositions[2];
                    GameObject.Find("QuitTape").transform.position = tapeStartPositions[1];
                    TurnOnArrows();
                } */
            }

          /*  if (GameObject.Find("PlayTape").transform.position == startArrow)
            {
                canGoDown = true;
                canGoUp = false;
            }*/
        }

        // Start level when player presses submit
        if(Input.GetAxis("Submit") > 0.2f)
        {
            if(controlsTape.transform.position.x == 235)
            {
                GameObject.Find("FakeControls").GetComponent<SpriteRenderer>().enabled = true;
                controlsTape.GetComponent<SpriteRenderer>().enabled = false;
                playAnim.SetBool("canControl", true);
                StartCoroutine(ControlsAnimation());
            }
            else if(quitTape.transform.position.x == 235)
            {
                GameObject.Find("FakeQuit").GetComponent<SpriteRenderer>().enabled = true;
                quitTape.GetComponent<SpriteRenderer>().enabled = false;
                playAnim.SetBool("canQuit", true);
                StartCoroutine(QuitAnimation());
            }
			else if(playTape.transform.position.x == 235)
            {
                GameObject.Find("FakePlay").GetComponent<SpriteRenderer>().enabled = true;
				playTape.GetComponent<SpriteRenderer>().enabled = false;
                playAnim.SetBool("canPlay", true);
                StartCoroutine(PlayAnimation());
                
            }
			else if(configTape.transform.position.x == 235)
			{
                GameObject.Find("FakeConfig").GetComponent<SpriteRenderer>().enabled = true;
                configTape.GetComponent<SpriteRenderer>().enabled = false;
                playAnim.SetBool("canConfig", true);
                StartCoroutine(ConfigAnimation());
			}
            else if(creditsTape.transform.position.x == 235)
            {
                GameObject.Find("FakeCredits").GetComponent<SpriteRenderer>().enabled = true;
                creditsTape.GetComponent<SpriteRenderer>().enabled = false;
                playAnim.SetBool("canCredit", true);
                StartCoroutine(CreditsAnimation());
            }
        }
    }

	IEnumerator DelayInput()
	{
		interactable = false;
		yield return new WaitForSeconds(0.3f);
		interactable = true;
	}

    IEnumerator PlayAnimation()
    {
        interactable = false;
        playAnim.Play("Play5");
		AudioSource boxClose = GetComponents<AudioSource>()[2];
		boxClose.PlayDelayed(1.6f);
        yield return new WaitForSeconds(2.3f);

        playButton.GetComponent<SpriteRenderer>().enabled = false;
		AudioSource buttonPress = GetComponents<AudioSource>()[0];
		buttonPress.Play ();
        //yield return new WaitForSeconds(0.1f);
        LevelManager.ClearData();
        LoadScene("OpeningCutscene");
    }

    IEnumerator CreditsAnimation()
    {
        interactable = false;
        playAnim.Play("CreditsAnimation");
		AudioSource boxClose = GetComponents<AudioSource>()[2];
		boxClose.PlayDelayed(3.5f);
        yield return new WaitForSeconds(4.2f);

        playButton.GetComponent<SpriteRenderer>().enabled = false;
		AudioSource buttonPress = GetComponents<AudioSource>()[0];
		buttonPress.Play ();
		LoadScene("Credits");
    }

    IEnumerator ControlsAnimation()
    {
        interactable = false;
        playAnim.Play("ControlsAnimation2");
		AudioSource boxClose = GetComponents<AudioSource>()[2];
		boxClose.PlayDelayed(2.1f);
        yield return new WaitForSeconds(3.0f);

        playButton.GetComponent<SpriteRenderer>().enabled = false;
		AudioSource buttonPress = GetComponents<AudioSource>()[0];
		buttonPress.Play ();
		LoadScene("Controls");
    }

    IEnumerator ConfigAnimation()
    {
        interactable = false;
        playAnim.Play("ConfigAnimation");
		AudioSource boxClose = GetComponents<AudioSource>()[2];
		boxClose.PlayDelayed(1.7f);
        yield return new WaitForSeconds(3.2f);

        playButton.GetComponent<SpriteRenderer>().enabled = false;
		AudioSource buttonPress = GetComponents<AudioSource>()[0];
		buttonPress.Play ();
		LoadScene("RemapKeys");
    }

    IEnumerator QuitAnimation()
    {
        interactable = false;
        playAnim.Play("Quit");
		AudioSource boxClose = GetComponents<AudioSource>()[2];
		boxClose.PlayDelayed(2.8f);
        yield return new WaitForSeconds(2f);
		GameObject.Find("FakeQuit").GetComponent<SpriteRenderer>().sortingOrder = 0;
		yield return new WaitForSeconds(1.5f);

        playButton.GetComponent<SpriteRenderer>().enabled = false;
		AudioSource buttonPress = GetComponents<AudioSource>()[0];
		buttonPress.Play();
		//yield return new WaitForSeconds(0.5f);
		QuitGame();
    }

   /* public void TurnOnArrows()
    {
        canGoUp = true;
        canGoDown = true;
    }*/

    public int GetLocator()
    {
        return locator;
    }

    public void SetLocator(int setter)
    {
        locator = setter;
    }
    // Closes game
    // Only affects builds
    public void QuitGame()
    {
        Application.Quit();
    }
	#endregion
}
#endregion