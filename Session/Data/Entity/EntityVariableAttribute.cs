using Godot;
using System;

public class EntityVariableAttribute : Attribute
{
    public static string SerializeName => "Serialize";
    public static string DeserializeName => "Deserialize";
    /*
    Must implement
    public static TVariable<TValue> Construct(TValue value, Entity entity, string name)
    public void Update(StrongWriteKey key, TValue newValue, IRepo repo, IServer server)
    public void ProcedureSet(ProcedureWriteKey key, TValue newValue)
    void ProcedureSetField(ProcedureWriteKey key, object newFieldValue, string fieldName)
    
    //TODO make these require writeKey again?
    public static string Serialize(TVariable<TValue> es)
    public static TVariable<TValue> Deserialize(string json, string name, Entity entity)
    
    Shouldn't set TValue from outside the constructor, Update method, or procedure set method
    Shouldn't set fields of the TValue outside an entity procedure 
    Procedure set method should only be called inside entity procedures
    */
}
