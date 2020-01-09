using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    public Transform playerTarget2;
    PlayerHealth playerHealth;
    Player2Health player2Health;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;


    void Awake ()
    {
        //I need to assign transform because i need later the vector3 to SetDestination
        //player = GameObject.FindGameObjectWithTag ("Player").transform;
        //assign references
        playerHealth = playerTarget.GetComponent <PlayerHealth> ();
        player2Health = playerTarget2.GetComponent<Player2Health>();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {

        //We have to compare witch player is near to assign nav destination
        float distanceP1 = (transform.position - playerTarget.position).magnitude;
        float distanceP2 = (transform.position - playerTarget2.position).magnitude;

        //Check if nav.enabled is true because if we set to false in PlayerManager for StopAll
        //it raises an error on SetDestination
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && nav.enabled == true && distanceP1 < distanceP2)
        {
            nav.SetDestination (playerTarget.position);
        }
        else if (enemyHealth.currentHealth > 0 && player2Health.currentHealth > 0 && nav.enabled == true && distanceP2 < distanceP1)
        {
            nav.SetDestination(playerTarget2.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
