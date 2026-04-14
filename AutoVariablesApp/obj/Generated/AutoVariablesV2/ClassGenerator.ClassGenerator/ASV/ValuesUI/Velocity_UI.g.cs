using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct Velocity_UI : IAutoUnitUI {
    public float x;
    public Velocity_UIType type;
    
    public Velocity Value => this;
    
    public Velocity_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(Velocity_UI v) => v.x;
    public static implicit operator Velocity(Velocity_UI v) => new(v.x);
    public Velocity magnitude => new(x);
}

[System.Serializable]
public struct Velocity2_UI : IAutoUnitUI2 {
    public float x, y;
    public Velocity_UIType type;
    
    public Velocity2 Value => this;
    
    public Velocity2_UI(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Velocity2_UI(Vector2 v2) {
      x = v2.x;
      y = v2.y;
    }
  
    public static implicit operator Vector2(Velocity2_UI v) => new(v.x, v.y);
    public static implicit operator Velocity2(Velocity2_UI v) => new(v.x, v.y);
    public Velocity magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
}

[System.Serializable]
public struct Velocity3_UI : IAutoUnitUI3 {
    public float x, y, z;
    public Velocity_UIType type;
    
    public Velocity3 Value => this;
    
    public Velocity3_UI(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Velocity3_UI(Vector3 v3) {
      x = v3.x;
      y = v3.y;
      z = v3.z;
    }

    public static implicit operator Vector3(Velocity3_UI v) => new(v.x, v.y, v.z);
    public static implicit operator Velocity3(Velocity3_UI v) => new(v.x, v.y, v.z);
    public Velocity magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
}

public enum Velocity_UIType {
    [InspectorName("m/s")]
    MeterSecond,

}
}
