using UnityEngine;
using System.Collections;

public class UnitNetwork : MonoBehaviour {

  public MapEntity mapEntity;

	// Use this for initialization
	void Start () {
	}

  void OnPlayerConnected() {
  }

  [RPC]
  void SetID(NetworkViewID newID) {
    gameObject.GetComponent<NetworkView>().viewID = newID;
  }
	
	// Update is called once per frame
	void Update () {
	}

  void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
    if (stream.isWriting) {
      HexCoord h = mapEntity.pos;
      int i = h.i;
      int j = h.j;
      stream.Serialize(ref i);
      stream.Serialize(ref j);
    }
    else { 
      int i = -1;
      int j = -1;
      stream.Serialize(ref i);
      stream.Serialize(ref j);
      mapEntity.MoveTo(new HexCoord(i, j));
    }
  }
}
