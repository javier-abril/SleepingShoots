using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;
    public AudioClip deathClip;
    Animator anim;
    AudioSource enemyAudio;

    //This is the particles played when receives a shoot
    ParticleSystem hitParticles;

    //Is needed to check if its dead on TakeDamage()
    //The value is assigned on Death()
    bool isDead;
    
    void Awake ()
    {

        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        
        //Sets current health on start
        currentHealth = startingHealth;
    }


    void Update ()
    {
        
    }

    //Called when player shoots and hits enemy, on function Shoot()
    public void TakeDamage (int amount, Vector3 hitPoint, string playerName)
    {
        if (!isDead)
        {
            //Plays impact audio from enemy
            enemyAudio.Play();

            //Rests health
            currentHealth -= amount;

            //Animation for particles hits on hitpoint passed in parameters
            hitParticles.transform.position = hitPoint;
            hitParticles.Play();


            if (currentHealth <= 0)
            {
                //update the public static int score, with the score value on death
                if (playerName == "Player")
                    PlayerScore.score += scoreValue;
                else
                    PlayerScore.score2 += scoreValue;

                Death();
            }
        }
    }


    void Death ()
    {
        //To avoid get damage after death on TakeDamage()
        isDead = true;

        //Raise die enemy animation
        anim.SetTrigger ("died");

        //Change audio source clip to deathclip and plays
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    //Function called from death animation events
    public void StartDissapearing ()
    {
        Destroy (gameObject, 2f);
    }
}
