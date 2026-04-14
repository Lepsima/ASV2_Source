using System;
using UnityEngine;

namespace Lepsima.ASV {
public struct Direction3 : IAutoUnit3 {
    public float x, y, z;
    
    public Direction3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Direction3(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }
    
    public Vector3 vector => this;
    
    public static implicit operator Vector3(Direction3 v) => new Vector3(v.x, v.y, v.z);
    public Magnitude magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));

    public static Force3 operator *(Force left, Direction3 right) => new(left.vector * right.vector);
    public static Force3 operator *(Direction3 left, Force right) => new(left.vector * right.vector);

    public static Position3 operator *(Position left, Direction3 right) => new(left.vector * right.vector);
    public static Position3 operator *(Direction3 left, Position right) => new(left.vector * right.vector);

    public static Angle3 operator *(Angle left, Direction3 right) => new(left.vector * right.vector);
    public static Angle3 operator *(Direction3 left, Angle right) => new(left.vector * right.vector);

    public static Velocity3 operator *(Velocity left, Direction3 right) => new(left.vector * right.vector);
    public static Velocity3 operator *(Direction3 left, Velocity right) => new(left.vector * right.vector);

    public static Accel3 operator *(Accel left, Direction3 right) => new(left.vector * right.vector);
    public static Accel3 operator *(Direction3 left, Accel right) => new(left.vector * right.vector);

    public static ForceAccel3 operator *(ForceAccel left, Direction3 right) => new(left.vector * right.vector);
    public static ForceAccel3 operator *(Direction3 left, ForceAccel right) => new(left.vector * right.vector);

    public static AngleVel3 operator *(AngleVel left, Direction3 right) => new(left.vector * right.vector);
    public static AngleVel3 operator *(Direction3 left, AngleVel right) => new(left.vector * right.vector);

    public static AngleAccel3 operator *(AngleAccel left, Direction3 right) => new(left.vector * right.vector);
    public static AngleAccel3 operator *(Direction3 left, AngleAccel right) => new(left.vector * right.vector);

    public Vector3 ToNormal() => new Vector3(x * 1f, y * 1f, z * 1f);
    public static Direction3 Normal(float x, float y, float z) => new Direction3(x * 1f, y * 1f, z * 1f);
    public static Direction3 Normal(Vector3 v) => new Direction3(v.x * 1f, v.y * 1f, v.z * 1f);

    public static Direction3 operator +(Direction3 a, Direction3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Direction3 operator -(Direction3 a, Direction3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Direction3 operator *(Direction3 a, Direction3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Direction3 operator /(Direction3 a, Direction3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
}
}
