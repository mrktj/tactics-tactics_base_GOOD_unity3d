using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HexCoord {
#region Private Variables
  
  // Offset Coordinates
  private int _i;
  private int _j;

  // Cube Coordinates
  private int _x;
  private int _y;
  private int _z;

  // Axial Coordinates
  private int _q;
  private int _r;

#endregion
#region Accessors

  public int i {
    set {
      _i = value;
      q = i - (j - (j&1)) / 2;
    }
    get {
      return _i;
    }
  }

  public int j {
    set {
      _j = value;
      q = i - (j - (j&1)) / 2;
      r = j;
    }
    get {
      return _j;
    }
  }

  public int q {
    set {
      _q = value;
      _x = q;
      _y = -x - z;
      _i = x + (z - (z&1)) / 2;
    }
    get {
      return _q;
    }
  }

  public int r {
    set {
      _r = value;
      _z = r;
      _y = -x - z;
      _i = x + (z - (z&1)) / 2;
      _j = z;
    }
    get {
      return _r;
    }
  }

  public int x {
    set {
      _x = value;
      _q = x;
      _y = -x - z;
      _i = x + (z - (z&1)) / 2;
    }
    get {
      return _x;
    }
  }

  public int y {
    get {
      return _y;
    }
  }

  public int z {
    set { 
      _z = value;
      _r = z;
      _y = -x - z;
      _i = x + (z - (z&1)) / 2;
      _j = z;
    }
    get {
      return _z;
    }
  }

#endregion
#region Constructors

  public HexCoord(int i0, int j0, bool axial = false) {
    if (!axial) {
      i = i0;
      j = j0;
    }
    else {
      q = i0;
      r = j0;
    }
  }

#endregion
#region Public Methods

  public List<HexCoord> Neighbors() {
    List<HexCoord> n = new List<HexCoord>() {
          new HexCoord(q + 1, r    , true),
          new HexCoord(q    , r + 1, true),
          new HexCoord(q - 1, r + 1, true),
          new HexCoord(q - 1, r    , true),
          new HexCoord(q    , r - 1, true),
          new HexCoord(q + 1, r - 1, true)};
    return n;
  }

  public List<HexCoord> Diagonals() {
    List<HexCoord> d = new List<HexCoord>() {
          new HexCoord(q + 1, r + 1, true),
          new HexCoord(q - 1, r + 2, true),
          new HexCoord(q - 2, r + 1, true),
          new HexCoord(q - 1, r - 1, true),
          new HexCoord(q + 1, r - 2, true),
          new HexCoord(q + 2, r - 1, true)};
    return d;
  }

  public int DistanceTo(HexCoord other) {
    return Distance(this, other);
  }

  public static int Distance(HexCoord a, HexCoord b) {
    return (Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z))/2;
  }

#endregion
#region Operator Overloads

  public static HexCoord operator +(HexCoord a, HexCoord b) {
    return new HexCoord(a.i + b.i, a.j + b.j);
  }

  public static HexCoord operator -(HexCoord a, HexCoord b) {
    return new HexCoord(a.i - b.i, a.j - b.j);
  }

  public static bool operator ==(HexCoord a, HexCoord b) {
    if ((System.Object) a != null && (System.Object) b != null) {
      return (a.i == b.i) && (a.j == b.j);
    }
    else return ((System.Object) a == null && (System.Object) b == null);
  }

  public static bool operator !=(HexCoord a, HexCoord b) {
    return !(a == b);
  }

  public override bool Equals(System.Object obj) {
    if (obj == null || ! (obj is HexCoord)) return false;
    else return this == (HexCoord) obj;
  }

  public override int GetHashCode() {
    int hash = 17;
    hash = hash * 23 + i.GetHashCode();
    hash = hash * 23 + j.GetHashCode();
    return hash;
  }

  public override string ToString() {
    return "HexCoord: " + i + " "+ j;
  }

#endregion
}
