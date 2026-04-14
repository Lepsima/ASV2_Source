using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Velocity : IAutoUnit {
    public float x;
    
    public Velocity(float x) {
        this.x = x;
    }
    public static implicit operator float(Velocity v) => v.x;
    public Velocity magnitude => new(x);

    public float ToMeterSecond() => x * 1f;
    public static Velocity MeterSecond(float x) => new Velocity(x * 1f);

    public static Velocity operator +(Velocity a, Velocity b) => new(a.x + b.x);
    public static Velocity operator -(Velocity a, Velocity b) => new(a.x - b.x);
    public static Velocity operator *(Velocity a, Velocity b) => new(a.x * b.x);
    public static Velocity operator /(Velocity a, Velocity b) => new(a.x / b.x);
    public Position Position(Time v) => new(v * x);
    public Time Time(Position v) => new(v / x);
    public Velocity(Position a, Time b) => a.Velocity(b);
    public Velocity(Time b, Position a) => a.Velocity(b);

    public static Velocity operator +(Accel a, Velocity b) => b + a.Velocity(VTime.deltaTime);
    public static Velocity operator +(Velocity b, Accel a) => b + a.Velocity(VTime.deltaTime);
    public static Velocity operator -(Accel a, Velocity b) => a.Velocity(VTime.deltaTime) - b;
    public static Velocity operator -(Velocity b, Accel a) => b - a.Velocity(VTime.deltaTime);
    
    public Accel Accel(Time v) => new(x / v);
    public Time Time(Accel v) => new(x / v);
    public Velocity(Accel a, Time b) => a.Velocity(b);
    public Velocity(Time b, Accel a) => a.Velocity(b);
}
}
