using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct Accel_UI : IAutoUnitUI {
    public float x;
    public Accel_UIType type;
    
    public Accel Value => this;
    
    public Accel_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(Accel_UI v) => v.x;
    public static implicit operator Accel(Accel_UI v) => new(v.x);
    public Accel magnitude => new(x);
}

[System.Serializable]
public struct Accel2_UI : IAutoUnitUI2 {
    public float x, y;
    public Accel_UIType type;
    
    public Accel2 Value => this;
    
    public Accel2_UI(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Accel2_UI(Vector2 v2) {
      x = v2.x;
      y = v2.y;
    }
  
    public static implicit operator Vector2(Accel2_UI v) => new(v.x, v.y);
    public static implicit operator Accel2(Accel2_UI v) => new(v.x, v.y);
    public Accel magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
}

[System.Serializable]
public struct Accel3_UI : IAutoUnitUI3 {
    public float x, y, z;
    public Accel_UIType type;
    
    public Accel3 Value => this;
    
    public Accel3_UI(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Accel3_UI(Vector3 v3) {
      x = v3.x;
      y = v3.y;
      z = v3.z;
    }

    public static implicit operator Vector3(Accel3_UI v) => new(v.x, v.y, v.z);
    public static implicit operator Accel3(Accel3_UI v) => new(v.x, v.y, v.z);
    public Accel magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
}

public enum Accel_UIType {
    [InspectorName("m/s2")]
    MeterSecondSecond,

}
}
