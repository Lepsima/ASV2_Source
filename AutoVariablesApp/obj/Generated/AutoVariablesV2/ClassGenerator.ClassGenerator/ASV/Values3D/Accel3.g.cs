using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Accel3 : IAutoUnit3 {
    public float x, y, z;
    
    public Accel3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Accel3(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    
    public static implicit operator Vector3(Accel3 v) => new Vector3(v.x, v.y, v.z);
    public Accel magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));

    public Vector3 ToMeterSecondSecond() => new Vector3(x * 1f, y * 1f, z * 1f);
    public static Accel3 MeterSecondSecond(float x, float y, float z) => new Accel3(x * 1f, y * 1f, z * 1f);
    public static Accel3 MeterSecondSecond(Vector3 v) => new Accel3(v.x * 1f, v.y * 1f, v.z * 1f);

    public static Accel3 operator +(Accel3 a, Accel3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Accel3 operator -(Accel3 a, Accel3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Accel3 operator *(Accel3 a, Accel3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Accel3 operator /(Accel3 a, Accel3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
    public Velocity3 Velocity3(Time v) => new(v * x, v * y, v * z);
    public Time Time(Velocity3 v) => new(v.magnitude / magnitude);
    public Accel3(Velocity3 a, Time b) => a.Accel3(b);
    public Accel3(Time b, Velocity3 a) => a.Accel3(b);
    public Force3 Force3(Mass v) => new(v * x, v * y, v * z);
    public Mass Mass(Force3 v) => new(v.magnitude / magnitude);
    public Accel3(Force3 a, Mass b) => a.Accel3(b);
    public Accel3(Mass b, Force3 a) => a.Accel3(b);
}
}
