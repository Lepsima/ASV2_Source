using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct ForceAccel : IAutoUnit {
    public float x;
    
    public ForceAccel(float x) {
        this.x = x;
    }
    public static implicit operator float(ForceAccel v) => v.x;
    public ForceAccel magnitude => new(x);

    public float ToNewtonSecond() => x * 1f;
    public static ForceAccel NewtonSecond(float x) => new ForceAccel(x * 1f);

    public static ForceAccel operator +(ForceAccel a, ForceAccel b) => new(a.x + b.x);
    public static ForceAccel operator -(ForceAccel a, ForceAccel b) => new(a.x - b.x);
    public static ForceAccel operator *(ForceAccel a, ForceAccel b) => new(a.x * b.x);
    public static ForceAccel operator /(ForceAccel a, ForceAccel b) => new(a.x / b.x);
    public Force Force(Time v) => new(v * x);
    public Time Time(Force v) => new(v / x);
    public ForceAccel(Force a, Time b) => a.ForceAccel(b);
    public ForceAccel(Time b, Force a) => a.ForceAccel(b);
}
}
