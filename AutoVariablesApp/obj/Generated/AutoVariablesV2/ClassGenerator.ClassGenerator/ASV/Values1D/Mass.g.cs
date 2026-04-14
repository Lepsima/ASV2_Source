using System;
using AutoVariablesApp;
namespace Generated.Units {
public struct Mass : IAutoUnit {
    public float x;
    
    public Mass(float x) {
        this.x = x;
    }
    public static implicit operator float(Mass v) => v.x;
    public Mass magnitude => new(x);

    public float ToMilligram() => x * 1000000f;
    public static Mass Milligram(float x) => new Mass(x * 1E-06f);

    public float ToGram() => x * 1000f;
    public static Mass Gram(float x) => new Mass(x * 0.001f);

    public float ToKilogram() => x * 1f;
    public static Mass Kilogram(float x) => new Mass(x * 1f);

    public float ToTon() => x * 0.001f;
    public static Mass Ton(float x) => new Mass(x * 1000f);

    public float ToKiloton() => x * 1E-06f;
    public static Mass Kiloton(float x) => new Mass(x * 1000000f);

    public static Mass operator +(Mass a, Mass b) => new(a.x + b.x);
    public static Mass operator -(Mass a, Mass b) => new(a.x - b.x);
    public static Mass operator *(Mass a, Mass b) => new(a.x * b.x);
    public static Mass operator /(Mass a, Mass b) => new(a.x / b.x);
    public Force Force(Accel v) => new(v.x * x);
    public Accel Accel(Force v) => new(v.x / x);
    public Mass(Force a, Accel b) => a.Mass(b);
    public Mass(Accel b, Force a) => a.Mass(b);
}
}
