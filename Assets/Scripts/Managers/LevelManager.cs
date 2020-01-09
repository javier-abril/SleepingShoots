using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static int levelNumber = 1;
    Text text;


    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        text.text = "LEVEL " + levelNumber;
	}
}
