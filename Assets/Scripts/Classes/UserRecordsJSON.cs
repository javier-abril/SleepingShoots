using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UserRecordsJSON {

    public bool ok;
    public int idUsuario;
    public string error;


    public static UserRecordsJSON CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserRecordsJSON>(jsonString);
    }
}
