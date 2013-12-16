using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ST = StatType;

public abstract class Entity {
#region Public Variables


#endregion
#region Private Variables

  private Dictionary<ST, Stat> _stats;

#endregion 
#region Accessors

  public List<Stat> stats {get {return _stats.Values.ToList();}}
  public int GetStat(ST type) {return _stats[type].val;}

#endregion
#region Mutators

  public void SetStat(ST type, int newVal) {_stats[type].val = newVal;}

#endregion
#region Constructors

  public Entity() {
    _stats = new Dictionary<ST, Stat>();
    foreach (ST st in ST.Defaults) {
      _stats.Add(st, new Stat(st));
    }
  }
#endregion 
}
