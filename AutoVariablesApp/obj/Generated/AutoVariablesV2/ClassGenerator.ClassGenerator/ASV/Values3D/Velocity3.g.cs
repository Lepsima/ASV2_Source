using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Velocity3 : IAutoUnit3 {
    public float x, y, z;
    
    public Velocity3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Velocity3(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    
    public static implicit operator Vector3(Velocity3 v) => new Vector3(v.x, v.y, v.z);
    public Velocity magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));

    public Vector3 ToMeterSecond() => new Vector3(x * 1f, y * 1f, z * 1f);
    public static Velocity3 MeterSecond(float x, float y, float z) => new Velocity3(x * 1f, y * 1f, z * 1f);
    public static Velocity3 MeterSecond(Vector3 v) => new Velocity3(v.x * 1f, v.y * 1f, v.z * 1f);

    public static Velocity3 operator +(Velocity3 a, Velocity3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Velocity3 operator -(Velocity3 a, Velocity3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Velocity3 operator *(Velocity3 a, Velocity3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Velocity3 operator /(Velocity3 a, Velocity3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
    public Position3 Position3(Time v) => new(v * x, v * y, v * z);
    public Time Time(Position3 v) => new(v.magnitude / magnitude);
    public Velocity3(Position3 a, Time b) => a.Velocity3(b);
    public Velocity3(Time b, Position3 a) => a.Velocity3(b);

    public static Velocity3 operator +(Accel3 a, Velocity3 b) => b + a.Velocity3(VTime.deltaTime);
    public static Velocity3 operator +(Velocity3 b, Accel3 a) => b + a.Velocity3(VTime.deltaTime);
    public static Velocity3 operator -(Accel3 a, Velocity3 b) => a.Velocity3(VTime.deltaTime) - b;
    public static Velocity3 operator -(Velocity3 b, Accel3 a) => b - a.Velocity3(VTime.deltaTime);
    
    public Accel3 Accel3(Time v) => new(x / v, y / v, z / v);
    public Time Time(Accel3 v) => new(magnitude / v.magnitude);
    public Velocity3(Accel3 a, Time b) => a.Velocity3(b);
    public Velocity3(Time b, Accel3 a) => a.Velocity3(b);
}
}
