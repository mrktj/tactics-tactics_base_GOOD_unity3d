using UnityEngine;
using System.Collections;

public class Box : Entity {
  public Box(int Vit) : base(){
    SetStat(StatType.Vit, Vit);
  }
}
