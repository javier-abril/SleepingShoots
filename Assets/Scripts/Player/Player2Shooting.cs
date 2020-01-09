using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Player2Shooting : MonoBehaviour {

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f; //Ray shoot length
    float effectsDisplayTime = 0.2f;

    //timer to check timeBetweenBullets
    float timer;

    //Shoot ray representation
    Ray shootRay = new Ray();

    //This is to get information from where shoots collides
    RaycastHit shootHit;

    //int of "shootable" layer(where the enemies are)
    int shootableMask;

    //gunParticles to play on shoot
    ParticleSystem gunParticles;

    //line trace of shoot launched
    LineRenderer gunLine;

    AudioSource gunAudio;

    //gunlight effect enabled when shoot
    Light gunLight;

    //This is for vibrating xbox gamepad P2
    PlayerIndex playerIndex = PlayerIndex.Two;

    bool isShooting;

    void Awake()
    {
        //References "shootable" layer (where enemies are)
        shootableMask = LayerMask.GetMask("Shootable");

        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }


    void Update()
    {
        timer += Time.deltaTime;

        //Check if trigger shoot is pushed for enable vibration right
        if (Input.GetAxisRaw("Fire1P2") == 0 && isShooting)
        {
            isShooting = false;
            GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
        }
        else
            GamePad.SetVibration(playerIndex, 0.0f, Input.GetAxisRaw("Fire1P2") / 2);


        //Checks if timebetweenbullets has passed and fire1 is pressed
        if ((Input.GetButton("Fire1P2") || Input.GetAxisRaw("Fire1P2") > 0) && timer >= timeBetweenBullets /*&& Time.timeScale != 0*/)
        {
            Shoot();
        }

        //disable light and line shooting effect when timeBetweenBullets * effectsDisplayTime passed
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    //disable light and line shooting effect
    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot()
    {
        //this variable is to disable vibration on Update()
        //is to prevent stopvibration while not isShooting
        isShooting = true;

        //Resets timer on every shoot
        timer = 0f;

        //Plays shoot sound
        gunAudio.Play();

        //Enable gunlight effect
        gunLight.enabled = true;

        //Plays guns shoot particles(like smoke)
        gunParticles.Stop();
        gunParticles.Play();

        //Enable line bullet renderer
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        //shootray in direction forward
        //is very important that gameobject is in correct direction
        shootRay.direction = transform.forward;

        //Check if hits any collider
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            //if touch an enemy call take damage
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point,"Player2");
            }
            //This is for not traspassing the enemy
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {   //If does not hit any enemy shows complete range shootray
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}