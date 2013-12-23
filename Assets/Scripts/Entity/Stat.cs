using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Stat {
  VIT,  STR,  FORT, AGI,
  INT,  WILL, WIS,  KEEN,
  DRV,  ZEAL, ZEN,  TMPO
};

public enum MapStat {
  HP, STM, FOC, SPR
};

public class MapStats { 
  private int[] values;
  private Stats s;

  public int HP  {set {values[(int) MapStat.HP]  = value;} get {return values[(int) MapStat.HP];}}
  public int STM {set {values[(int) MapStat.STM] = value;} get {return values[(int) MapStat.STM];}}
  public int FOC {set {values[(int) MapStat.FOC] = value;} get {return values[(int) MapStat.FOC];}}
  public int SPR {set {values[(int) MapStat.SPR] = value;} get {return values[(int) MapStat.SPR];}}

  public int maxSTM {get {return (s.VIT * 3) / 2;}}
  public int maxFOC {get {return s.INT * 2;}}
  public int maxSPR {get {return 100;}}

  public int startFOC {get {return s.INT;}}
  public int genSPR {get {return s.DRV;}}

  public int PATK {get {return s.STR;}}
  public int PDEF {get {return s.FORT;}}
  public int PSPD {get {return s.AGI;}}
  public int MATK {get {return s.WILL;}}
  public int MDEF {get {return s.WIS;}}
  public int MSPD {get {return s.KEEN;}}
  public int SATK {get {return s.ZEAL;}}
  public int SDEF {get {return s.ZEN;}}
  public int SSPD {get {return s.TMPO;}}

  public MapStats(Stats baseStats) {
    s = baseStats;
    values = new int[4];
    HP = s.VIT + s.INT + s.DRV;
    STM = maxSTM; 
    FOC = startFOC;
    SPR = 0;
  }

  public int PACC(MapStats other) {
    return PSPD - other.PSPD;
  }
  public int MACC(MapStats other) {
    return MSPD - other.MSPD;
  }
  public int SACC(MapStats other) {
    return SSPD - other.SSPD;
  }
}

public class Stats {
#region Public Variables
  
  private int[] values;

  private static int maxVal = 999;
  private static int minVal = 0;
  private static int numStats = 12;
  public int[] Body {get {return spliced(0, 3);}}
  public int[] Mind {get {return spliced(4, 7);}}
  public int[] Soul {get {return spliced(8, 12);}}
  public int VIT  {set {values[(int) Stat.VIT]  = value;} get {return values[(int) Stat.VIT];}}
  public int STR  {set {values[(int) Stat.STR]  = value;} get {return values[(int) Stat.STR];}}
  public int FORT {set {values[(int) Stat.FORT] = value;} get {return values[(int) Stat.FORT];}}
  public int AGI  {set {values[(int) Stat.AGI]  = value;} get {return values[(int) Stat.AGI];}}
  public int INT  {set {values[(int) Stat.INT]  = value;} get {return values[(int) Stat.INT];}}
  public int WILL {set {values[(int) Stat.WILL] = value;} get {return values[(int) Stat.WILL];}}
  public int WIS  {set {values[(int) Stat.WIS]  = value;} get {return values[(int) Stat.WIS];}}
  public int KEEN {set {values[(int) Stat.KEEN] = value;} get {return values[(int) Stat.KEEN];}}
  public int DRV  {set {values[(int) Stat.DRV]  = value;} get {return values[(int) Stat.DRV];}}
  public int ZEAL {set {values[(int) Stat.ZEAL] = value;} get {return values[(int) Stat.ZEAL];}}
  public int ZEN  {set {values[(int) Stat.ZEN]  = value;} get {return values[(int) Stat.ZEN];}}
  public int TMPO {set {values[(int) Stat.TMPO] = value;} get {return values[(int) Stat.TMPO];}}

#endregion
#region Constructors

  public Stats() {
    values = new int[numStats];
    for (int i = 0; i < numStats; i++) {
      values[i] = minVal;
    }
  }

  public Stats(int[] newvalues) {
    values = newvalues;
  }

  public Stats(string[] newvalues) {
    values = new int[numStats];
    for (int i = 0; i < numStats; i++) {
      values[i] = Convert.ToInt32(newvalues[i]);
    }
  }

  public int Get(Stat s) {
    return values[(int) s];
  }

  public void Set(Stat s, int val) {
    values[(int) s] = clamp(val);
  }

#endregion
#region Private Methods

  private int[] spliced(int i, int j) {
    if (j > 11 || i < 0 || i > j) {
      throw new ArgumentException("Invalid splice range");
    }

    int[] output = new int[j - i + 1];
    for (int idx = i; idx < j; idx++) {
      output[idx - i] = values[idx];
    }
    return output;
  }
  
  private int clamp(int val) {
    if (val < minVal) {
      return 0;
    }
    else if (val > maxVal) {
      return maxVal;
    }
    return val;
  }

#endregion
}
