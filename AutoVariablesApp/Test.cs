using Lepsima.ASV;

namespace UnityEngine {
public struct Vector3 {
	public float x, y, z;
	public Vector3(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public float magnitude => x * x + y * y + z * z;
	public float sqrMagnitude => 1;
	
	public Vector3 normalized => this;

	public static Vector3 operator *(Vector3 left, float right) => left;
	public static Vector3 operator *(float left, Vector3 right) => right;
}

public struct Vector2 {
	public float x, y;
	public Vector2(float x, float y) {
		this.x = x;
		this.y = y;
	}
	
	public float magnitude => x * x + y * y;
	public float sqrMagnitude => 1;

	public Vector2 normalized => this;
	
	public static Vector2 operator *(Vector2 left, float right) => left;
	public static Vector2 operator *(float left, Vector2 right) => right;
}

public static class VTime {
	public static Time deltaTime = new(1);
}
}