using UnityEngine;
using System;
using System.Collections;

public class NetworkingManager : MonoBehaviour {

  public BattleManager battleManager;
  public string connectionIP = "127.0.0.1";
  public int connectionPort = 25001;

	void OnServerInitialized () {
    Debug.Log("ServerInitialized");
	}

  void OnPlayerConnected (NetworkPlayer player) {
    Debug.Log("ClientConnected");
    NetworkView theNetworkView = battleManager.gameObject.networkView;
    theNetworkView.RPC("setOwner", RPCMode.AllBuffered, player);
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

  void OnGUI() {
    if (Network.peerType == NetworkPeerType.Disconnected) {
      GUI.Label(new Rect(10, 10, 200, 20), "Status: Disconnected");
      if (GUI.Button(new Rect(10, 30, 120, 20), "Client Connect")) {
        Network.Connect(connectionIP, connectionPort);
        battleManager.LoadBattle();
      }
      if (GUI.Button(new Rect(10, 50, 120, 20), "Initialize Server")) {
        Network.InitializeServer(32, connectionPort, false);
        battleManager.LoadBattle();
      }
      if (GUI.Button(new Rect(10, 70, 120, 20), "Local Game")) {
        battleManager.LoadBattle();
      }
    }
    else if (Network.peerType == NetworkPeerType.Client) {
      GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client");
      if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect")) {
        Network.Disconnect(200);
      }
    }
  }
}
