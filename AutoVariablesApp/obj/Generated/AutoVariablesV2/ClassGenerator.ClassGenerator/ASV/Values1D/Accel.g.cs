using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Accel : IAutoUnit {
    public float x;
    
    public Accel(float x) {
        this.x = x;
    }
    public static implicit operator float(Accel v) => v.x;
    public Accel magnitude => new(x);

    public float ToMeterSecondSecond() => x * 1f;
    public static Accel MeterSecondSecond(float x) => new Accel(x * 1f);

    public static Accel operator +(Accel a, Accel b) => new(a.x + b.x);
    public static Accel operator -(Accel a, Accel b) => new(a.x - b.x);
    public static Accel operator *(Accel a, Accel b) => new(a.x * b.x);
    public static Accel operator /(Accel a, Accel b) => new(a.x / b.x);
    public Velocity Velocity(Time v) => new(v * x);
    public Time Time(Velocity v) => new(v / x);
    public Accel(Velocity a, Time b) => a.Accel(b);
    public Accel(Time b, Velocity a) => a.Accel(b);
    public Force Force(Mass v) => new(v * x);
    public Mass Mass(Force v) => new(v / x);
    public Accel(Force a, Mass b) => a.Accel(b);
    public Accel(Mass b, Force a) => a.Accel(b);
}
}
