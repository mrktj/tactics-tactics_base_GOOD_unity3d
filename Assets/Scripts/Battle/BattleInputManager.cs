using UnityEngine;
using System.Collections;

public class BattleInputManager : MonoBehaviour {
  public Vector3 pos;
  public NetworkPlayer theOwner;
  public MouseInput lastInputClient = new MouseInput();
  public MouseInput currentInputServer = new MouseInput();

  void Start() {
  }

  void Awake () {
    if (Network.isClient) enabled=false;
  }
  
	void Update () {
    if ((theOwner != null) && Network.player == theOwner) {
      MouseInput mi = new MouseInput(); 
      if (lastInputClient != mi) {
        lastInputClient = mi;
      }
      if (Network.isServer) {
        SendMovementInput(mi.pos, mi.left, mi.right);
      }
      else if (Network.isClient) {
        networkView.RPC("SendMovementInput", RPCMode.Server, mi.pos, mi.left, mi.right);
      }
    }
    if (Network.isServer) {
    }
  }

  [RPC]
  void SendMovementInput(Vector3 pos, bool left, bool right) {
    currentInputServer = new MouseInput(pos, left, right); 
  }

  void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
    if (stream.isWriting) {
      Vector3 sendpos = pos;
      Debug.Log("send" + " " + sendpos);
      stream.Serialize(ref sendpos);
    }
    else {
      Vector3 posReceive = Vector3.zero;
      stream.Serialize(ref posReceive);
      pos = posReceive;
      Debug.Log(pos);
    }
  }
  
  [RPC]
  public void SetPlayer(NetworkPlayer player) {
    theOwner = player;
    if (player == Network.player)
      enabled = true;
  }

  public class MouseInput {
    public Vector3 pos = Vector3.zero;
    public bool left = false;
    public bool right = false;
    public bool middle = false;

    public MouseInput() {
      Update();
    }

    public MouseInput(Vector3 mpos, bool l, bool r) {
      pos = mpos;
      left = l;
      right = r;
    }
    
    public void Update() {
      pos = Input.mousePosition;
      left = Input.GetMouseButtonUp(0);
      right = Input.GetMouseButtonUp(1);
    }

    public bool Equals(MouseInput mi) {
      if ((object) mi == null) return false;
      return (pos == mi.pos) && (left == mi.left) && (right == mi.right);
    }
    
    public override int GetHashCode() {
      int hash = 17;
      hash = hash * 31 + pos.GetHashCode();
      hash = hash * 31 + left.GetHashCode();
      hash = hash * 31 + right.GetHashCode();
      return hash;
    }
  }
}
