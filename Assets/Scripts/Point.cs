using Unity.Mathematics;

public struct Point
{
	public float3 current;
	public float3 old;

	public bool anchor;

	public int neighborCount;
	
	public void CopyFrom(Point other) {
		current.x = other.current.x;
		current.y = other.current.y;
		current.z = other.current.z;
		old.x = other.old.x;
		old.y = other.old.y;
		old.z = other.old.z;

		anchor = other.anchor;
		neighborCount = other.neighborCount;
	}

	public void AssignCurrent(float x, float y, float z)
	{
		current.x = x;
		current.y = y;
		current.z = z;
	}
	public void AssignOld(float x, float y, float z)
	{
		old.x = x;
		old.y = y;
		old.z = z;
	}

	public void AddToCurrent(float3 addition)
	{
		current += addition;
	}
}
