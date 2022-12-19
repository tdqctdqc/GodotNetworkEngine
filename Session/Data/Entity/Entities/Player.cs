using Godot;
using System;

public class Player : Entity
{
    public string Name => Get<string>(nameof(Name));
    public EntityRef<Player> Enemy => Get<EntityRef<Player>>(nameof(Enemy));

    public Player(int id, string name, int enemy, HostWriteKey key) : base(id, key)
    {
        SetInner<string>(name, nameof(Name), key);
        SetInner(new EntityRef<Player>(enemy), nameof(Enemy), key);
    }

    private static Player DeserializeConstructor(StrongWriteKey key, string json)
    {
        return new Player(key, json);
    }
    private Player(StrongWriteKey key, string json) : base(key, json) { }
}
