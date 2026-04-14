using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Accel2 : IAutoUnit2 {
    public float x, y;
    
    public Accel2(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Accel2(Vector2 v2) {
        x = v2.x;
        y = v2.y;
    }

    public static implicit operator Vector2(Accel2 v) => new Vector2(v.x, v.y);
    public Accel magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));

    public Vector2 ToMeterSecondSecond() => new Vector2(x * 1f, y * 1f);
    public static Accel2 MeterSecondSecond(float x, float y) => new Accel2(x * 1f, y * 1f);
    public static Accel2 MeterSecondSecond(Vector2 v) => new Accel2(v.x * 1f, v.y * 1f);

    public static Accel2 operator +(Accel2 a, Accel2 b) => new(a.x + b.x, a.y + b.y);
    public static Accel2 operator -(Accel2 a, Accel2 b) => new(a.x - b.x, a.y - b.y);
    public static Accel2 operator *(Accel2 a, Accel2 b) => new(a.x * b.x, a.y * b.y);
    public static Accel2 operator /(Accel2 a, Accel2 b) => new(a.x / b.x, a.y / b.y);
    public Velocity2 Velocity2(Time v) => new(v * x, v * y);
    public Time Time(Velocity2 v) => new(v.magnitude / magnitude);
    public Accel2(Velocity2 a, Time b) => a.Accel2(b);
    public Accel2(Time b, Velocity2 a) => a.Accel2(b);
    public Force2 Force2(Mass v) => new(v * x, v * y);
    public Mass Mass(Force2 v) => new(v.magnitude / magnitude);
    public Accel2(Force2 a, Mass b) => a.Accel2(b);
    public Accel2(Mass b, Force2 a) => a.Accel2(b);
}
}
