using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeguimiento : MonoBehaviour {

    public Transform targetPlayer;
    public Transform targetPlayer2;
    public Camera cameraFollow;


    /* Working with 1 player

    public float suavizado = 5f;

    Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - targetPlayer.position;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 targetCamPosi = targetPlayer.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetCamPosi, suavizado * Time.deltaTime);
            
	}*/

    void Update()
    {
        FixedCameraFollowSmooth(cameraFollow, targetPlayer, targetPlayer2);
    }

    public void FixedCameraFollowSmooth(Camera cam, Transform t1, Transform t2)
    {
        // How many units should we keep from the players
        // Minus than 2.8 shows a band in bottom screen
        float zoomFactor = 2.8f;
        float followTimeDelta = 0.8f;

        // Midpoint we're after
        Vector3 midpoint = (t1.position + t2.position) / 2f;

        // Distance between objects
        float distance = (t1.position - t2.position).magnitude;

        // Move camera a certain distance
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;

        // Adjust ortho size if we're using one of those
        if (cam.orthographic)
        {
            //if distance is more then 36 objects will become very small and is not playable
            //its needed that ortographic size * 3 = distance to avoid camera jumps
            if (distance > 36f)
                cam.orthographicSize = 12f;
            else if (distance <= 15f)
                cam.orthographicSize = 5f;
            else
                //I have to divide distance because de proportion distancePlayers-distanceCam was incorrect
                cam.orthographicSize = distance / 3f;

        }
        
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }

}
