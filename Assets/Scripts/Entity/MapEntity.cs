using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapEntity {
#region Public Variables

  [SerializeField]
  public GameObject gameObject;

#endregion
#region Private Variables

  private HexCoord _pos;
  private string _path;
  private int _idx;
  private Map map;
  private MapStats mstats;

#endregion
#region Accessors

  public int idx {get {return _idx;}}
  public HexCoord pos {get {return _pos;}}
  public String path {get {return _path;}}
  private static string baseDirectory = "MapEntities/Enemies/";

#endregion
#region
#endregion
#region

  public MapEntity(String newpath, HexCoord newPos, Stats entityStats, Map m, bool friendly) {
    _path = newpath;
    mstats = new MapStats(entityStats);
    map = m;
    _idx = map.mapEntities.Count;
    gameObject = (GameObject) UnityEngine.Object.Instantiate(Resources.Load(baseDirectory + path));
    gameObject.transform.parent = map.transform;
    gameObject.renderer.sortingLayerName = "AboveMap";
    gameObject.name = _path + map.mapEntities.Count;
    gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("friendly", friendly);
    gameObject.GetComponent<UnitNetwork>().mapEntity = this;
    MoveTo(newPos);

    if (Network.isServer) {
      NetworkViewID newid = Network.AllocateViewID();
      NetworkView network = gameObject.GetComponent<NetworkView>();
      network.RPC("SetID", RPCMode.AllBuffered, newid);
    }
  }

  public void MoveTo(HexCoord h) {
    if (h == null) {
      throw new ArgumentException("was passed a null parameter");
    }
    _pos = h;
    gameObject.transform.position = map.gridDrawer.HexToPixel(_pos);
  }

  public void MoveBy(HexCoord h) {
    if (h == null) {
      throw new ArgumentException("was passed a null parameter");
    }
    MoveTo(_pos + h);
  }

#endregion
}
