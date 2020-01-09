using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayManager : MonoBehaviour {

    public static bool stopTimer;

    public GameObject pauseMenu;
    public float timeRemaining = 200f;
    public Text textTimer;
    public Text textEnemiesRemain;
    public GameObject[] enemies;
    public GameObject[] players;
    public GameObject hud;
    public Text player1Name;
    public Text player2Name;
    public GameObject ImageSavingRecords;

    Animator anim;
    int enemiesRemaining;
    bool win = false;
    private float timestamp = 0F;


    // Use this for initialization
    void Start () {

        //Set colors to players from staticmanager
        Renderer[] rend = players[0].GetComponentsInChildren<Renderer>();
        rend[3].material.SetColor("_Color", StaticManager.p1.PlayerColor);

        Renderer[] rend2 = players[1].GetComponentsInChildren<Renderer>();
        rend2[3].material.SetColor("_Color", StaticManager.p2.PlayerColor);

        //Set names
        player1Name.text = StaticManager.p1.Name;
        player2Name.text = StaticManager.p2.Name;

    }

    private void Awake()
    {
        anim = hud.GetComponent<Animator>();


        /* This was used before making new scenes

        NavMeshAgent nav;
        EnemyAttack2P enemyAtack;
        
        //Increase speed of each enemy to increase difficulty
        foreach (GameObject enemy in enemies)
        {
            //This is because an enemy became null after died
            if (enemy != null)
            {

                enemyAtack = enemy.GetComponent<EnemyAttack2P>();
                nav = enemy.GetComponent<NavMeshAgent>();

                //Increase attack damage and nav speed on each level
                enemyAtack.attackDamage += LevelManager.levelNumber;
                
                nav.speed = LevelManager.levelNumber;

            }
        }
        */

    }

    // Update is called once per frame
    void Update () {

        if (!stopTimer)
        {
            timeRemaining -= Time.deltaTime;
            textTimer.text = "TIME REMAINING: " + ((int)timeRemaining).ToString();


            //When enemies death they become null, so we have to check how many aren't null
            enemiesRemaining = 0;

            foreach (GameObject eH in enemies)
            {
                if (eH != null)
                    enemiesRemaining += 1;
            }

            //Update UI text
            textEnemiesRemain.text = "ENEMIES: " + enemiesRemaining;

            if (timeRemaining < 0 && PlayersLives.lives > 1 && enemiesRemaining > 0)
            {
                Timeout();
            }
            else if (timeRemaining < 0 && PlayersLives.lives == 1 && enemiesRemaining > 0)
            {
                GameOver();
            }
            else if (enemiesRemaining == 0 && timeRemaining > 0 && PlayersLives.lives >= 1)
            {
                Win();
            }
        }

        if ((Input.GetButton("PauseP1") || Input.GetButton("PauseP2")) && Time.time >= timestamp)
        {
            if (pauseMenu.activeSelf)
            {
                
                DisablePause();
            }
            else
            {

                EnablePause();
            }
            timestamp = Time.time + 0.5f;
        }

    }


    public void StopALL()
    {
        //This is for do nothing on Update()
        stopTimer = true;

        StopBGMusic();     

        StopAllEnemies();

        StopPlayers();
    }

    public void StopAllDiePlayers()
    {
        //This is for do nothing on Update()
        stopTimer = true;

        StopBGMusic();

        StopAllEnemies();

        StopPlayersAndDie();
    }

    public void StopBGMusic()
    {
        //I have added tag for gameObject of BGmusic to stop background music
        GameObject bgMusic = GameObject.FindGameObjectWithTag("BGmusic");
        AudioSource audioMusic = bgMusic.GetComponent<AudioSource>();
        audioMusic.Stop();
    }

    public void StopAllEnemies()
    {
        Animator enemyAnim;
        EnemyAttack2P enemyAttack;
        EnemyMovement enemyMovement;


        stopTimer = true;

        foreach (GameObject enemy in enemies)
        {
            //This is because an enemy became null after died
            if (enemy != null)
            {
                enemyAttack = enemy.GetComponent<EnemyAttack2P>();
                enemyMovement = enemy.GetComponent<EnemyMovement>();
                enemyAnim = enemy.GetComponent<Animator>();

                //When launch this trigger monster go idle anim
                enemyAnim.SetTrigger("playerDead");
                enemyMovement.enabled = false;
                enemyAttack.enabled = false;
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            }
        }
 
    }


    public void EnablePause()
    {
        //This is for do nothing on Update()
        stopTimer = true;

        StopAllEnemies();

        StopPlayers();

        pauseMenu.SetActive(true);
    }

    public void DisablePause()
    {
        stopTimer = false;

        UnPauseAllEnemies();

        UnPausePlayers();

        pauseMenu.SetActive(false);
    }


    public void UnPauseAllEnemies()
    {
        Animator enemyAnim;

        stopTimer = false;

        foreach (GameObject enemy in enemies)
        {
            //This is because an enemy became null after died
            if (enemy != null)
            {
                enemyAnim = enemy.GetComponent<Animator>();
                //When launch this trigger when monster is iddle to walk
                enemyAnim.SetTrigger("unPause");

            }
        }

        //Wait 2 seconds for start walking anim. It cannot be edited
        StartCoroutine(Wait3Secs());       

    }

    IEnumerator Wait3Secs()
    {
        yield return new WaitForSeconds(1f);

        EnemyAttack2P enemyAttack;
        EnemyMovement enemyMovement;

        foreach (GameObject enemy in enemies)
        {
            //This is because an enemy became null after died
            if (enemy != null)
            {
                enemyAttack = enemy.GetComponent<EnemyAttack2P>();
                enemyMovement = enemy.GetComponent<EnemyMovement>();

                enemyMovement.enabled = true;
                enemyAttack.enabled = true;
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
        }

    }

    public void UnPausePlayers()
    {
        foreach (GameObject player in players)
        {
            if (player != null)
            {

                //This is for multiplayer because scripts are different name
                if (player.name == "Player")
                {
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    PlayerShooting playerShooting = player.GetComponentInChildren<PlayerShooting>();
                    //this is to stop walking animation
                    playerMovement.AnimateWalk(0, 0);

                    playerMovement.enabled = true;
                    playerShooting.enabled = true;
                }
                else
                {
                    Player2Movement player2Movement = player.GetComponent<Player2Movement>();
                    Player2Shooting player2Shooting = player.GetComponentInChildren<Player2Shooting>();

                    //this is to stop walking animation
                    player2Movement.AnimateWalk(0, 0);

                    player2Movement.enabled = true;
                    player2Shooting.enabled = true;
                }

            }
        }
    }

    public void StopPlayers()
    {
        foreach (GameObject player in players)
        {
            if (player != null)
            {

                //This is for multiplayer because scripts are different name
                if (player.name == "Player")
                {
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    PlayerShooting playerShooting = player.GetComponentInChildren<PlayerShooting>();
                    //this is to stop walking animation
                    playerMovement.AnimateWalk(0, 0);

                    playerMovement.enabled = false;
                    playerShooting.enabled = false;
                }
                else
                {
                    Player2Movement player2Movement = player.GetComponent<Player2Movement>();
                    Player2Shooting player2Shooting = player.GetComponentInChildren<Player2Shooting>();

                    //this is to stop walking animation
                    player2Movement.AnimateWalk(0, 0);

                    player2Movement.enabled = false;
                    player2Shooting.enabled = false;
                }

            }
        }
    }

    public void StopPlayersAndDie()
    {
        Animator playerAnim;
        AudioSource playerAudio;



        foreach (GameObject player in players)
        {
            if (player != null)
            {
                playerAnim = player.GetComponent<Animator>();
                playerAudio = player.GetComponent<AudioSource>();

                //This is for multiplayer because scripts are different name
                if (player.name == "Player")
                {
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    PlayerShooting playerShooting = player.GetComponentInChildren<PlayerShooting>();

                    playerMovement.enabled = false;
                    playerShooting.enabled = false;
                }
                else
                {
                    Player2Movement player2Movement = player.GetComponent<Player2Movement>();
                    Player2Shooting player2Shooting = player.GetComponentInChildren<Player2Shooting>();

                    player2Movement.enabled = false;
                    player2Shooting.enabled = false;
                }



                if (!win)
                {
                    playerAnim.SetTrigger("Die");
                    if (player.name == "Player")
                        playerAudio.clip = player.GetComponent<PlayerHealth>().deathClip;
                    else
                        playerAudio.clip = player.GetComponent<Player2Health>().deathClip;

                    playerAudio.Play();
                }


            }
        }
    }

    public void StopAllVibrations()
    {
        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
    }

    //It will be called from animation event timeout
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        stopTimer = false;
    }

    void Timeout()
    {
        StopALL();
        
        //To avoid negative values
        textTimer.text = "TIME REMAINING: 0";
        //We rest 1 life for timeout
        PlayersLives.lives -= 1;

        anim.SetTrigger("Timeout");

    }

    public void GameOver()
    {
        StopAllDiePlayers();

        //To avoid negative values
        textTimer.text = "TIME REMAINING: 0";
        //We rest 1 life for timeout
        PlayersLives.lives -= 1;

        anim.SetTrigger("Gameover");

        //TO-DO Save score

    }

    void Win()
    {
        win = true;
        StopALL();
        anim.SetTrigger("Win");

    }


    //Called from Hud Anim script after win anim ends
    public void NextLevel()
    {
        
        
        if (LevelManager.levelNumber == 3)
        {
            InsertRecordsAndLoadMain();
        }

        stopTimer = false;
        /* OLD method leveling
        LevelManager.levelNumber += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        */

        if(SceneManager.GetActiveScene().name == "Level1")
        {
            LevelManager.levelNumber = 2;
            SceneManager.LoadScene("Level2");
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            LevelManager.levelNumber = 3;
            SceneManager.LoadScene("Level3");
        }
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
        catch (Exception ex) { }

        //Load scene after await
        SceneManager.LoadScene("MainMenu");

    }

    //This is for pause menu button
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
