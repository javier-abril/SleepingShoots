using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        //Find player by tag. For these its needed to asign a tag to GameObject
        player = GameObject.FindGameObjectWithTag ("Player");
        //Assign references
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }

    //This is triggered when enemy sphere collider collides with other collider
    void OnTriggerEnter (Collider other)
    {
        //If collides with player, player is in range attack
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    //This is triggered when enemy exits from collide with other
    void OnTriggerExit (Collider other)
    {
        
        if(other.gameObject == player)
        {
            playerInRange = false;
            anim.SetBool("attack", false); //Also stop the attack animation
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        //Check if timerbetweebattacks has passed, player is in range and enemy has health > 0
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {          
            Attack ();
        }

        //Raise playerDead if enemy kills
        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("playerDead");
        }
    }


    void Attack ()
    {
        //On every attack i need to put timer to 0 for checking time between attacks on Update()
        timer = 0f;

        //Player is alive
        if(playerHealth.currentHealth > 0)
        {
            //Launch attack animation
            anim.SetBool("attack", true);
            //Call take damage from playerhealth to rest damaged
            playerHealth.TakeDamage (attackDamage);
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
            if(aud.name == "hips")
            {
                aud.Play();
            }
        }

    }
}
