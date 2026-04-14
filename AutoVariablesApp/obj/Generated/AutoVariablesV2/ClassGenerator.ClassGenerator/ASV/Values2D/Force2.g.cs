using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Force2 : IAutoUnit2 {
    public float x, y;
    
    public Force2(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Force2(Vector2 v2) {
        x = v2.x;
        y = v2.y;
    }

    public static implicit operator Vector2(Force2 v) => new Vector2(v.x, v.y);
    public Force magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));

    public Vector2 ToMillinewton() => new Vector2(x * 1000f, y * 1000f);
    public static Force2 Millinewton(float x, float y) => new Force2(x * 0.001f, y * 0.001f);
    public static Force2 Millinewton(Vector2 v) => new Force2(v.x * 0.001f, v.y * 0.001f);

    public Vector2 ToNewton() => new Vector2(x * 1f, y * 1f);
    public static Force2 Newton(float x, float y) => new Force2(x * 1f, y * 1f);
    public static Force2 Newton(Vector2 v) => new Force2(v.x * 1f, v.y * 1f);

    public Vector2 ToKilonewton() => new Vector2(x * 0.001f, y * 0.001f);
    public static Force2 Kilonewton(float x, float y) => new Force2(x * 1000f, y * 1000f);
    public static Force2 Kilonewton(Vector2 v) => new Force2(v.x * 1000f, v.y * 1000f);

    public Vector2 ToMeganewton() => new Vector2(x * 1E-06f, y * 1E-06f);
    public static Force2 Meganewton(float x, float y) => new Force2(x * 1000000f, y * 1000000f);
    public static Force2 Meganewton(Vector2 v) => new Force2(v.x * 1000000f, v.y * 1000000f);

    public static Force2 operator +(Force2 a, Force2 b) => new(a.x + b.x, a.y + b.y);
    public static Force2 operator -(Force2 a, Force2 b) => new(a.x - b.x, a.y - b.y);
    public static Force2 operator *(Force2 a, Force2 b) => new(a.x * b.x, a.y * b.y);
    public static Force2 operator /(Force2 a, Force2 b) => new(a.x / b.x, a.y / b.y);
    public Accel2 Accel2(Mass v) => new(x / v, y / v);
    public Mass Mass(Accel2 v) => new(magnitude / v.magnitude);
    public Force2(Accel2 a, Mass b) => a.Force2(b);
    public Force2(Mass b, Accel2 a) => a.Force2(b);

    public static Force2 operator +(ForceAccel2 a, Force2 b) => b + a.Force2(VTime.deltaTime);
    public static Force2 operator +(Force2 b, ForceAccel2 a) => b + a.Force2(VTime.deltaTime);
    public static Force2 operator -(ForceAccel2 a, Force2 b) => a.Force2(VTime.deltaTime) - b;
    public static Force2 operator -(Force2 b, ForceAccel2 a) => b - a.Force2(VTime.deltaTime);
    
    public ForceAccel2 ForceAccel2(Time v) => new(x / v, y / v);
    public Time Time(ForceAccel2 v) => new(magnitude / v.magnitude);
    public Force2(ForceAccel2 a, Time b) => a.Force2(b);
    public Force2(Time b, ForceAccel2 a) => a.Force2(b);
}
}
