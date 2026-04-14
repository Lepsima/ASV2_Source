using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Time : IAutoUnit {
    public float x;
    
    public Time(float x) {
        this.x = x;
    }
    public static implicit operator float(Time v) => v.x;
    public Time magnitude => new(x);

    public float ToMicrosecond() => x * 1000000f;
    public static Time Microsecond(float x) => new Time(x * 1E-06f);

    public float ToMillisecond() => x * 1000f;
    public static Time Millisecond(float x) => new Time(x * 0.001f);

    public float ToSecond() => x * 1f;
    public static Time Second(float x) => new Time(x * 1f);

    public float ToMinute() => x * 0.01666666667f;
    public static Time Minute(float x) => new Time(x * 60f);

    public float ToHour() => x * 0.0002777777778f;
    public static Time Hour(float x) => new Time(x * 3600f);

    public static Time operator +(Time a, Time b) => new(a.x + b.x);
    public static Time operator -(Time a, Time b) => new(a.x - b.x);
    public static Time operator *(Time a, Time b) => new(a.x * b.x);
    public static Time operator /(Time a, Time b) => new(a.x / b.x);
    public Position Position(Velocity v) => new(v.x * x);
    public Velocity Velocity(Position v) => new(v.x / x);
    public Time(Position a, Velocity b) => a.Time(b);
    public Time(Velocity b, Position a) => a.Time(b);
    public Velocity Velocity(Accel v) => new(v.x * x);
    public Accel Accel(Velocity v) => new(v.x / x);
    public Time(Velocity a, Accel b) => a.Time(b);
    public Time(Accel b, Velocity a) => a.Time(b);
    public Force Force(ForceAccel v) => new(v.x * x);
    public ForceAccel ForceAccel(Force v) => new(v.x / x);
    public Time(Force a, ForceAccel b) => a.Time(b);
    public Time(ForceAccel b, Force a) => a.Time(b);
}
}
