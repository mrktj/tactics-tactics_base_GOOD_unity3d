using UnityEngine;
using System.Collections;

public class BattleNetwork : MonoBehaviour {

  public BattleManager battleManager;
  private int[] data;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if (Network.isClient) {
      return;
    }
    battleManager.DoMove(data);
    data = null;
	}

  [RPC]
  public void updateClientInput(int entityIdx, int x, int y) {
    data = new int[3] {entityIdx, x, y};
  }
}
