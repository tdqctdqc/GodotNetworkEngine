using Godot;
using System;
using System.Collections.Generic;

public class BaseDomain : Domain
{
    public Repository<Player> Players => GetRepo<Player>();
    public BaseDomain() : base()
    {
        _repos.Add(typeof(Player), new Repository<Player>());
    }
}
