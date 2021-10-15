using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Bar 
{
	public int pointIndex1;
	public int pointIndex2;
	public float length;
	public Matrix4x4 matrix;
	public float3 oldD;
	
	public Color color;
	public float thickness;

	public void CopyToPoint1(Point newPoint)
	{
		this.pointIndex1 = newPoint.Index;
	}
	public void CopyToPoint2(Point newPoint)
	{ 
		this.pointIndex2 = newPoint.Index;
	}
	public void AssignPoints(Point a, Point b) 
	{
		this.pointIndex1 = a.Index;
		this.pointIndex2 = b.Index;
		Vector3 delta = new Vector3(b.current.x - a.current.x,b.current.y - a.current.y,b.current.z - a.current.z);
		this.length = delta.magnitude;

		this.thickness = Random.Range(.25f,.35f);

		Vector3 pos = new Vector3(a.current.x + b.current.x,a.current.y + b.current.y,a.current.z + b.current.z) * .5f;
		Quaternion rot = Quaternion.LookRotation(delta);
		Vector3 scale = new Vector3(thickness,thickness,length);
		this.matrix = Matrix4x4.TRS(pos,rot,scale);

		float upDot = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.up,delta.normalized)))/Mathf.PI;
		this.color = Color.white * upDot*Random.Range(.7f,1f);
	}
}
