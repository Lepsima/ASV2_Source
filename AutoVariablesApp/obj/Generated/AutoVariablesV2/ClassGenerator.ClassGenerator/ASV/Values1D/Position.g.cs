using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Position : IAutoUnit {
    public float x;
    
    public Position(float x) {
        this.x = x;
    }
    public static implicit operator float(Position v) => v.x;
    public Position magnitude => new(x);

    public float ToFeet() => x * 0.3047999977f;
    public static Position Feet(float x) => new Position(x * 3.28083992f);

    public float ToMile() => x * 0.0006213711945f;
    public static Position Mile(float x) => new Position(x * 1609.343994f);

    public float ToMillimeter() => x * 1000f;
    public static Position Millimeter(float x) => new Position(x * 0.001f);

    public float ToCentimeter() => x * 100f;
    public static Position Centimeter(float x) => new Position(x * 0.01f);

    public float ToMeter() => x * 1f;
    public static Position Meter(float x) => new Position(x * 1f);

    public float ToKilometer() => x * 0.001f;
    public static Position Kilometer(float x) => new Position(x * 1000f);

    public float ToMegameter() => x * 1E-06f;
    public static Position Megameter(float x) => new Position(x * 1000000f);

    public static Position operator +(Position a, Position b) => new(a.x + b.x);
    public static Position operator -(Position a, Position b) => new(a.x - b.x);
    public static Position operator *(Position a, Position b) => new(a.x * b.x);
    public static Position operator /(Position a, Position b) => new(a.x / b.x);

    public static Position operator +(Velocity a, Position b) => b + a.Position(VTime.deltaTime);
    public static Position operator +(Position b, Velocity a) => b + a.Position(VTime.deltaTime);
    public static Position operator -(Velocity a, Position b) => a.Position(VTime.deltaTime) - b;
    public static Position operator -(Position b, Velocity a) => b - a.Position(VTime.deltaTime);
    
    public Velocity Velocity(Time v) => new(x / v);
    public Time Time(Velocity v) => new(x / v);
    public Position(Velocity a, Time b) => a.Position(b);
    public Position(Time b, Velocity a) => a.Position(b);
}
}
