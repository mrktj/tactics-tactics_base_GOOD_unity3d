using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Entity {
#region Public Variables
#endregion
#region Private Variables

  private Stats _stats;
  private String _path;

#endregion 
#region Accessors

  public Stats stats {get {return _stats;}}
  public String path {get {return _path;}}
  public String mapPath {get {return _path;}}

#endregion
#region Constructors


  public Entity() {
    _path = "";
    _stats = new Stats();
  }

  public Entity(string s) {
    _path = s;
    _stats = new Stats();
  }

  public Entity(Stats s) {
    _path = "";
    _stats = s;
  }

  public Entity(string p, Stats s) {
    _path = p;
    _stats = s;
  }

#endregion
#region Public Methods

  public MapEntity CreateMapEntity(HexCoord initPos, Map m, bool friendly) {
    return new MapEntity(mapPath, initPos, stats, m, friendly);
  }

#endregion 
}
