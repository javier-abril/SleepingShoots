using UnityEngine;

public class Player2Movement : MonoBehaviour
{

    public float speed = 6f;
    public float speedRotationJoy = 5f;
    public float jumpForce = 2f;
    public LayerMask floorLayer;
    public int floorLayerInt;

    //Max distance for the camRay, this was used to rotate with mouse
    //public float distanceCamRay = 100f;

    Vector3 movement;
    Rigidbody jugRigidbody;
    CapsuleCollider jugCollider;

    Animator animator;


    void Awake()
    {
        jugRigidbody = GetComponent<Rigidbody>();
        jugCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        //Gets int from layer "suelo". This is needed to check if player is no the floor IsOntheFloor()
        floorLayerInt = LayerMask.GetMask("Suelo");
    }

    void Update()
    {
        //Get movement from input
        float moveHor = Input.GetAxisRaw("HorizontalP2");
        float moveVer = Input.GetAxisRaw("VerticalP2");

        //Calls function to move player
        Move(moveHor, moveVer);

        //this is to avoid jumping if player is not on the floor
        if (Input.GetButton("JumpP2") && IsOnTheFloor())
        {
            Jump();
        }

        //Function to rotate with joystick input buttons
        JoyTurning();

        AnimateWalk(moveHor, moveVer);

    }

    //I made it public to stop walking animation from player manager
    public void AnimateWalk(float h, float v)
    {
        //Conditional assignment to walking
        //if h != 0 or v!= 0 walking = true
        bool walking = h != 0f || v != 0f;

        //Calls walking animation
        animator.SetBool("IsWalking", walking);
    }

    void JoyTurning()
    {
        transform.Rotate(0.0f, -Input.GetAxisRaw("RotacionP2") * speedRotationJoy, 0.0f);
    }

    /*
     * This was used firstly to rotate with mouse Y
     * but since i want to made it multiplayer is better to rotate
     * with JOYstick support (implemented on JoyTurning)
     * 
    void Turn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, distanceCamRay, floorLayerInt))
        {
            Vector3 jugadorAMouse = floorHit.point - transform.position;
            jugadorAMouse.y = 0f;

            Quaternion nuevaRotacion = Quaternion.LookRotation(jugadorAMouse);
            jugRigidbody.MoveRotation(nuevaRotacion);
         }
    }
    */

    //Function to move player depending on speed setted
    void Move(float h, float v)
    {

        //initialise new vector with move values
        movement = new Vector3(h, 0, v);

        //applies the speed factor to movement
        movement = movement.normalized * speed * Time.deltaTime;

        //moves players rigidbody (current player transform position - movement)
        jugRigidbody.MovePosition(transform.position - movement);

    }

    void Jump()
    {
        //Adds impulse force UP to rigidbody to JUMP
        jugRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    //Checks if player is colliding with the floor layer
    bool IsOnTheFloor()
    {
        //Gets the actual position of player's collider
        Vector3 posiCollider = new Vector3(jugCollider.bounds.center.x,
                                jugCollider.bounds.center.y, jugCollider.bounds.center.z);

        //Check if player is touching the floor layer
        //For this, is needed to put on the player layer and floor in "suelo" layer
        return Physics.CheckCapsule(jugCollider.bounds.center, posiCollider, jugCollider.radius * 1.65f, floorLayer);
    }


}
