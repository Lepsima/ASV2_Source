using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct ForceAccel2 : IAutoUnit2 {
    public float x, y;
    
    public ForceAccel2(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public ForceAccel2(Vector2 v2) {
        x = v2.x;
        y = v2.y;
    }

    public static implicit operator Vector2(ForceAccel2 v) => new Vector2(v.x, v.y);
    public ForceAccel magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));

    public Vector2 ToNewtonSecond() => new Vector2(x * 1f, y * 1f);
    public static ForceAccel2 NewtonSecond(float x, float y) => new ForceAccel2(x * 1f, y * 1f);
    public static ForceAccel2 NewtonSecond(Vector2 v) => new ForceAccel2(v.x * 1f, v.y * 1f);

    public static ForceAccel2 operator +(ForceAccel2 a, ForceAccel2 b) => new(a.x + b.x, a.y + b.y);
    public static ForceAccel2 operator -(ForceAccel2 a, ForceAccel2 b) => new(a.x - b.x, a.y - b.y);
    public static ForceAccel2 operator *(ForceAccel2 a, ForceAccel2 b) => new(a.x * b.x, a.y * b.y);
    public static ForceAccel2 operator /(ForceAccel2 a, ForceAccel2 b) => new(a.x / b.x, a.y / b.y);
    public Force2 Force2(Time v) => new(v * x, v * y);
    public Time Time(Force2 v) => new(v.magnitude / magnitude);
    public ForceAccel2(Force2 a, Time b) => a.ForceAccel2(b);
    public ForceAccel2(Time b, Force2 a) => a.ForceAccel2(b);
}
}
