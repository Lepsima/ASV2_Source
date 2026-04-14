using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Velocity2 : IAutoUnit2 {
    public float x, y;
    
    public Velocity2(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Velocity2(Vector2 v2) {
        x = v2.x;
        y = v2.y;
    }

    public static implicit operator Vector2(Velocity2 v) => new Vector2(v.x, v.y);
    public Velocity magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));

    public Vector2 ToMeterSecond() => new Vector2(x * 1f, y * 1f);
    public static Velocity2 MeterSecond(float x, float y) => new Velocity2(x * 1f, y * 1f);
    public static Velocity2 MeterSecond(Vector2 v) => new Velocity2(v.x * 1f, v.y * 1f);

    public static Velocity2 operator +(Velocity2 a, Velocity2 b) => new(a.x + b.x, a.y + b.y);
    public static Velocity2 operator -(Velocity2 a, Velocity2 b) => new(a.x - b.x, a.y - b.y);
    public static Velocity2 operator *(Velocity2 a, Velocity2 b) => new(a.x * b.x, a.y * b.y);
    public static Velocity2 operator /(Velocity2 a, Velocity2 b) => new(a.x / b.x, a.y / b.y);
    public Position2 Position2(Time v) => new(v * x, v * y);
    public Time Time(Position2 v) => new(v.magnitude / magnitude);
    public Velocity2(Position2 a, Time b) => a.Velocity2(b);
    public Velocity2(Time b, Position2 a) => a.Velocity2(b);

    public static Velocity2 operator +(Accel2 a, Velocity2 b) => b + a.Velocity2(VTime.deltaTime);
    public static Velocity2 operator +(Velocity2 b, Accel2 a) => b + a.Velocity2(VTime.deltaTime);
    public static Velocity2 operator -(Accel2 a, Velocity2 b) => a.Velocity2(VTime.deltaTime) - b;
    public static Velocity2 operator -(Velocity2 b, Accel2 a) => b - a.Velocity2(VTime.deltaTime);
    
    public Accel2 Accel2(Time v) => new(x / v, y / v);
    public Time Time(Accel2 v) => new(magnitude / v.magnitude);
    public Velocity2(Accel2 a, Time b) => a.Velocity2(b);
    public Velocity2(Time b, Accel2 a) => a.Velocity2(b);
}
}
