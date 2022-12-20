using Godot;
using System;

public sealed class Player : Entity
{
    public EntityString Name { get; set; }
    public Player(int id, string name, HostWriteKey key) : base(id, key)
    {
        Name = EntityString.Construct(name, this, nameof(Name));
    }

    private static Player DeserializeConstructor(string json)
    {
        GD.Print("calling deserialize constructor");

        return new Player(json);
    }
    private Player(string json) : base(json) { }
}
