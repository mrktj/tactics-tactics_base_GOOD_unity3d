using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StatType {
  private int _min;
  private int _max;

  public int min {get {return _min;}}
  public int max {get {return _max;}}

  public StatType() {
    _min = 0;
    _max = 0;
  }

  public StatType(int minVal, int maxVal) {
    if (minVal > maxVal) {
      throw new System.ArgumentException("minVal must be less than maxVal");
    }
    _min = minVal;
    _max = maxVal;
  }

  public static StatType Vit  = new StatType(1, 9999);
  public static StatType Int  = new StatType(1, 9999);
  public static StatType Soul = new StatType(1, 9999);

  public static StatType Health = new StatType(1, 999);
  public static StatType Focus  = new StatType(1, 999);
  public static StatType Spirit = new StatType(1, 999);

  public static List<StatType> Defaults {
    get {
      return new List<StatType> {
        Vit, Int, Soul
      };
    }
  }

  public static List<StatType> MapDefaults {
    get {
      return new List<StatType> {
        Health, Focus, Spirit
      };
    }
  }
}

[Serializable()]
public class StatTypeException : Exception {
  public StatTypeException() : base() {}
  public StatTypeException(string message) : base(message) {}
  public StatTypeException(string message, System.Exception inner) : base(message, inner) {}
  
  protected StatTypeException(System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) { }
}

public class Stat {
#region Private Variables

  private int _val;
  private StatType _type;

#endregion
#region Accessors

  public StatType type {get {return _type;}}
  public int val {get {return _val;} set {_val = val; clamp();}}

#endregion
#region Constructors

  public Stat() {
    val = 0;
    _type = new StatType();
  }

  public Stat(StatType t) {
    _type = t;
    val = t.min;
  }

  public Stat(StatType t, int newVal) : this(t) {
    val = newVal;
  }

#endregion
#region Public Methods

  public void SetVal(int newVal) {
    val = newVal;
  }

  public void Increment() {
    val += 1;
  }

  public void Decrement() {
    val -= 1;
  }

  public void Clear() {
    val = type.min;
  }

#endregion
#region Private Methods

  private void clamp() {
    if (val < type.min) {
      _val = type.min;
    }
    else if (val > type.max) {
      _val = type.max;
    }
  }

#endregion
#region Operator Overloads

  private static void CheckArithmeticError(Stat s1, Stat s2) {
    if (s1.type != s2.type) {
      throw new StatTypeException("Cannot perform arithmetic on Stats with different StatType");
    }
  }

  private static void CheckComparisonError(Stat s1, Stat s2) {
    if (s1.type != s2.type) {
      throw new StatTypeException("Cannot compare Stats with different StatType");
    }
  }

  public static Stat operator +(Stat s1, Stat s2) {
    CheckArithmeticError(s1, s2);
    return new Stat(s1.type, s1.val + s2.val);
  }

  public static Stat operator -(Stat s1, Stat s2) {
    CheckArithmeticError(s1, s2);
    return new Stat(s1.type, s1.val - s2.val);
  }

  public static Stat operator *(Stat s1, Stat s2) {
    CheckArithmeticError(s1, s2);
    return new Stat(s1.type, s1.val * s2.val);
  }

  public static Stat operator /(Stat s1, Stat s2) {
    CheckArithmeticError(s1, s2);
    return new Stat(s1.type, s1.val / s2.val);
  }

  public static bool operator ==(Stat s1, Stat s2) {
    CheckComparisonError(s1, s2);
    return s1.val == s2.val;
  }

  public static bool operator !=(Stat s1, Stat s2) {
    return !(s1 == s2);
  }

  public static bool operator >(Stat s1, Stat s2) {
    CheckComparisonError(s1, s2);
    return s1.val > s2.val;
  }

  public static bool operator <(Stat s1, Stat s2) {
    CheckComparisonError(s1, s2);
    return s1.val < s2.val;
  }

  public static bool operator <=(Stat s1, Stat s2) {
    return !(s1 > s2);
  }

  public static bool operator >=(Stat s1, Stat s2) {
    return !(s1 < s2);
  }

  public override bool Equals(System.Object obj) {
    if (obj == null || ! (obj is Stat)) return false;
    else return this == (Stat) obj;
  }

  public override int GetHashCode() {
    int hash = 17;
    hash = hash * 23 + val.GetHashCode();
    return hash;
  }

#endregion
}
