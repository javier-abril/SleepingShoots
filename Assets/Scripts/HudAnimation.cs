using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudAnimation : MonoBehaviour {

    public AudioSource timeoutAudioSource;
    AudioSource winAudioHud;
    public PlayManager playManager;
    public GameObject ImageSavingRecords;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        winAudioHud = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update () {
		
	}


    //Called at the end off animation screen die player on HUD
    void RestartLevel()
    {   
        //Restarts if any player dies
        if (PlayersLives.lives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            //this bool is used on Update() from play manager
            PlayManager.stopTimer = false;
        }

    }

    //this is raised at the end of win animation to go to the next level
    //it call nextlevel from playManager script
    void NextLevel()
    {
        playManager.NextLevel();
    }


    //This is raised from gameover anim on HUD
    void GameOverAudio()
    {
        playManager.GetComponent<AudioSource>().Play();
    }

    
    //This is called from win anim on HUD
    void WinAudio()
    {
        winAudioHud.Play();
    }

    //This is called from timeout anim on HUD
    void TimeoutAudio()
    {
        timeoutAudioSource.Play();
    }

    //This is called from gameover anim
    public void LoadMainMenu()
    {       

        InsertRecordsAndLoadMain();
        
    }

    async private void InsertRecordsAndLoadMain()
    {
        WebServiceAPI ws = new WebServiceAPI();

        ImageSavingRecords.SetActive(true);

        try
        {

            await ws.InsertRecord(StaticManager.p1);
            await ws.InsertRecord(StaticManager.p2);

        }
        catch(Exception ex) { }

        //Load scene after await
        SceneManager.LoadScene("MainMenu");

    }
}
