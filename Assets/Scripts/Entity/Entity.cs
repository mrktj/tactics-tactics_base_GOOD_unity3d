using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ST = StatType;

public abstract class Entity : ScriptableObject{
#region Public Variables
#endregion
#region Private Variables

  private Dictionary<ST, Stat> _stats;
  private String _name;

#endregion 
#region Accessors

  public List<Stat> stats {get {return _stats.Values.ToList();}}
  public int GetStat(ST type) {return _stats[type].val;}
  public String entityName {get {return _name;}}

#endregion
#region Mutators

  public void SetStat(ST type, int newVal) {_stats[type].val = newVal;}

#endregion
#region Constructors

  public virtual void OnEnable() {
    _name = "";
    _stats = new Dictionary<ST, Stat>();
    foreach (ST st in ST.Defaults) {
      _stats.Add(st, new Stat(st));
    }
  }

  public virtual void Init(string s) {
    _name = s;
  }

  public string GetMapEntityName() { 
    return entityName;
  }

  public MapEntity CreateMapEntity(HexCoord initPos, Map m) {
    return new MapEntity(GetMapEntityName(), initPos, _stats, m);
  }

#endregion 
}
