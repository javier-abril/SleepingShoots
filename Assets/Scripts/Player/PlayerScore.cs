using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public static int score = 0;
    public static int score2 = 0;
    Text text;


    void Awake()
    {
        text = GetComponent<Text>();
    }


    void Update()
    {
        //If is attached on P1 Hud Object
        if(name == "Record")
        {
            text.text = "Score: " + score.ToString();
            StaticManager.p1.Record = score;
        }
        else //If is attached on P2 Hud Object
        {
            text.text = "Score: " + score2.ToString();
            StaticManager.p2.Record = score2;
        }
        
    }
}
