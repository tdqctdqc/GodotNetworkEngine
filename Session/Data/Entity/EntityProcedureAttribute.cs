using Godot;
using System;

public class EntityProcedureAttribute : Attribute
{
    /*
     
    Must have ProcedureWriteKey key as first argument
    
    Must be function, part of entity class
    
    Must only either use ProcedureSet on entities variables, set entityVariables value's fields,
    or call other entities procedures
     
     */
}
