using Godot;
using System;

public interface IRepo
{
    void AddEntity(Entity e, StrongWriteKey key);
    void RemoveEntity(Entity e, StrongWriteKey key);
}
