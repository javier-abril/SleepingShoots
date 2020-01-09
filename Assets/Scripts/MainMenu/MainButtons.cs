using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using System;

public class MainButtons : MonoBehaviour {

    public Text TextError;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject ImageBtnP1;
    public GameObject ImageBtnP2;
    public GameObject ImgLoginP1;
    public GameObject ImgLoginP2;
    public GameObject ImgRegisterP1;
    public GameObject ImgRegisterP2;
    public GameObject BtnStart;
    public GameObject BtnRecords;
    public GameObject BtnQuit;
    public GameObject BtnLoginP1;
    public GameObject BtnLoginP2;
    public GameObject ImgConnection;
    public GameObject ImgTopRecords;
    public Text TextTopRecords;
    public Text TextNameP1;
    public Text TextNameP2;

    List<Color> colorP1 = new List<Color>();
    List<Color> colorP2 = new List<Color>();

    int p1ColorPosi;
    int p2ColorPosi;

    //Use this to avoid bucle of changing colors
    float p1BotonColor;
    float p2BotonColor;

    //Declaration of webservice API class for calling asinc task WS
    WebServiceAPI ws = new WebServiceAPI();

    List<Record> recordsList = new List<Record>();

    // Use this for initialization
    void Start () {

        colorP1.Add(Color.white);
        colorP1.Add(Color.blue);
        colorP1.Add(Color.red);
        colorP1.Add(Color.green);
        colorP1.Add(Color.grey);
        colorP1.Add(Color.yellow);
        colorP1.Add(Color.magenta);
        colorP1.Add(Color.black);

        colorP2.Add(Color.white);
        colorP2.Add(Color.blue);
        colorP2.Add(Color.red);
        colorP2.Add(Color.green);
        colorP2.Add(Color.grey);
        colorP2.Add(Color.yellow);
        colorP2.Add(Color.magenta);
        colorP2.Add(Color.black);
    }
	
	// Update is called once per frame
	void Update () {

        //If we push A button, hide button image and show player
        if (Input.GetButton("Fire3") && !Player1.activeSelf)
        {
            ImageBtnP1.SetActive(false);
            Player1.SetActive(true);
        }

        if (Input.GetButton("Fire3P2") && !Player2.activeSelf)
        {
            ImageBtnP2.SetActive(false);
            Player2.SetActive(true);
        }



        if (Input.GetAxisRaw("Rotacion") == 1 && p1ColorPosi < 7 && p1BotonColor < 1)
        {
            p1ColorPosi++;
        }
        else if (Input.GetAxisRaw("Rotacion") ==1 && p1ColorPosi == 7 && p1BotonColor < 1)
        {
            p1ColorPosi = 0;
        }
        else if(Input.GetAxisRaw("Rotacion")==-1 && p1ColorPosi > 0 && p1BotonColor > -1)
        {
            p1ColorPosi--;
        }
        else if (Input.GetAxisRaw("Rotacion")==-1 && p1ColorPosi == 0 && p1BotonColor > -1)
        {
            p1ColorPosi = 7;
        }



        if (Input.GetAxisRaw("RotacionP2") == 1 && p2ColorPosi < 7 && p2BotonColor < 1)
        {
            p2ColorPosi++;
        }
        else if (Input.GetAxisRaw("RotacionP2") == 1 && p2ColorPosi == 7 && p2BotonColor < 1)
        {
            p2ColorPosi = 0;
        }
        else if (Input.GetAxisRaw("RotacionP2") == -1 && p2ColorPosi > 0 && p2BotonColor > -1)
        {
            p2ColorPosi--;
        }
        else if (Input.GetAxisRaw("RotacionP2")== -1 && p2ColorPosi == 0 && p2BotonColor > -1)
        {
            p2ColorPosi = 7;
        }
       

        //If player is active we can change the color
        if (Player1.activeSelf)
        {
            //Gets the proper renderer for player skin and change the albedo color
            Renderer[] rend = Player1.GetComponentsInChildren<Renderer>();
            rend[3].material.SetColor("_Color", colorP1[p1ColorPosi]);
        }

        //If player2 is active we can change the color
        if (Player2.activeSelf)
        {
            //Gets the proper renderer for player skin and change the albedo color
            Renderer[] rend = Player2.GetComponentsInChildren<Renderer>();
            rend[3].material.SetColor("_Color", colorP2[p2ColorPosi]);
        }

        //Update its value for check in if of change color to avoid color change loop
        p1BotonColor = Input.GetAxisRaw("Rotacion");
        p2BotonColor = Input.GetAxisRaw("RotacionP2");
    }

