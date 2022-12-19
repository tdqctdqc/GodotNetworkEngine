using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectionExt 
{
    public static List<Type> GetConcreteTypesOfType<TAbstract>(this Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => t.IsInterface == false && t.IsAbstract == false && typeof(TAbstract).IsAssignableFrom(t)).ToList();
    }
    public static List<Type> GetConcreteTypesOfType(this Assembly assembly, Type abstractType)
    {
        return assembly.GetTypes()
            .Where(t => t.IsInterface == false && t.IsAbstract == false && abstractType.IsAssignableFrom(t)).ToList();
    }
}