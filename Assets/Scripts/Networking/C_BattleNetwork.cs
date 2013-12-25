using UnityEngine;
using System.Collections;

public class C_BattleNetwork : MonoBehaviour {

  public BattleManager battleManager;
  private NetworkPlayer owner;

  [RPC]
  public void setOwner(NetworkPlayer player) {
    owner = player;
    if (player == Network.player) {
      enabled=true;
    }
  }

  [RPC]
  public NetworkPlayer getOwner() {
    return owner;
  }

  void Awake() {
    if (Network.isClient) {
      enabled = false;
    }
  }

  void Update() {
    if (Network.isServer) return;
    if ((owner != null) && Network.player == owner) {
      int[] data = battleManager.CursorHandler(Input.mousePosition, Input.GetMouseButtonUp(0), Input.GetMouseButtonUp(1));
      if (data != null) {
        networkView.RPC("updateClientInput", RPCMode.Server, data[0], data[1], data[2]);
      }
    }
  }
}
