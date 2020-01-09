using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserRegisterJSON {


    public bool ok;
    public int idUsuario;
    public string error;


    public static UserRegisterJSON CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserRegisterJSON>(jsonString);
    }
}