    public void RunFirstScene()
    {
        if (Player2.activeSelf && Player1.activeSelf)
        {
            //We save color player to staticmanager, on level 1 web are going to get color
            StaticManager.p1.PlayerColor = colorP1[p1ColorPosi];
            StaticManager.p2.PlayerColor = colorP2[p2ColorPosi];
            StaticManager.p1.Record = 0;
            StaticManager.p2.Record = 0;

            PlayerScore.score = 0;
            PlayerScore.score2 = 0;

            PlayersLives.lives = 3;
            SceneManager.LoadScene("Level1");
        }
        else
            TextError.text = "Select both players before starting";
    }


    void enableButtons()
    {
        BtnLoginP1.SetActive(true);
        BtnLoginP2.SetActive(true);
        BtnStart.SetActive(true);
        BtnRecords.SetActive(true);
        BtnQuit.SetActive(true);
    }

    void disableButtons()
    {
        
        BtnLoginP1.SetActive(false);
        BtnLoginP2.SetActive(false);
        BtnStart.SetActive(false);
        BtnRecords.SetActive(false);
        BtnQuit.SetActive(false);
    }


    public void OpenLoginWindowP1()
    {
        if (!ImgLoginP1.activeSelf)
        {

            disableButtons();
            //Show img login
            ImgLoginP1.SetActive(true);

        }

    }


    public void OpenLoginWindowP2()
    {
        if (!ImgLoginP2.activeSelf)
        {
            disableButtons();

            //Show img login
            ImgLoginP2.SetActive(true);
        }
    }


    public void RegP1()
    {
        Text[] listTexts = ImgRegisterP1.GetComponentsInChildren<Text>();

        try
        {
            //Shows image before connection
            ImgConnection.SetActive(true);

            string user = listTexts[1].text;
            string pass = listTexts[3].text;
            string email = listTexts[5].text;

            RegisterP1(user, pass, email);

        }
        catch (Exception ex)
        {
            ImgConnection.SetActive(false);
            listTexts[6].text = ex.Message;
        }

    }

    async public void RegisterP1(string user, string password, string email)
    {

        try
        {

            Text[] listTexts = ImgRegisterP1.GetComponentsInChildren<Text>();

            await ws.Register(user, password, email);


            if (ws.userRegistered.idUsuario <= 0)
            {
                //Error textbox inside RegisterP1
                listTexts[6].text = "User name already used";

            }
            else
            {
                TextNameP1.text = user;

                //Update static manager settings for persistence
                StaticManager.p1.Name = user;
                StaticManager.p1.Id = ws.userLogued.Id;

                enableButtons();

                //Clear all textbox on exit
                listTexts[1].text = "";
                listTexts[3].text = "";
                listTexts[5].text = "";
                listTexts[6].text = "";

                //Hide login window
                ImgRegisterP1.SetActive(false);
            }

            //Hide connection anim
            ImgConnection.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.Log("Error records");
            TextError.text = "Error contacting with register server";
            //Hide toprecords windows
            ImgRegisterP1.SetActive(false);
            //Hide connection anim
            ImgConnection.SetActive(false);

            enableButtons();
        }
    }




    public void RegP2()
    {
        Text[] listTexts = ImgRegisterP2.GetComponentsInChildren<Text>();

        try
        {
            //Shows image before connection
            ImgConnection.SetActive(true);

            string user = listTexts[1].text;
            string pass = listTexts[3].text;
            string email = listTexts[5].text;

            RegisterP2(user, pass, email);

        }catch (Exception ex)
        {
            ImgConnection.SetActive(false);
            listTexts[6].text = ex.Message;
        }

    }

