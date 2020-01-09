using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack2P : MonoBehaviour {


    public float timeBetweenAttacks = 1f;
    public float vibrationDuration = 6f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    GameObject player2;

    GameObject audioGameObject;
    AudioSource bgAudio;
    PlayerHealth playerHealth;
    Player2Health player2Health;
    EnemyHealth enemyHealth;
    bool playerInRange;
    bool player2InRange;

    float timerP1;
    float timerP2;


    void Awake()
    {
        //Find player by tag. For these its needed to asign a tag to GameObject
        player = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        audioGameObject = GameObject.FindGameObjectWithTag("BGmusic");
        bgAudio = audioGameObject.GetComponent<AudioSource>();
        

        //Assign references
        playerHealth = player.GetComponent<PlayerHealth>();
        player2Health = player2.GetComponent<Player2Health>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    //This is triggered when enemy sphere collider collides with other collider
    void OnTriggerEnter(Collider other)
    {
        //If collides with player, player is in range attack
        if (other.gameObject == player)
        {
            playerInRange = true;

        }
        else if (other.gameObject == player2)
        {
            player2InRange = true;
            
        }
    }

    //This is triggered when enemy exits from collide with other
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject == player)
        {
            playerInRange = false;
            anim.SetBool("attack", false); //Also stop the attack animation
            
        }
        else if (other.gameObject == player2)
        {
            player2InRange = false;
            anim.SetBool("attack", false); //Also stop the attack animation
            
        }
    }


    void Update()
    {
        
        timerP1 += Time.deltaTime;
        timerP2 += Time.deltaTime;


        //Check if timerbetweebattacks has passed, player is in range and enemy has health > 0
        if (timerP1 >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            
            //Raise playerDead if enemy kills to stop enemy animation
            if (playerHealth.currentHealth <= 0)
            {
                anim.SetTrigger("playerDead");
            }
            else
            {
                Attack();
            }
        }
        else if (timerP2 >= timeBetweenAttacks && player2InRange && enemyHealth.currentHealth > 0)
        {
            //Raise playerDead if enemy kills to stop enemy animation
            if (player2Health.currentHealth <= 0)
            {
                anim.SetTrigger("playerDead");
            }
            else
            {
                AttackP2();
            }
        }
       
        
    }


    void Attack()
    {
        //On every attack i need to put timer to 0 for checking time between attacks on Update()
        timerP1 = 0f;
        
        //Player is alive
        if (playerHealth.currentHealth > 0)
        {
            //Launch attack animation
            anim.SetBool("attack", true);
            //Call take damage from playerhealth to rest damaged
            playerHealth.TakeDamage(attackDamage);
            
        }

    }

    void AttackP2()
    {
        //On every attack i need to put timer to 0 for checking time between attacks on Update()
        timerP2 = 0f;

        //Player is alive
        if (player2Health.currentHealth > 0)
        {
            //Launch attack animation
            anim.SetBool("attack", true);
            //Call take damage from playerhealth to rest damaged
            player2Health.TakeDamage(attackDamage);
        }

    }

    //Called from event animation attack1
    public void AttackSound()
    {
        Component[] audioSources;

        //Get all children audio sources(it has on hips, foots...)
        audioSources = GetComponentsInChildren<AudioSource>();

        foreach (AudioSource aud in audioSources)
        {
            //Raise the sound attack on hips audiosource
            if (aud.name == "hips")
            {
                aud.Play();
            }
        }

    }
}
