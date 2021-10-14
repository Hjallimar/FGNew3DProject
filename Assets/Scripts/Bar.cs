using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Bar 
{
	public Point point1;
	public Point point2;
	public float length;
	public Matrix4x4 matrix;
	public float3 oldD;
	
	public Color color;
	public float thickness;

	public void CopyToPoint1(Point newPoint)
	{
		this.point1.CopyFrom(newPoint);
	}
	public void CopyToPoint2(Point newPoint)
	{
		this.point2.CopyFrom(newPoint);
	}
	public void AssignPoints(Point a, Point b) 
	{
		this.point1.CopyFrom(a);
		this.point2.CopyFrom(b);
		Vector3 delta = new Vector3(point2.current.x - point1.current.x,point2.current.y - point1.current.y,point2.current.z - point1.current.z);
		this.length = delta.magnitude;

		this.thickness = Random.Range(.25f,.35f);

		Vector3 pos = new Vector3(point1.current.x + point2.current.x,point1.current.y + point2.current.y,point1.current.z + point2.current.z) * .5f;
		Quaternion rot = Quaternion.LookRotation(delta);
		Vector3 scale = new Vector3(thickness,thickness,length);
		this.matrix = Matrix4x4.TRS(pos,rot,scale);

		float upDot = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.up,delta.normalized)))/Mathf.PI;
		this.color = Color.white * upDot*Random.Range(.7f,1f);
	}
}