    async public void RegisterP2(string user, string password, string email)
    {

        try
        {

            Text[] listTexts = ImgRegisterP2.GetComponentsInChildren<Text>();

            await ws.Register(user, password, email);


            if (ws.userRegistered.idUsuario <= 0)
            {
                //Error textbox inside RegisterP1
                listTexts[6].text = "User name already used";

            }
            else
            {
                TextNameP2.text = user;

                //Update static manager settings for persistence
                StaticManager.p2.Name = user;
                StaticManager.p2.Id = ws.userLogued.Id;

                enableButtons();

                //Clear all textbox on exit
                listTexts[1].text = "";
                listTexts[3].text = "";
                listTexts[5].text = "";
                listTexts[6].text = "";

                //Hide login window
                ImgRegisterP2.SetActive(false);
            }

            //Hide connection anim
            ImgConnection.SetActive(false);

        }
        catch (Exception ex)
        {
            Debug.Log("Error records");
            TextError.text = "Error contacting with register server";
            //Hide toprecords windows
            ImgRegisterP2.SetActive(false);
            //Hide connection anim
            ImgConnection.SetActive(false);

            enableButtons();
        }
    }

    //We use ImgLoginP1 to get text in children a pass values to LoginP1 async
    public void LogP1()
    {
        Text[] listTexts = ImgLoginP1.GetComponentsInChildren<Text>();

        try { 
        //Shows image before connection
        ImgConnection.SetActive(true);       

        string user = listTexts[1].text;
        string pass = listTexts[3].text;

        LoginP1(user, pass);

        }
        catch (Exception ex)
        {
            ImgConnection.SetActive(false);
            listTexts[4].text = ex.Message;
        }

    }

    //We cannot access directly to an async function, we use LogP1 for get text values and pass to this
    async public void LoginP1(string user, string password)
    {
        try { 

            await ws.Login(user, password);

            if(ws.userLogued.Id == -1)
            {
                Text[] listTexts = ImgLoginP1.GetComponentsInChildren<Text>();
                //Error textbox inside loginP1
                listTexts[4].text = "Incorrect user or password";
            }
            else if (ws.userLogued.Id != -1 && ws.userLogued.Id == StaticManager.p2.Id)
            {
                Text[] listTexts = ImgLoginP2.GetComponentsInChildren<Text>();
                //Error textbox inside loginP2
                listTexts[4].text = "User logged by P2";
            }
            else
            {
                TextNameP1.text = user;

                //Update static manager settings for persistence
                StaticManager.p1.Name = user;
                StaticManager.p1.Id = ws.userLogued.Id;

                enableButtons();

                Text[] listTexts = ImgLoginP1.GetComponentsInChildren<Text>();
                //Clear all textbox on exit
                listTexts[1].text = "";
                listTexts[3].text = "";
                listTexts[4].text = "";

                //Hide login window
                ImgLoginP1.SetActive(false);
            }

            //Hide connection anim
            ImgConnection.SetActive(false);

        }
        catch (Exception ex)
        {
            TextError.text = "Error contacting with login server";
            //Hide login window
            ImgLoginP1.SetActive(false);
            //Hide connection anim
            ImgConnection.SetActive(false);

            enableButtons();
        }

    }


    //We use ImgLoginP2 to get text in children a pass values to LoginP1 async
    public void LogP2()
    {
        Text[] listTexts = ImgLoginP2.GetComponentsInChildren<Text>();

        try
        {
            //Shows image before connection
            ImgConnection.SetActive(true);

            string user = listTexts[1].text;
            string pass = listTexts[3].text;

            LoginP2(user, pass);

        }
        catch (Exception ex)
        {
            ImgConnection.SetActive(false);
            listTexts[4].text = ex.Message;
        }

    }

