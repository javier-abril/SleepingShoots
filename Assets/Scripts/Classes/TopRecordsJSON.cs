using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TopRecordsJSON : MonoBehaviour {

    public bool ok;
    public List<Record> records = new List<Record>();

    public static TopRecordsJSON CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<TopRecordsJSON>(jsonString);
    }
}
