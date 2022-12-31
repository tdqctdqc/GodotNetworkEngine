using Godot;
using System;

[EntityProcedure]
public abstract class Procedure<T> where T : Entity
{
    protected static ProcedureWriteKey Key = new ProcedureWriteKey();
}

public class Proc : Procedure<Player>
{
    public static object GetArgs(Player t)
    {
        return new ProcArgs(1, "doot");
    }
    public static void EnactAndPushToServer(HostWriteKey key, object args)
    {
        Enact(Key, args);
        //TODO send update to server
    }
    public static void Enact(ProcedureWriteKey key, object args)
    {
        var pArgs = (ProcArgs) args;
    }
    private class ProcArgs
    {
        public readonly int IntField;
        public readonly int PlayerId;
        public readonly string StringField;

        public ProcArgs(int intField, string stringField)
        {
            IntField = intField;
            StringField = stringField;
        }
    }
}
