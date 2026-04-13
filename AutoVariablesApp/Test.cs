namespace Generated.Units {
public struct Vector3 {
	public float x, y, z;
	public Vector3(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	public float magnitude => x * x + y * y + z * z;
}

public struct Vector2 {
	public float x, y;
	public Vector2(float x, float y) {
		this.x = x;
		this.y = y;
	}
	
	public float magnitude => x * x + y * y;
}

public static class VTime {
	public static Time deltaTime = new(1);
}
}