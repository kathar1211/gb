using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueTrigger : MonoBehaviour {

    static GameObject dText;
    static GameObject dialogueObject;
    static GameObject dialogueBox;
    GameObject Muse;
    TextMeshPro dTextTM;
    [SerializeField]
    bool topAligned;
    [SerializeField]
    bool isReactivatable;
    [SerializeField]
    string[] scriptLines;
    [SerializeField]
    float CharacterDelay;
    [SerializeField]
    float LineDelay;
    bool dialogueIsActive;
    bool playedOnce;
    bool skipButtonPressed;
    static GameObject musePortrait;
    static GameObject gloomPortrait;
    Vector3 botPosition;
    Vector3 topPosition;
    Coroutine pdCoroutine;

	[SerializeField]
	Sprite[] gloomPortraits;
	[SerializeField]
	Sprite[] musePortraits;

	// Use this for initialization
	void Start ()
    {
        dialogueObject = GameObject.Find("DialogueObject");
        dText = GameObject.Find("DText");
        dialogueBox = GameObject.Find("DialogueBox");
        dTextTM = dText.GetComponent<TextMeshPro>();
        musePortrait = GameObject.Find("MusePortrait");
        gloomPortrait = GameObject.Find("GloomPortrait");
        Muse = GameObject.FindGameObjectWithTag("Player");
        Invoke("DisableDialogueSubobjects", 0f);
        //hardcoded top and bottom positions
        botPosition = new Vector3(0, -5f, 0);
        topPosition = new Vector3(0, 3f, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(KeyboardState.IsButtonJustPressed(InputManager.stringToControllerButton[InputManager.cJump]) || (KeyboardState.IsKeyJustPressed(InputManager.stringToKey[InputManager.kPlay]))) 
        {
            skipButtonPressed = true;
        }
        if(dialogueIsActive && (KeyboardState.IsButtonJustPressed(InputManager.stringToControllerButton["start"]) || (KeyboardState.IsKeyJustPressed(InputManager.stringToKey["enter"]))))
        {
            //print("trying to exit dialogue");
            ExitDialogue();
        }
    }

    void DisableDialogueSubobjects()
    {
        dText.gameObject.SetActive(false);
        dialogueBox.SetActive(false);
        musePortrait.SetActive(false);
        gloomPortrait.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //set flag to true
            dialogueIsActive = true;
            //begin coroutine to go through lines
            pdCoroutine = StartCoroutine(ProgressDialogue());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //set flags to false
            if(!isReactivatable)
            {
                playedOnce = true;
            }            
            dialogueIsActive = false;
            dText.SetActive(false);
            dialogueBox.SetActive(false);
            musePortrait.SetActive(false);
            gloomPortrait.SetActive(false);
            Muse.GetComponent<PlayerController>().UnfreezeInput();
            //end coroutine
            if (pdCoroutine!=null)
            {
                StopCoroutine(pdCoroutine);
            }
        }
    }

    IEnumerator ProgressDialogue()
    {
        if(dialogueIsActive && !playedOnce)
        {
            //freeze input
            Muse.GetComponent<PlayerController>().FreezeInput();
            //Position and activate text
            if(topAligned)
            {
                dialogueObject.transform.localPosition = topPosition;
            }
            else
            {
                dialogueObject.transform.localPosition = botPosition;
            }
            dText.SetActive(true);
            dialogueBox.SetActive(true);
            //Start printing characters
            for (int i=0;i<scriptLines.Length;i++)
            {
				//assign entire line
				//dTextTM.text = scriptLines[i].Substring(2);
                if(scriptLines[i][0]=='M')
                {
                    //customize text for Muse
                    //dTextTM.font = Resources.Load("GloriaHallelujah SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
                    dTextTM.color = new Color32(140, 1, 3, 255);

					switch(scriptLines[i][1])
					{
						case 'd':
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[0];
							break;
						case 'e':
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[1];
							break;
						case 'h':
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[2];
							break;
						case 'm':
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[3];
							break;
						case 'p':
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[4];
							break;
						case 's':
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[5];
							break;
						default:
							musePortrait.GetComponent<SpriteRenderer>().sprite = musePortraits[0];
							break;
					}

                    musePortrait.SetActive(true);
                }
                else if(scriptLines[i][0]=='G')
                {
                    //customize text for Gloom
                    //dTextTM.font = Resources.Load("Orbitron-Bold SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
                    dTextTM.color = new Color32(57, 84, 113, 255);

					switch(scriptLines[i][1])
					{
						case 'd':
							gloomPortrait.GetComponent<SpriteRenderer>().sprite = gloomPortraits[0];
							break;
						case 'i':
							gloomPortrait.GetComponent<SpriteRenderer>().sprite = gloomPortraits[1];
							break;
						case 'm':
							gloomPortrait.GetComponent<SpriteRenderer>().sprite = gloomPortraits[2];
							break;
						case 's':
							gloomPortrait.GetComponent<SpriteRenderer>().sprite = gloomPortraits[3];
							break;
						case 't':
							gloomPortrait.GetComponent<SpriteRenderer>().sprite = gloomPortraits[4];
							break;
						default:
							gloomPortrait.GetComponent<SpriteRenderer>().sprite = gloomPortraits[0];
							break;
					}

                    gloomPortrait.SetActive(true);
                }
				//reveal one by one
				//dTextTM.
                for(int j=3;j<scriptLines[i].Length;j++)
                {
                    dTextTM.text = scriptLines[i].Substring(3, j-2);
					if(scriptLines[i][j] == '<')
					{
						do
						{
							j++;
							dTextTM.text = scriptLines[i].Substring(3, j-2);
						} while(scriptLines[i][j] != '>');
					}
                    yield return null;
                    if(skipButtonPressed)
                    {
                        j = scriptLines[i].Length - 2;
                        skipButtonPressed = false;
                        continue;
                    }
                    yield return new WaitForSeconds(CharacterDelay);
                }
                while(!skipButtonPressed)
                {
                    yield return null;
                }
                //yield return new WaitForSeconds(LineDelay);
                skipButtonPressed = false;
                musePortrait.SetActive(false);
                gloomPortrait.SetActive(false);
            }
            if(!isReactivatable)
            {
                playedOnce = true;
            }
            musePortrait.SetActive(false);
            gloomPortrait.SetActive(false);
            dialogueIsActive = false;
            dText.SetActive(false);
            dialogueBox.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            Muse.GetComponent<PlayerController>().UnfreezeInput();
        }
    }
    
    void ExitDialogue()
    {
        if (pdCoroutine != null)
        {
            StopCoroutine(pdCoroutine);
        }
        if (!isReactivatable)
        {
            playedOnce = true;
        }
        dialogueIsActive = false;
        dText.SetActive(false);
        dialogueBox.SetActive(false);
        musePortrait.SetActive(false);
        gloomPortrait.SetActive(false);
        Muse.GetComponent<PlayerController>().UnfreezeInput();
    } 
}
