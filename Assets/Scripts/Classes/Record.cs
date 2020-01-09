using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Record {

    public string nombre;
    public int puntos;

    public Record()
    {
    }

    public Record(string nombre, int puntos)
    {
        this.nombre = nombre;
        this.puntos = puntos;
    }
}
