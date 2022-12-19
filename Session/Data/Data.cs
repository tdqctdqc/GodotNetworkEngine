using Godot;
using System;
using System.Collections.Generic;

public class Data
{
    public IReadOnlyDictionary<Type, Domain> Domains => _domains;
    private Dictionary<Type, Domain> _domains;
    private Dictionary<int, Entity> _entities;
    public Entity this[int id] => _entities.ContainsKey(id) ? _entities[id] : null;
    public BaseDomain BaseDomain => GetDomain<BaseDomain>();

    public Data()
    {
        _domains = new Dictionary<Type, Domain>();
        _domains.Add(typeof(BaseDomain), new BaseDomain());
    }

    public void AddEntity(Entity e, Type domainType, StrongWriteKey key)
    {
        _entities.Add(e.Id, e);
        _domains[domainType].Repos[e.GetType()].AddEntity(e, key);
    }

    public void RemoveEntity(Entity e, Type domainType, StrongWriteKey key)
    {
        _domains[domainType].Repos[e.GetType()].RemoveEntity(e, key);
        _entities.Remove(e.Id);
    }
    public T GetDomain<T>() where T : Domain
    {
        return (T) _domains[typeof(T)];
    }

    public Domain GetDomain(Type domainType)
    {
        return _domains[domainType];
    }

    public T GetEntity<T>(int id) where T : Entity
    {
        return (T) _entities[id];
    }
}
