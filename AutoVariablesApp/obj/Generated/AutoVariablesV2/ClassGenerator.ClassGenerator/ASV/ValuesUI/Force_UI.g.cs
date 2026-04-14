using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct Force_UI : IAutoUnitUI {
    public float x;
    public Force_UIType type;
    
    public Force Value => this;
    
    public Force_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(Force_UI v) => v.x;
    public static implicit operator Force(Force_UI v) => new(v.x);
    public Force magnitude => new(x);
}

[System.Serializable]
public struct Force2_UI : IAutoUnitUI2 {
    public float x, y;
    public Force_UIType type;
    
    public Force2 Value => this;
    
    public Force2_UI(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Force2_UI(Vector2 v2) {
      x = v2.x;
      y = v2.y;
    }
  
    public static implicit operator Vector2(Force2_UI v) => new(v.x, v.y);
    public static implicit operator Force2(Force2_UI v) => new(v.x, v.y);
    public Force magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
}

[System.Serializable]
public struct Force3_UI : IAutoUnitUI3 {
    public float x, y, z;
    public Force_UIType type;
    
    public Force3 Value => this;
    
    public Force3_UI(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Force3_UI(Vector3 v3) {
      x = v3.x;
      y = v3.y;
      z = v3.z;
    }

    public static implicit operator Vector3(Force3_UI v) => new(v.x, v.y, v.z);
    public static implicit operator Force3(Force3_UI v) => new(v.x, v.y, v.z);
    public Force magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
}

public enum Force_UIType {
    [InspectorName("N")]
    Newton,
    [InspectorName("kN")]
    Kilonewton,
    [InspectorName("MN")]
    Meganewton,

}
}
