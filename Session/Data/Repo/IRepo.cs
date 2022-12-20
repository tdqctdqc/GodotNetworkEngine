using Godot;
using System;

public interface IRepo
{
    Domain Domain { get; }
    void AddEntity(Entity e, StrongWriteKey key);
    void RemoveEntity(Entity e, StrongWriteKey key);
    void RaiseValueChangedNotice(string valueName, int id, WriteKey key);
}
