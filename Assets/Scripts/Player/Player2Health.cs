using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Health : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;

    //For launching gameover
    public PlayManager playManager;

    //UI Elements on HUD canvas
    public Slider healthSlider;
    public Text lifeText;
    public Image damageImage;
    //For flashing when receive a damage
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    //Player death clip sound
    public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;

    //This are the only variable needed to work with 2Players
    Player2Movement playerMovement;
    Player2Shooting playerShooting;

    //Is needed to check if its dead on TakeDamage()
    //The value is assigned on Death()
    bool isDead;
    //is needed to flash screen on red when receive a damage
    //is assingned on TakeDamage()
    bool damaged;


    public GameObject hud;
    Animator hudAnim;

    void Awake()
    {

        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        //References the script movement and shooting
        playerMovement = GetComponent<Player2Movement>();
        playerShooting = GetComponentInChildren<Player2Shooting>();

        //Set current health to starting (100 by default)
        currentHealth = startingHealth;

        //Gets the animator's hud for die screen animation
        hudAnim = hud.GetComponent<Animator>();
    }


    void Update()
    {
        //If player receives a damage
        if (damaged)
        {
            //Change color to damageImage
            damageImage.color = flashColour;
        }
        else
        {
            //Transition from one color to another for flashin effect
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

    }

    //Called from enemyAttack script to rest health to player
    public void TakeDamage(int amount)
    {
        //Set damaged bool to true to check on update() and flash color
        damaged = true;

        //Rest damage
        currentHealth -= amount;

        //Change health text on UI
        lifeText.text = currentHealth.ToString();

        //Change slider on UI
        healthSlider.value = currentHealth;

        //Plays hurt sound from player audiosource
        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            //this is for showing 0, not negative values on UI
            lifeText.text = "0";
            Death();
        }
    }


    void Death()
    {
        //This is used to check if is already dead on TakeDamage()
        isDead = true;

        //Dissable particles effects
        playerShooting.DisableEffects();

        //Stops all enemies
        playManager.StopAllEnemies();

        //Stop players after die (disable movement and shoots)
        playManager.StopPlayers();

        //Stop background music
        playManager.StopBGMusic();

        //Stop gamepads vibrations
        playManager.StopAllVibrations();

        if (PlayersLives.lives > 1) { 
            //This raise the screen Die animation
            hudAnim.SetTrigger("Die");

            //This raise the player Die animation
            anim.SetTrigger("Die");

            //Rests 1 live
            PlayersLives.lives -= 1;
        }
        else //Call gameover in opposite of die hud screen
        {
            playManager.GameOver();
        }

        //Playes audioclip(without audiosource)
        playerAudio.clip = deathClip;
        playerAudio.Play();

        //Disables player shoot and movement after death
        playerMovement.enabled = false;
        playerShooting.enabled = false;

        //Stops the timer on playManager for countdown
        PlayManager.stopTimer = true;

    }



}
