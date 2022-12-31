using Godot;
using System;
using System.Collections.Generic;

public class ClientServer : Node, IServer
{
    private ServerWriteKey _key = new ServerWriteKey();
    [Remote] public void ReceiveUpdates(string updatesJson, string updateTypesJson)
    {
        var updatesJsonsList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(updatesJson);
        var updateTypes = System.Text.Json.JsonSerializer.Deserialize<List<string>>(updateTypesJson);

        for (int i = 0; i < updatesJsonsList.Count; i++)
        {
            var updateType = updateTypes[i];
            if (updateType == EntityVarUpdate.UpdateType)
            {
                EntityVarUpdate.DeserializeAndEnact(updatesJsonsList[i], _key);
            }
            else
            {
                //is procedure
            }
        }
    }

    
}