    //We cannot access directly to an async function, we use LogP1 for get text values and pass to this
    async public void LoginP2(string user, string password)
    {

        try
        {

            await ws.Login(user, password);

            if (ws.userLogued.Id == -1)
            {
                Text[] listTexts = ImgLoginP2.GetComponentsInChildren<Text>();
                //Error textbox inside loginP2
                listTexts[4].text = "Incorrect user or password";
            }
            else if (ws.userLogued.Id != -1 && ws.userLogued.Id == StaticManager.p1.Id)
            {
                Text[] listTexts = ImgLoginP2.GetComponentsInChildren<Text>();
                //Error textbox inside loginP2
                listTexts[4].text = "User logged by P1";
            }
            else
            {

                TextNameP2.text = user;

                //Update static manager settings for persistence
                StaticManager.p2.Name = user;
                StaticManager.p2.Id = ws.userLogued.Id;

                enableButtons();

                Text[] listTexts = ImgLoginP2.GetComponentsInChildren<Text>();
                //Clear all textbox on exit
                listTexts[1].text = "";
                listTexts[3].text = "";
                listTexts[4].text = "";

                //Hide login window
                ImgLoginP2.SetActive(false);
            }

            //Hide connection anim
            ImgConnection.SetActive(false);

        }catch (Exception ex)
        {
            TextError.text = "Error contacting with login server";
            //Hide login window
            ImgLoginP2.SetActive(false);
            //Hide connection anim
            ImgConnection.SetActive(false);

            enableButtons();
        }

    }


    async public void GetTopRecords()
    {
        try { 

            //Shows image before connection
            ImgConnection.SetActive(true);

            await ws.GetTopRecords();

            //If result is OK we get list of records
            if (ws.topRecords.ok)
            {

                this.recordsList = ws.topRecords.records;

                TextTopRecords.text = "";

                foreach (Record rec in recordsList)
                {
                    TextTopRecords.text = TextTopRecords.text + "\t" + rec.nombre + "  -  " + rec.puntos + "\r\n";
                }

                ImgTopRecords.SetActive(true);

                disableButtons();
            
            }

            //Shows image before connection
            ImgConnection.SetActive(false);

        }
        catch (Exception ex)
        {
            Debug.Log("Error records");
            TextError.text = "Error contacting with records server";
            //Hide toprecords windows
            ImgTopRecords.SetActive(false);
            //Hide connection anim
            ImgConnection.SetActive(false);

            enableButtons();
        }
    }


    public void OpenRegisterP1()
    {
        CloseLoginWindowNobuttons();

        if (!ImgRegisterP1.activeSelf)
            ImgRegisterP1.SetActive(true);
    }


    public void OpenRegisterP2()
    {
        CloseLoginWindowNobuttons();

        if (!ImgRegisterP2.activeSelf)
            ImgRegisterP2.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void OpenTopRecords()
    {

        if (!ImgTopRecords.activeSelf)
        {
            if (recordsList.Count > 0)
            {
                TextTopRecords.text = "";

                foreach (Record rec in recordsList)
                {
                    TextTopRecords.text = TextTopRecords.text + rec.nombre + "  -  " + rec.puntos + "\r\n";
                }

                ImgTopRecords.SetActive(true);

                disableButtons();

            }
            else
            {

                this.GetTopRecords();

            }
        }
    }

    public void CloseTopRecords()
    {
        if (ImgTopRecords.activeSelf)
        {
            ImgTopRecords.SetActive(false);

            enableButtons();
        }
    }

    public void CloseLoginWindow()
    {
        if (ImgLoginP1.activeSelf)
        {
            ImgLoginP1.SetActive(false);

            enableButtons();
        }

        if (ImgLoginP2.activeSelf)
        {
            ImgLoginP2.SetActive(false);

            enableButtons();
        }
            
    }

    public void CloseLoginWindowNobuttons()
    {
        if (ImgLoginP1.activeSelf)
        {
            ImgLoginP1.SetActive(false);
        }

        if (ImgLoginP2.activeSelf)
        {
            ImgLoginP2.SetActive(false);
        }

    }


    public void CloseRegisterWindow()
    {
        if (ImgRegisterP1.activeSelf)
        {
            ImgRegisterP1.SetActive(false);

            enableButtons();
        }

        if (ImgRegisterP2.activeSelf)
        {
            ImgRegisterP2.SetActive(false);

            enableButtons();
        }

    }
}
