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

	public void AssignPoints(Point a, Point b) 
	{
		point1 = a;
		point2 = b;
		Vector3 delta = new Vector3(point2.current.x - point1.current.x,point2.current.y - point1.current.y,point2.current.z - point1.current.z);
		length = delta.magnitude;

		thickness = Random.Range(.25f,.35f);

		Vector3 pos = new Vector3(point1.current.x + point2.current.x,point1.current.y + point2.current.y,point1.current.z + point2.current.z) * .5f;
		Quaternion rot = Quaternion.LookRotation(delta);
		Vector3 scale = new Vector3(thickness,thickness,length);
		matrix = Matrix4x4.TRS(pos,rot,scale);

		float upDot = Mathf.Acos(Mathf.Abs(Vector3.Dot(Vector3.up,delta.normalized)))/Mathf.PI;
		color = Color.white * upDot*Random.Range(.7f,1f);
	}
}
