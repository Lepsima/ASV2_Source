using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct Time_UI : IAutoUnitUI {
    public float x;
    public Time_UIType type;
    
    public Time Value => this;
    
    public Time_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(Time_UI v) => v.x;
    public static implicit operator Time(Time_UI v) => new(v.x);
    public Time magnitude => new(x);
}

public enum Time_UIType {
    [InspectorName("ms")]
    Millisecond,
    [InspectorName("s")]
    Second,
    [InspectorName("m")]
    Minute,
    [InspectorName("h")]
    Hour,

}
}
