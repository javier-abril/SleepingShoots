using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class WebServiceAPI : MonoBehaviour {

    public User userLogued;
    public UserRegisterJSON userRegistered;
    public UserRecordsJSON userRecord;
    public TopRecordsJSON topRecords;

    // Use this for initialization
    public WebServiceAPI()
    {
        userLogued = new User();
    }

    
    async public Task Login(string name, string password)
    {

        WWWForm form = new WWWForm();

        form.AddField("name", name);
        form.AddField("password", password);

        WWW www = new WWW("https://sleepingshoots.azurewebsites.net/login", form);

        do
        {
            await Task.Delay(100);

        } while (!www.isDone);
        
        Debug.Log(www.text);

        UserLoginJSON userJSON = JsonUtility.FromJson<UserLoginJSON>(www.text);

        userLogued.Name = name;
        userLogued.Id = userJSON.idUsuario; 

        Debug.Log("ok:" + userJSON.ok);
        Debug.Log("id:" + userJSON.idUsuario);

    }

    async public Task Register(string name, string password, string email)
    {

        WWWForm form = new WWWForm();

        form.AddField("name", name);
        form.AddField("password", password);
        form.AddField("email", email);

        WWW www = new WWW("https://sleepingshoots.azurewebsites.net/register", form);

        do
        {
            await Task.Delay(100);

        } while (!www.isDone);

        Debug.Log(www.text);

        userRegistered = JsonUtility.FromJson<UserRegisterJSON>(www.text);

        userLogued.Name = name;
        userLogued.Id = userRegistered.idUsuario;

        Debug.Log("ok:" + userRegistered.ok);
        Debug.Log("id:" + userRegistered.idUsuario);
        Debug.Log("error:" + userRegistered.error);

    }

    async public Task InsertRecord(int record)
    {

        WWWForm form = new WWWForm();

        form.AddField("userid", Convert.ToInt32(userLogued.Id));
        form.AddField("puntos", record);
 
        WWW www = new WWW("https://sleepingshoots.azurewebsites.net/insertaRecord", form);

        do
        {
            await Task.Delay(100);

        } while (!www.isDone);

        Debug.Log(www.text);

        userRecord = JsonUtility.FromJson<UserRecordsJSON>(www.text);

        Debug.Log("ok:" + userRecord.ok);
        Debug.Log("error:" + userRecord.error);

    }

    //Insert records from user object
    async public Task InsertRecord(User p1)
    {

        WWWForm form = new WWWForm();

        form.AddField("userid", Convert.ToInt32(p1.Id));
        form.AddField("puntos", p1.Record);

        WWW www = new WWW("https://sleepingshoots.azurewebsites.net/insertaRecord", form);

        do
        {
            await Task.Delay(100);

        } while (!www.isDone);

        Debug.Log(www.text);

        userRecord = JsonUtility.FromJson<UserRecordsJSON>(www.text);

        Debug.Log("ok:" + userRecord.ok);
        Debug.Log("error:" + userRecord.error);

    }


    async public Task GetTopRecords()
    {

        topRecords = new TopRecordsJSON();

        topRecords.ok = false;

        WWWForm form = new WWWForm();
        

        WWW www = new WWW("https://sleepingshoots.azurewebsites.net/getTopRecords", form);

        do
        {
            await Task.Delay(100);

        } while (!www.isDone);

        Debug.Log(www.text);

        //As unity JsonUtility Cannot parse JsonArrays i need to parse it manually. If ok is true i 
        //get records splitted manually and added to TopRecordsJSON list of records
        if (www.text.Contains("\"ok\":true"))
        {

            string textTemp = www.text.Replace("\"ok\":true,", "");
            int posIni = textTemp.IndexOf('[');
            int posEnd = textTemp.IndexOf(']');

            textTemp = textTemp.Substring(posIni + 1,posEnd - posIni-1);

            //replace separator comma por dot comma to split records correctly later
            textTemp = textTemp.Replace("},{", "};{");

            //Split Records into an array
            string[] textSplitted = textTemp.Split(';');

            topRecords.ok = true;

            for (int i = 0; i < textSplitted.Length; i++)
            {
                Record rec = JsonUtility.FromJson<Record>(textSplitted[i]);
                topRecords.records.Add(rec);
            }

        }
        else if(www.text == "")
        {
            throw new Exception ("Error contacting with server");
        }
        
    }




}
