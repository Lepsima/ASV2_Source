using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Position3 : IAutoUnit3 {
    public float x, y, z;
    
    public Position3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Position3(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    
    public static implicit operator Vector3(Position3 v) => new Vector3(v.x, v.y, v.z);
    public Position magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));

    public Vector3 ToFeet() => new Vector3(x * 0.3047999977f, y * 0.3047999977f, z * 0.3047999977f);
    public static Position3 Feet(float x, float y, float z) => new Position3(x * 3.28083992f, y * 3.28083992f, z * 3.28083992f);
    public static Position3 Feet(Vector3 v) => new Position3(v.x * 3.28083992f, v.y * 3.28083992f, v.z * 3.28083992f);

    public Vector3 ToMile() => new Vector3(x * 0.0006213711945f, y * 0.0006213711945f, z * 0.0006213711945f);
    public static Position3 Mile(float x, float y, float z) => new Position3(x * 1609.343994f, y * 1609.343994f, z * 1609.343994f);
    public static Position3 Mile(Vector3 v) => new Position3(v.x * 1609.343994f, v.y * 1609.343994f, v.z * 1609.343994f);

    public Vector3 ToMillimeter() => new Vector3(x * 1000f, y * 1000f, z * 1000f);
    public static Position3 Millimeter(float x, float y, float z) => new Position3(x * 0.001f, y * 0.001f, z * 0.001f);
    public static Position3 Millimeter(Vector3 v) => new Position3(v.x * 0.001f, v.y * 0.001f, v.z * 0.001f);

    public Vector3 ToCentimeter() => new Vector3(x * 100f, y * 100f, z * 100f);
    public static Position3 Centimeter(float x, float y, float z) => new Position3(x * 0.01f, y * 0.01f, z * 0.01f);
    public static Position3 Centimeter(Vector3 v) => new Position3(v.x * 0.01f, v.y * 0.01f, v.z * 0.01f);

    public Vector3 ToMeter() => new Vector3(x * 1f, y * 1f, z * 1f);
    public static Position3 Meter(float x, float y, float z) => new Position3(x * 1f, y * 1f, z * 1f);
    public static Position3 Meter(Vector3 v) => new Position3(v.x * 1f, v.y * 1f, v.z * 1f);

    public Vector3 ToKilometer() => new Vector3(x * 0.001f, y * 0.001f, z * 0.001f);
    public static Position3 Kilometer(float x, float y, float z) => new Position3(x * 1000f, y * 1000f, z * 1000f);
    public static Position3 Kilometer(Vector3 v) => new Position3(v.x * 1000f, v.y * 1000f, v.z * 1000f);

    public Vector3 ToMegameter() => new Vector3(x * 1E-06f, y * 1E-06f, z * 1E-06f);
    public static Position3 Megameter(float x, float y, float z) => new Position3(x * 1000000f, y * 1000000f, z * 1000000f);
    public static Position3 Megameter(Vector3 v) => new Position3(v.x * 1000000f, v.y * 1000000f, v.z * 1000000f);

    public static Position3 operator +(Position3 a, Position3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Position3 operator -(Position3 a, Position3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Position3 operator *(Position3 a, Position3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Position3 operator /(Position3 a, Position3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);

    public static Position3 operator +(Velocity3 a, Position3 b) => b + a.Position3(VTime.deltaTime);
    public static Position3 operator +(Position3 b, Velocity3 a) => b + a.Position3(VTime.deltaTime);
    public static Position3 operator -(Velocity3 a, Position3 b) => a.Position3(VTime.deltaTime) - b;
    public static Position3 operator -(Position3 b, Velocity3 a) => b - a.Position3(VTime.deltaTime);
    
    public Velocity3 Velocity3(Time v) => new(x / v, y / v, z / v);
    public Time Time(Velocity3 v) => new(magnitude / v.magnitude);
    public Position3(Velocity3 a, Time b) => a.Position3(b);
    public Position3(Time b, Velocity3 a) => a.Position3(b);
}
}
