using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersLives : MonoBehaviour {
    
    //Is needed to use static int to avoid lost his value on restartLevel
    public static int lives = 3;

    Text text;


    void Awake()
    {
        text = GetComponent<Text>();
    }


    void Update()
    {

        text.text = "X " + lives.ToString();
       
    }
}
