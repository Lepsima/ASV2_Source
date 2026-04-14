using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct ForceAccel3 : IAutoUnit3 {
    public float x, y, z;
    
    public ForceAccel3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public ForceAccel3(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    
    public static implicit operator Vector3(ForceAccel3 v) => new Vector3(v.x, v.y, v.z);
    public ForceAccel magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));

    public Vector3 ToNewtonSecond() => new Vector3(x * 1f, y * 1f, z * 1f);
    public static ForceAccel3 NewtonSecond(float x, float y, float z) => new ForceAccel3(x * 1f, y * 1f, z * 1f);
    public static ForceAccel3 NewtonSecond(Vector3 v) => new ForceAccel3(v.x * 1f, v.y * 1f, v.z * 1f);

    public static ForceAccel3 operator +(ForceAccel3 a, ForceAccel3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static ForceAccel3 operator -(ForceAccel3 a, ForceAccel3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static ForceAccel3 operator *(ForceAccel3 a, ForceAccel3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static ForceAccel3 operator /(ForceAccel3 a, ForceAccel3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
    public Force3 Force3(Time v) => new(v * x, v * y, v * z);
    public Time Time(Force3 v) => new(v.magnitude / magnitude);
    public ForceAccel3(Force3 a, Time b) => a.ForceAccel3(b);
    public ForceAccel3(Time b, Force3 a) => a.ForceAccel3(b);
}
}
