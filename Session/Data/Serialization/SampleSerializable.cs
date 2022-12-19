using Godot;
using System;

public abstract class SampleSerializable : Serializable
{
    public string Field => Get<string>(nameof(Field));
    protected SampleSerializable(string field, HostWriteKey key) : base()
    {
        //when initializing should use SetInner
        //Entity.Set is for updating existing entities and sending update if host
        //Non-entity serializables will have own methods of updating
        SetInner<string>(field, nameof(Field), key);
    }

    
    // private static SampleEntity DeserializeConstructor(WriteKey key, string json)
    // {
    //     return new SampleEntity(key, json);
    // }
    
    //write key is FIRST here to prevent confusion with a possible constructor initializing a string field
    protected SampleSerializable(StrongWriteKey key, string json) : base(key, json) { }
}