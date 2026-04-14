using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Position2 : IAutoUnit2 {
    public float x, y;
    
    public Position2(float x, float y) {
        this.x = x;
        this.y = y;
    }
    
    public Position2(Vector2 v2) {
        x = v2.x;
        y = v2.y;
    }

    public static implicit operator Vector2(Position2 v) => new Vector2(v.x, v.y);
    public Position magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));

    public Vector2 ToFeet() => new Vector2(x * 0.3047999977f, y * 0.3047999977f);
    public static Position2 Feet(float x, float y) => new Position2(x * 3.28083992f, y * 3.28083992f);
    public static Position2 Feet(Vector2 v) => new Position2(v.x * 3.28083992f, v.y * 3.28083992f);

    public Vector2 ToMile() => new Vector2(x * 0.0006213711945f, y * 0.0006213711945f);
    public static Position2 Mile(float x, float y) => new Position2(x * 1609.343994f, y * 1609.343994f);
    public static Position2 Mile(Vector2 v) => new Position2(v.x * 1609.343994f, v.y * 1609.343994f);

    public Vector2 ToMillimeter() => new Vector2(x * 1000f, y * 1000f);
    public static Position2 Millimeter(float x, float y) => new Position2(x * 0.001f, y * 0.001f);
    public static Position2 Millimeter(Vector2 v) => new Position2(v.x * 0.001f, v.y * 0.001f);

    public Vector2 ToCentimeter() => new Vector2(x * 100f, y * 100f);
    public static Position2 Centimeter(float x, float y) => new Position2(x * 0.01f, y * 0.01f);
    public static Position2 Centimeter(Vector2 v) => new Position2(v.x * 0.01f, v.y * 0.01f);

    public Vector2 ToMeter() => new Vector2(x * 1f, y * 1f);
    public static Position2 Meter(float x, float y) => new Position2(x * 1f, y * 1f);
    public static Position2 Meter(Vector2 v) => new Position2(v.x * 1f, v.y * 1f);

    public Vector2 ToKilometer() => new Vector2(x * 0.001f, y * 0.001f);
    public static Position2 Kilometer(float x, float y) => new Position2(x * 1000f, y * 1000f);
    public static Position2 Kilometer(Vector2 v) => new Position2(v.x * 1000f, v.y * 1000f);

    public Vector2 ToMegameter() => new Vector2(x * 1E-06f, y * 1E-06f);
    public static Position2 Megameter(float x, float y) => new Position2(x * 1000000f, y * 1000000f);
    public static Position2 Megameter(Vector2 v) => new Position2(v.x * 1000000f, v.y * 1000000f);

    public static Position2 operator +(Position2 a, Position2 b) => new(a.x + b.x, a.y + b.y);
    public static Position2 operator -(Position2 a, Position2 b) => new(a.x - b.x, a.y - b.y);
    public static Position2 operator *(Position2 a, Position2 b) => new(a.x * b.x, a.y * b.y);
    public static Position2 operator /(Position2 a, Position2 b) => new(a.x / b.x, a.y / b.y);

    public static Position2 operator +(Velocity2 a, Position2 b) => b + a.Position2(VTime.deltaTime);
    public static Position2 operator +(Position2 b, Velocity2 a) => b + a.Position2(VTime.deltaTime);
    public static Position2 operator -(Velocity2 a, Position2 b) => a.Position2(VTime.deltaTime) - b;
    public static Position2 operator -(Position2 b, Velocity2 a) => b - a.Position2(VTime.deltaTime);
    
    public Velocity2 Velocity2(Time v) => new(x / v, y / v);
    public Time Time(Velocity2 v) => new(magnitude / v.magnitude);
    public Position2(Velocity2 a, Time b) => a.Position2(b);
    public Position2(Time b, Velocity2 a) => a.Position2(b);
}
}
