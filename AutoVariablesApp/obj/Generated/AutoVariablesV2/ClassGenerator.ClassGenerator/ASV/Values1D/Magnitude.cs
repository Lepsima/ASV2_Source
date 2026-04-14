using System;
using UnityEngine;

namespace Lepsima.ASV {
public struct Magnitude : IAutoUnit {
    public float x;
    
    public Magnitude(float x) {
        this.x = x;
    }
    
    public float vector => x;
    
    public static implicit operator float(Magnitude v) => v.x;
    public Magnitude magnitude => new(x);

    public static Mass operator *(Mass left, Magnitude right) => new(left.vector * right.vector);
    public static Mass operator *(Magnitude left, Mass right) => new(left.vector * right.vector);

    public static Force operator *(Force left, Magnitude right) => new(left.vector * right.vector);
    public static Force operator *(Magnitude left, Force right) => new(left.vector * right.vector);

    public static Force2 operator *(Force2 left, Magnitude right) => new(left.vector * right.vector);
    public static Force2 operator *(Magnitude left, Force2 right) => new(left.vector * right.vector);

    public static Force3 operator *(Force3 left, Magnitude right) => new(left.vector * right.vector);
    public static Force3 operator *(Magnitude left, Force3 right) => new(left.vector * right.vector);

    public static Time operator *(Time left, Magnitude right) => new(left.vector * right.vector);
    public static Time operator *(Magnitude left, Time right) => new(left.vector * right.vector);

    public static Position operator *(Position left, Magnitude right) => new(left.vector * right.vector);
    public static Position operator *(Magnitude left, Position right) => new(left.vector * right.vector);

    public static Position2 operator *(Position2 left, Magnitude right) => new(left.vector * right.vector);
    public static Position2 operator *(Magnitude left, Position2 right) => new(left.vector * right.vector);

    public static Position3 operator *(Position3 left, Magnitude right) => new(left.vector * right.vector);
    public static Position3 operator *(Magnitude left, Position3 right) => new(left.vector * right.vector);

    public static Angle operator *(Angle left, Magnitude right) => new(left.vector * right.vector);
    public static Angle operator *(Magnitude left, Angle right) => new(left.vector * right.vector);

    public static Angle2 operator *(Angle2 left, Magnitude right) => new(left.vector * right.vector);
    public static Angle2 operator *(Magnitude left, Angle2 right) => new(left.vector * right.vector);

    public static Angle3 operator *(Angle3 left, Magnitude right) => new(left.vector * right.vector);
    public static Angle3 operator *(Magnitude left, Angle3 right) => new(left.vector * right.vector);

    public static Velocity operator *(Velocity left, Magnitude right) => new(left.vector * right.vector);
    public static Velocity operator *(Magnitude left, Velocity right) => new(left.vector * right.vector);

    public static Velocity2 operator *(Velocity2 left, Magnitude right) => new(left.vector * right.vector);
    public static Velocity2 operator *(Magnitude left, Velocity2 right) => new(left.vector * right.vector);

    public static Velocity3 operator *(Velocity3 left, Magnitude right) => new(left.vector * right.vector);
    public static Velocity3 operator *(Magnitude left, Velocity3 right) => new(left.vector * right.vector);

    public static Accel operator *(Accel left, Magnitude right) => new(left.vector * right.vector);
    public static Accel operator *(Magnitude left, Accel right) => new(left.vector * right.vector);

    public static Accel2 operator *(Accel2 left, Magnitude right) => new(left.vector * right.vector);
    public static Accel2 operator *(Magnitude left, Accel2 right) => new(left.vector * right.vector);

    public static Accel3 operator *(Accel3 left, Magnitude right) => new(left.vector * right.vector);
    public static Accel3 operator *(Magnitude left, Accel3 right) => new(left.vector * right.vector);

    public static ForceAccel operator *(ForceAccel left, Magnitude right) => new(left.vector * right.vector);
    public static ForceAccel operator *(Magnitude left, ForceAccel right) => new(left.vector * right.vector);

    public static ForceAccel2 operator *(ForceAccel2 left, Magnitude right) => new(left.vector * right.vector);
    public static ForceAccel2 operator *(Magnitude left, ForceAccel2 right) => new(left.vector * right.vector);

    public static ForceAccel3 operator *(ForceAccel3 left, Magnitude right) => new(left.vector * right.vector);
    public static ForceAccel3 operator *(Magnitude left, ForceAccel3 right) => new(left.vector * right.vector);

    public static AngleVel operator *(AngleVel left, Magnitude right) => new(left.vector * right.vector);
    public static AngleVel operator *(Magnitude left, AngleVel right) => new(left.vector * right.vector);

    public static AngleVel2 operator *(AngleVel2 left, Magnitude right) => new(left.vector * right.vector);
    public static AngleVel2 operator *(Magnitude left, AngleVel2 right) => new(left.vector * right.vector);

    public static AngleVel3 operator *(AngleVel3 left, Magnitude right) => new(left.vector * right.vector);
    public static AngleVel3 operator *(Magnitude left, AngleVel3 right) => new(left.vector * right.vector);

    public static AngleAccel operator *(AngleAccel left, Magnitude right) => new(left.vector * right.vector);
    public static AngleAccel operator *(Magnitude left, AngleAccel right) => new(left.vector * right.vector);

    public static AngleAccel2 operator *(AngleAccel2 left, Magnitude right) => new(left.vector * right.vector);
    public static AngleAccel2 operator *(Magnitude left, AngleAccel2 right) => new(left.vector * right.vector);

    public static AngleAccel3 operator *(AngleAccel3 left, Magnitude right) => new(left.vector * right.vector);
    public static AngleAccel3 operator *(Magnitude left, AngleAccel3 right) => new(left.vector * right.vector);

    public float ToNumber() => x * 1f;
    public static Magnitude Number(float x) => new Magnitude(x * 1f);

    public static Magnitude operator +(Magnitude a, Magnitude b) => new(a.x + b.x);
    public static Magnitude operator -(Magnitude a, Magnitude b) => new(a.x - b.x);
    public static Magnitude operator *(Magnitude a, Magnitude b) => new(a.x * b.x);
    public static Magnitude operator /(Magnitude a, Magnitude b) => new(a.x / b.x);
}
}
