using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPU : MonoBehaviour {

    public GameObject P1;
    public GameObject P2;
    public AudioClip PowerUpAudio;
    LineRenderer weaponAmmoP1;
    LineRenderer weaponAmmoP2;
    PlayerShooting p1ShootScript;
    Player2Shooting p2ShootScript;
    AudioSource p1AudioShoot;
    AudioSource p2AudioShoot;

    // Use this for initialization
    void Start () {

        weaponAmmoP1 = P1.GetComponent<LineRenderer>();
        weaponAmmoP2 = P2.GetComponent<LineRenderer>();
        p1ShootScript = P1.GetComponent<PlayerShooting>();
        p2ShootScript = P2.GetComponent<Player2Shooting>();
        p1AudioShoot = P1.GetComponent<AudioSource>();
        p2AudioShoot = P2.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //When the collider istriggered
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //Change the size off linerenderer
            weaponAmmoP1.SetWidth(0.2f, 0.2f);

            //Change damage on P1
            p1ShootScript.damagePerShot = 60;
            //p1ShootScript.timeBetweenBullets = 0.075f;

            //Change color off line renderer
            Material[] mats = weaponAmmoP1.materials;

            Color col = new Color();

            col.a = 0.3f;
            col.b = 0f;
            col.g = 0f;
            col.r = 150f;

            mats[0].color = col;

            //Change audio
            p1AudioShoot.clip = PowerUpAudio;

            //Hides this powerUp
            this.gameObject.SetActive(false);

        }
        else if(other.tag == "Player2")
        {
            //Change the size off linerenderer
            weaponAmmoP2.SetWidth(0.2f, 0.2f);

            //Change damage on P1
            p2ShootScript.damagePerShot = 60;
            //p2ShootScript.timeBetweenBullets = 0.075f;

            //Change color off line renderer
            Material[] mats = weaponAmmoP2.materials;

            Color col = new Color();

            col.a = 0.3f;
            col.b = 150f;
            col.g = 0f;
            col.r = 0f;

            mats[0].color = col;

            //Change audio
            p2AudioShoot.clip = PowerUpAudio;

            //Hides this powerUp
            this.gameObject.SetActive(false);
        }
    }
}
