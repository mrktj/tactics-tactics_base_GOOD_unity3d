using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ST = StatType;

[Serializable]
public class MapEntity : ScriptableObject{
#region Public Variables

#endregion
#region Private Variables

  [SerializeField]
  private Dictionary<ST, Stat> _mapStats;
  [SerializeField]
  private HexCoord _pos;
  [SerializeField]
  private string _name;

#endregion
#region Accessors

  public List<Stat> mapStats {get {return _mapStats.Values.ToList();}}
  public int GetMapStat(ST type) {return _mapStats[type].val;}
  public HexCoord pos {get {return _pos;}}
  public String mapEntityName {get {return _name;}}

#endregion
#region

  public void SetMapStat(ST type, int newVal) {_mapStats[type].val = newVal;}

#endregion
#region

  void OnEnable() {
    _name = "";
    _pos = new HexCoord(0, 0);
    _mapStats = new Dictionary<ST, Stat>();
    foreach (ST st in ST.MapDefaults) {
      _mapStats.Add(st, new Stat(st));
    }
  }

  public void Init(String newName, HexCoord newPos) {
    _name = newName;
    MoveTo(newPos);
  }

  public void MoveTo(HexCoord h) {
    _pos = h;
  }

  public void MoveBy(HexCoord h) {
    _pos += h;
  }

#endregion
}
