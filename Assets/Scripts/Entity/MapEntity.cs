using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ST = StatType;

public class MapEntity {
#region Public Variables

  [SerializeField]
  public GameObject gameObject;

#endregion
#region Private Variables

  private Dictionary<ST, Stat> _mapStats;
  private HexCoord _pos;
  private string _name;
  private Map map;

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

  public MapEntity(String newName, HexCoord newPos, Dictionary<ST, Stat> entityStats, Map m) {
    _mapStats = new Dictionary<ST, Stat>();
    foreach (ST st in ST.MapDefaults) {
      _mapStats.Add(st, new Stat(st));
    }
    _name = newName;
    map = m;
    SetMapStat(ST.Health, entityStats[ST.Vit].val);
    SetMapStat(ST.Focus,  entityStats[ST.Int].val);
    SetMapStat(ST.Spirit, entityStats[ST.Soul].val);
    gameObject = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Sprites/MapEntities/" + mapEntityName));
    gameObject.transform.parent = map.transform;
    MoveTo(newPos);
  }

  public void MoveTo(HexCoord h) {
    _pos = h;
    gameObject.transform.localPosition = map.gridDrawer.HexToPixel(_pos);
  }

  public void MoveBy(HexCoord h) {
    MoveTo(_pos + h);
  }

#endregion
}
