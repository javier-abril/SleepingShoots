using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour {

    public static StaticManager Instance { get; private set; }

    public static User p1;
    public static User p2;
    public static int level = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            //Default players id for saving records
            p1 = new User("Player1",9);
            p2 = new User("Player2",11);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
