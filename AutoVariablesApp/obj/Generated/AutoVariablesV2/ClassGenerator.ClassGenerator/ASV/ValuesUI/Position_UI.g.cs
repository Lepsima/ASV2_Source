using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct Position_UI : IAutoUnitUI {
    public float x;
    public Position_UIType type;
    
    public Position Value => this;
    
    public Position_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(Position_UI v) => v.x;
    public static implicit operator Position(Position_UI v) => new(v.x);
    public Position magnitude => new(x);
}

[System.Serializable]
public struct Position2_UI : IAutoUnitUI2 {
    public float x, y;
    public Position_UIType type;
    
    public Position2 Value => this;
    
    public Position2_UI(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Position2_UI(Vector2 v2) {
      x = v2.x;
      y = v2.y;
    }
  
    public static implicit operator Vector2(Position2_UI v) => new(v.x, v.y);
    public static implicit operator Position2(Position2_UI v) => new(v.x, v.y);
    public Position magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
}

[System.Serializable]
public struct Position3_UI : IAutoUnitUI3 {
    public float x, y, z;
    public Position_UIType type;
    
    public Position3 Value => this;
    
    public Position3_UI(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Position3_UI(Vector3 v3) {
      x = v3.x;
      y = v3.y;
      z = v3.z;
    }

    public static implicit operator Vector3(Position3_UI v) => new(v.x, v.y, v.z);
    public static implicit operator Position3(Position3_UI v) => new(v.x, v.y, v.z);
    public Position magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
}

public enum Position_UIType {
    [InspectorName("ft")]
    Feet,
    [InspectorName("mi")]
    Mile,
    [InspectorName("cm")]
    Centimeter,
    [InspectorName("m")]
    Meter,
    [InspectorName("km")]
    Kilometer,

}
}
