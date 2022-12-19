using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class EntityRefCollection<T> where T : Entity
{
    public IReadOnlyCollection<int> Ids { get; private set; }
    private HashSet<int> _col;

    public EntityRefCollection(IReadOnlyCollection<int> ids)
    {
        _col = ids.ToHashSet();
        Ids = _col;
    }
    public IEnumerable<T> GetEnumerable()
    {
        _col.RemoveWhere(id => (T) Game.I.Session.Data[id] == null);
        return _col.Select(id => (T)Game.I.Session.Data[id]);
    }
}
