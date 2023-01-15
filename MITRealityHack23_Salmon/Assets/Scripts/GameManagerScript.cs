using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private bool audio11HasPlayed = false;
    private bool audio12HasPlayed = false;
    private bool audio22HasPlayed = false;


    #region GameObjects
    public int myGameState = 0;
    Leap.Unity.Interaction.InteractionBehaviour myBehaviorScript;
    public GameObject myCamera;
    public GameObject myBuilding;
    public GameObject myWave;
    public GameObject myGoose;
    public GameObject myBook;
    public GameObject waveBook;
    public GameObject waveOut;
    public bool calledZoom = false;
    #endregion

    #region Audio
    public AudioSource aud;
    public AudioClip audio0;
    public AudioClip audio11;
    public AudioClip audio12;
    public AudioClip audio22;
    #endregion

    #region GameManager
    private static GameManagerScript instance;

    public static GameManagerScript GetInstance()
    {
        return instance;
    }
    #endregion

    public void Awake()
    {
        myCamera = GameObject.FindWithTag("MainCamera");
        myBuilding = GameObject.Find("tianyige");
        myWave = GameObject.Find("Wave");
        myGoose = GameObject.Find("goose");
        myBook = GameObject.Find("bookshelf");
        waveBook = GameObject.Find("WaveBook");
        waveOut = GameObject.Find("WaveOut");
    
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //Timer
    public float myTimer = 0.0f;

    //Scene2
    public void ZoomInName()
    {
        myBuilding.transform.localScale = Vector3.Lerp(myBuilding.transform.localScale, new Vector3(22, 22, 22), Time.deltaTime * 0.9f);
        Debug.Log("zoom in");
        calledZoom = true;
    }

    //To Scene3
    public void GrabStart()
    {
        Debug.Log("GrabtoEnd");
        InvokeRepeating("GrabtoEnd", 0, 0.1f);
    }
    public void GrabtoEnd()
    {
        //Debug.Log("GrabtoEnd");
        GameObject.Find("FadeBlack").GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), Time.time * 0.2f));

    }

    public void ZoomToBook()
    {
        //change the camera clip to 0.01
        myCamera.GetComponent<LookingGlass.Holoplay>().CameraData.NearClipFactor = 0.01f;
    }

    public void ZoomOut()
    {
        myCamera.GetComponent<LookingGlass.Holoplay>().CameraData.NearClipFactor = 0.75f;
    }
    // Start is called before the first frame update
    void Start()
    {
        myBook.SetActive(false);
        waveBook.SetActive(false);
        waveOut.SetActive(false);
        aud = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        aud.Play();
    }

    // Update is called once per frame
    void Update()
    {
       
        Debug.Log(myGoose.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreGrasping);

        myTimer += Time.deltaTime;
        int myTimerSec = (int)myTimer % 60;

        //if (Input.GetMouseButtonDown(0))
        //{
            //GrabStart();
        //}

        //Debug.Log(myBuilding.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreGrasping);
        Debug.Log(myGameState);
        //Debug.Log(myBuilding.transform.localScale.x);


        //Start scene2
        //State = 0: white building, playing audio 0

        //Audio 0 finished, enable hover, user puts the hand to hover to rain
        if (myGameState == 0 && !aud.isPlaying)
        {
            //Debug.Log("enable rain");
            myBuilding.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreHoverMode = Leap.Unity.Interaction.IgnoreHoverMode.None;
            myBuilding.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignorePrimaryHover = false;
        }

        //color changed,State = 1
        if (myGameState == 0 && myBuilding.GetComponent<Renderer>().material.GetColor("_EmissionColor")==Color.black)
        {
            myGameState = 1;
        }

        //If state=1
        if(myGameState == 1 && !aud.isPlaying && !audio11HasPlayed)
        {
            //Play audio 1.1
            aud.clip = audio11;
            aud.Play();
            Debug.Log("aud11");
            audio11HasPlayed = true;
        }

        //audio 1.1 finished,  allow zoom in to banner
        if (myGameState == 1 && !aud.isPlaying)
        {
            myGameState = 2;
        }

        if(myGameState == 2)
        {
            myWave.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreContact = false;
        }

        //State = 2: user zooms in to banner, state = 3
        if (myGameState ==2 && myBuilding.transform.localScale.x>=21)
        {
            myGameState = 3;
        }

        //State=3: play audio 1.2
        if(myGameState == 3 && !aud.isPlaying && !audio12HasPlayed)
        {
            //play audio1.2
            aud.clip = audio12;
            aud.Play();
            Debug.Log("aud12");
            audio12HasPlayed = true;
        }

        //Audio 1.2 finished, state=4
        if (myGameState == 3 && !aud.isPlaying)
        {
            myGameState = 4;
        }

        //State = 4: users zoom in to bookshelf, state = 5
        if (myGameState == 4) 
        {
            myBook.SetActive(true);
            waveBook.SetActive(true);
            //change the camera clip to 0.01 by calling the function

        }

        if (myGameState == 4 && myCamera.GetComponent<LookingGlass.Holoplay>().CameraData.NearClipFactor == 0.01f)//%% camera clip check =0.01
        {
            myGameState = 5;
            waveBook.SetActive(false);
        }

        if (myGameState == 5 && !aud.isPlaying && !audio22HasPlayed)
        {
            //play audio 2.2
            aud.clip = audio22;
            aud.Play();
            Debug.Log("aud22");
            audio22HasPlayed = true;
        }

        //if audio 2.2 finished, state=6
        if (myGameState == 5 && !aud.isPlaying)
        {
            myGameState = 6;
            waveOut.SetActive(true);
        }
  
        //State=6: zoom out to building, state =7
        if (myGameState == 6)
        {
            // change camera to near clipping 0.75
            myCamera.GetComponent<LookingGlass.Holoplay>().CameraData.NearClipFactor = 0.75f;
        }

        if (myGameState == 6 && myCamera.GetComponent<LookingGlass.Holoplay>().CameraData.NearClipFactor == 0.75f)//&&camera check 0.75
        {
            myGameState = 7;
            waveOut.SetActive(false);
        }
        //In state 7. now you already zoom out to entire building, so we grab goose.
        if (myGameState == 7)
        {
            myGoose.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreGrasping = false;
            myGoose.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreContact = false;

            myBuilding.GetComponent<Leap.Unity.Interaction.InteractionBehaviour>().ignoreContact = true;
        }
        if (myGameState ==7 && GameObject.Find("FadeBlack").GetComponent<Renderer>().material.GetColor("_Color") == Color.black)
        {
            myGameState = 8;
        }

        //Transition to end scene when the scene is dark
        if(myGameState == 8)
        {
            SceneManager.LoadScene("ThirdScene");
        }
    }

}
