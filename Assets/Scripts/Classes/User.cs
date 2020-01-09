using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User {

    public User()
    {
        this.Name = "";
        this.Id = -1;
        this.Record = 0;
        this.PlayerColor = Color.white;
    }

    public User(string userName)
    {
        this.Name = userName;
        this.Id = -1;
        this.Record = 0;
        this.PlayerColor = Color.white;
    }

    public User(string userName, long id)
    {
        this.Name = userName;
        this.Id = id;
        this.Record = 0;
        this.PlayerColor = Color.white;
    }

    public User(string userName, long id, int record, Color color)
    {
        this.Name = userName;
        this.Id = id;
        this.Record = record;
        this.PlayerColor = color;
    }

    public string Name { get; set; }
    public long Id { get; set; }
    public int Record { get; set; }
    public Color PlayerColor { get; set; }
}
