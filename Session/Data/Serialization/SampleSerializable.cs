using Godot;
using System;

public abstract class SampleSerializable : Serializable
{
    protected SampleSerializable(string field, HostWriteKey key) : base()
    {
        //if entity should initialize entityVariables here from arguments
    }

    
    // private static SampleEntity DeserializeConstructor(string json)
    // {
    //     return new SampleEntity(json);
    // }
    //this is constructor for deserialization
    protected SampleSerializable(string json) : base(json) { }
}