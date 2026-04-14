using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Force3 : IAutoUnit3 {
    public float x, y, z;
    
    public Force3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Force3(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    
    public static implicit operator Vector3(Force3 v) => new Vector3(v.x, v.y, v.z);
    public Force magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));

    public Vector3 ToMillinewton() => new Vector3(x * 1000f, y * 1000f, z * 1000f);
    public static Force3 Millinewton(float x, float y, float z) => new Force3(x * 0.001f, y * 0.001f, z * 0.001f);
    public static Force3 Millinewton(Vector3 v) => new Force3(v.x * 0.001f, v.y * 0.001f, v.z * 0.001f);

    public Vector3 ToNewton() => new Vector3(x * 1f, y * 1f, z * 1f);
    public static Force3 Newton(float x, float y, float z) => new Force3(x * 1f, y * 1f, z * 1f);
    public static Force3 Newton(Vector3 v) => new Force3(v.x * 1f, v.y * 1f, v.z * 1f);

    public Vector3 ToKilonewton() => new Vector3(x * 0.001f, y * 0.001f, z * 0.001f);
    public static Force3 Kilonewton(float x, float y, float z) => new Force3(x * 1000f, y * 1000f, z * 1000f);
    public static Force3 Kilonewton(Vector3 v) => new Force3(v.x * 1000f, v.y * 1000f, v.z * 1000f);

    public Vector3 ToMeganewton() => new Vector3(x * 1E-06f, y * 1E-06f, z * 1E-06f);
    public static Force3 Meganewton(float x, float y, float z) => new Force3(x * 1000000f, y * 1000000f, z * 1000000f);
    public static Force3 Meganewton(Vector3 v) => new Force3(v.x * 1000000f, v.y * 1000000f, v.z * 1000000f);

    public static Force3 operator +(Force3 a, Force3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Force3 operator -(Force3 a, Force3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Force3 operator *(Force3 a, Force3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Force3 operator /(Force3 a, Force3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
    public Accel3 Accel3(Mass v) => new(x / v, y / v, z / v);
    public Mass Mass(Accel3 v) => new(magnitude / v.magnitude);
    public Force3(Accel3 a, Mass b) => a.Force3(b);
    public Force3(Mass b, Accel3 a) => a.Force3(b);

    public static Force3 operator +(ForceAccel3 a, Force3 b) => b + a.Force3(VTime.deltaTime);
    public static Force3 operator +(Force3 b, ForceAccel3 a) => b + a.Force3(VTime.deltaTime);
    public static Force3 operator -(ForceAccel3 a, Force3 b) => a.Force3(VTime.deltaTime) - b;
    public static Force3 operator -(Force3 b, ForceAccel3 a) => b - a.Force3(VTime.deltaTime);
    
    public ForceAccel3 ForceAccel3(Time v) => new(x / v, y / v, z / v);
    public Time Time(ForceAccel3 v) => new(magnitude / v.magnitude);
    public Force3(ForceAccel3 a, Time b) => a.Force3(b);
    public Force3(Time b, ForceAccel3 a) => a.Force3(b);
}
}
