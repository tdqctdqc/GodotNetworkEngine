using Godot;
using System;

public class UserCredential
{
    public string Name { get; private set; }
    public Guid Guid { get; private set; }
    public string Password { get; private set; }
}
