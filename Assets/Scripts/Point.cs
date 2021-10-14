using Unity.Mathematics;

public struct Point
{
	public int Index;
	public float3 current;
	public float3 old;

	public bool anchor;

	public int neighborCount;
	
	public void CopyFrom(Point other)
	{
		this.Index = other.Index;
		this.current = other.current;
		this.old = other.old;

		this.anchor = other.anchor;
		this.neighborCount = other.neighborCount;
	}

	public void AssignCurrent(float x, float y, float z)
	{
		this.current.x = x;
		this.current.y = y;
		this.current.z = z;
	}
	public void AssignOld(float x, float y, float z)
	{
		this.old.x = x;
		this.old.y = y;
		this.old.z = z;
	}

	public void AddToCurrent(float3 addition)
	{
		this.current += addition;
	}
	public void SubtractFromCurrent(float3 subtraction)
	{
		this.current -= subtraction;
	}
}
