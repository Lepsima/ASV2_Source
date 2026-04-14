using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct ForceAccel_UI : IAutoUnitUI {
    public float x;
    public ForceAccel_UIType type;
    
    public ForceAccel Value => this;
    
    public ForceAccel_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(ForceAccel_UI v) => v.x;
    public static implicit operator ForceAccel(ForceAccel_UI v) => new(v.x);
    public ForceAccel magnitude => new(x);
}

[System.Serializable]
public struct ForceAccel2_UI : IAutoUnitUI2 {
    public float x, y;
    public ForceAccel_UIType type;
    
    public ForceAccel2 Value => this;
    
    public ForceAccel2_UI(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public ForceAccel2_UI(Vector2 v2) {
      x = v2.x;
      y = v2.y;
    }
  
    public static implicit operator Vector2(ForceAccel2_UI v) => new(v.x, v.y);
    public static implicit operator ForceAccel2(ForceAccel2_UI v) => new(v.x, v.y);
    public ForceAccel magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
}

[System.Serializable]
public struct ForceAccel3_UI : IAutoUnitUI3 {
    public float x, y, z;
    public ForceAccel_UIType type;
    
    public ForceAccel3 Value => this;
    
    public ForceAccel3_UI(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public ForceAccel3_UI(Vector3 v3) {
      x = v3.x;
      y = v3.y;
      z = v3.z;
    }

    public static implicit operator Vector3(ForceAccel3_UI v) => new(v.x, v.y, v.z);
    public static implicit operator ForceAccel3(ForceAccel3_UI v) => new(v.x, v.y, v.z);
    public ForceAccel magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
}

public enum ForceAccel_UIType {
    [InspectorName("N/s")]
    NewtonSecond,

}
}
