using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//This could not inherit from monobehaviour because it cannot
public class UserLoginJSON{

    public bool ok;
    public int idUsuario;


    public static UserLoginJSON CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserLoginJSON>(jsonString);
    }

}
