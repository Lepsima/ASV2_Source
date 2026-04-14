using System;
using AutoVariablesApp;
namespace Generated.Units {
[System.Serializable]
public struct Mass_UI : IAutoUnitUI {
    public float x;
    public Mass_UIType type;
    
    public Mass Value => this;
    
    public Mass_UI(float x) {
        this.x = x;
    }
    
    public static implicit operator float(Mass_UI v) => v.x;
    public static implicit operator Mass(Mass_UI v) => new(v.x);
    public Mass magnitude => new(x);
}

public enum Mass_UIType {
    [InspectorName("g")]
    Gram,
    [InspectorName("kg")]
    Kilogram,
    [InspectorName("t")]
    Ton,

}
}
