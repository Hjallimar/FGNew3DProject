using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PointManager : MonoBehaviour {
	public Material barMaterial;
	public Mesh barMesh;
	[Range(0f,1f)]
	public float damping;
	[Range(0f,1f)]
	public float friction;
	public float breakResistance;
	public float expForce;
	[Range(0f,1f)]
	public float tornadoForce;
	public float tornadoMaxForceDist;
	public float tornadoHeight;
	public float tornadoUpForce;
	public float tornadoInwardForce;

	Point[] points;
	Bar[] bars;
	public int pointCount;

	bool generating;
	public static float tornadoX;
	public static float tornadoZ;

	float tornadoFader = 0f;

	Matrix4x4[][] matrices;
	MaterialPropertyBlock[] matProps;

	Transform cam;

	protected int PointercountIndex;
	protected int BarcountIndex;
	const int instancesPerBatch = 1023;

	private void Awake() {
		Time.timeScale = 0f;
	}
	void Start() {
		StartCoroutine(Generate());
		cam = Camera.main.transform;
	}

	public static float TornadoSway(float y) {
		return Mathf.Sin(y / 5f + Time.time/4f) * 3f;
	}
	
	IEnumerator Generate() {
		generating = true;

		Point[] pointsList = new Point[10000];
		//List<Point> pointsList = new List<Point>();
		PointercountIndex = 0;
		Bar[] barsList = new Bar[50000];
		BarcountIndex = 0;
		//List<Bar> barsList = new List<Bar>();
		List<List<Matrix4x4>> matricesList = new List<List<Matrix4x4>>();
		matricesList.Add(new List<Matrix4x4>());

		// buildings
		for (int i = 0; i < 35; i++) 
		{
			int height = Random.Range(4,12);
			Vector3 pos = new Vector3(Random.Range(-45f,45f),0f,Random.Range(-45f,45f));
			float spacing = 2f;
			for (int j = 0; j < height; j++) 
			{
				Point point = new Point();
				point.AssignCurrent(pos.x+spacing,j * spacing, pos.z-spacing);
				point.AssignOld(point.current.x, point.current.y, point.current.z);  
				
				if (j==0) 
				{
					point.anchor=true;
				}

				pointsList[PointercountIndex].CopyFrom(point);
				PointercountIndex++;
				//pointsList.Add(point);
				
				point.AssignCurrent(pos.x-spacing,j * spacing, pos.z-spacing);
				point.AssignOld(point.current.x, point.current.y, point.current.z);
				
				if (j==0) 
				{
					point.anchor=true;
				}
				pointsList[PointercountIndex].CopyFrom(point);
				PointercountIndex++;
				//pointsList.Add(point);
				
				point.AssignCurrent(pos.x+0f,j * spacing, pos.z+spacing);
				point.AssignOld(point.current.x, point.current.y, point.current.z);
				
				if (j==0) 
				{
					point.anchor=true;
				}
				pointsList[PointercountIndex].CopyFrom(point);
				PointercountIndex++;
				//pointsList.Add(point);
			}
		}

		// ground details
		for (int i=0;i<600;i++) 
		{
			Vector3 pos = new Vector3(Random.Range(-55f,55f),0f,Random.Range(-55f,55f));
			Point point = new Point();
			point.AssignCurrent(pos.x + Random.Range(-.2f,-.1f),pos.y+Random.Range(0f,3f), pos.z + Random.Range(.1f,.2f));
			point.AssignOld(point.current.x, point.current.y, point.current.z);
			
			pointsList[PointercountIndex].CopyFrom(point);
			PointercountIndex++;
			//pointsList.Add(point);

			point.AssignCurrent(pos.x + Random.Range(.2f,.1f),pos.y+Random.Range(0f,.2f), pos.z + Random.Range(-.1f,-.2f));
			point.AssignOld(point.current.x, point.current.y, point.current.z);
			if (Random.value<.1f) 
			{
				point.anchor = true;
			}
			pointsList[PointercountIndex].CopyFrom(point);
			PointercountIndex++;
			//pointsList.Add(point);
		}

		int batch = 0;

		for (int i = 0; i < PointercountIndex; i++) 
		{
			for (int j = i + 1; j < PointercountIndex; j++) 
			{
				Bar bar = new Bar();
				bar.AssignPoints(pointsList[i],pointsList[j]);
				if (bar.length < 5f && bar.length>.2f)
				{
					pointsList[i].neighborCount++;
					pointsList[j].neighborCount++;
					// bar.point1.neighborCount++;
					// bar.point2.neighborCount++;
					
					barsList[BarcountIndex].AssignPoints(pointsList[i], pointsList[j]);
					BarcountIndex++;
					//barsList.Add(bar);
					matricesList[batch].Add(barsList[BarcountIndex].matrix);
					if (matricesList[batch].Count == instancesPerBatch) 
					{
						batch++;
						matricesList.Add(new List<Matrix4x4>());
					}
					if (BarcountIndex % 500 == 0) 
					{
						yield return null;
					}
				}
			}
		}
		points = new Point[BarcountIndex * 2];
		pointCount = 0;
		for (int i=0;i<PointercountIndex;i++) 
		{
			if (pointsList[i].neighborCount > 0) 
			{
				points[pointCount] = pointsList[i];
				pointCount++;
			}
		}
		Debug.Log(pointCount + " points, room for " + points.Length + " (" + BarcountIndex + " bars)");

		bars = barsList;

		matrices = new Matrix4x4[matricesList.Count][];
		for (int i=0;i<matrices.Length;i++) 
		{
			matrices[i] = matricesList[i].ToArray();
		}

		matProps = new MaterialPropertyBlock[BarcountIndex];
		Vector4[] colors = new Vector4[instancesPerBatch];
		for (int i=0;i<BarcountIndex;i++) 
		{
			colors[i%instancesPerBatch] = barsList[i].color;
			if ((i + 1) % instancesPerBatch == 0 || i == BarcountIndex - 1) 
			{
				MaterialPropertyBlock block = new MaterialPropertyBlock();
				block.SetVectorArray("_Color",colors);
				matProps[i / instancesPerBatch] = block;
			}
		}

		pointsList = null;
		barsList = null;
		matricesList = null;
		System.GC.Collect();
		generating = false;
		Time.timeScale = 1f;
	}
	
	void FixedUpdate () {
		if (generating == false) 
		{
			tornadoFader = Mathf.Clamp01(tornadoFader + Time.deltaTime / 10f);

			float invDamping = 1f - damping;
			for (int i = 0; i < pointCount; i++) 
			{
				//Point point = points[i];
				if (points[i].anchor == false) 
				{
					// float startX = points[i].current.x;
					// float startY = points[i].current.y;
					// float startZ = points[i].current.z;

					points[i].old.y += .01f;

					// tornado force
					float tdx = tornadoX+TornadoSway(points[i].current.y) - points[i].current.x;
					float tdz = tornadoZ - points[i].current.z;
					float tornadoDist = Mathf.Sqrt(tdx * tdx + tdz * tdz);
					tdx /= tornadoDist;
					tdz /= tornadoDist;
					if (tornadoDist<tornadoMaxForceDist) 
					{
						float force = (1f - tornadoDist / tornadoMaxForceDist);
						float yFader= Mathf.Clamp01(1f - points[i].current.y / tornadoHeight);
						force *= tornadoFader*tornadoForce*Random.Range(-.3f,1.3f);
						float forceY = tornadoUpForce;
						points[i].old.y -= forceY * force;
						float forceX = -tdz + tdx * tornadoInwardForce*yFader;
						float forceZ = tdx + tdz * tornadoInwardForce*yFader;
						points[i].old.x -= forceX * force;
						points[i].old.z -= forceZ * force;
					}

					points[i].AssignOld(points[i].current.x, points[i].current.y, points[i].current.z);
		
					points[i].current.x += (points[i].current.x - points[i].old.x) * invDamping;
					points[i].current.y += (points[i].current.y - points[i].old.y) * invDamping;
					points[i].current.z += (points[i].current.z - points[i].old.z) * invDamping;

					// points[i].old.x = startX;
					// points[i].old.y = startY;
					// points[i].old.z = startZ;
					if (points[i].current.y < 0f) 
					{
						points[i].current.y = 0f;
						points[i].old.x = -points[i].old.y;
						points[i].old.y += (points[i].current.x - points[i].old.x) * friction;
						points[i].old.z += (points[i].current.z - points[i].old.z) * friction;
					}
				}
			}

			for (int i = 0; i < BarcountIndex; i++) 
			{
				//Bar bar = bars[i];

				Point point1 = bars[i].point1;
				Point point2 = bars[i].point2;

				float3 diff = GetDiff(point2.current, point1.current);

				float dist = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);
				float extraDist = dist - bars[i].length;

				float pushX = (diff.x / dist * extraDist) * .5f;
				float pushY = (diff.y / dist * extraDist) * .5f;
				float pushZ = (diff.z / dist * extraDist) * .5f;

				if (point1.anchor == false && point2.anchor == false) 
				{
					point1.current.x += pushX;
					point1.current.y += pushY;
					point1.current.z += pushZ;
					point2.current.x -= pushX;
					point2.current.y -= pushY;
					point2.current.z -= pushZ;
				} 
				else if (point1.anchor) {
					point2.current.x -= pushX*2f;
					point2.current.y -= pushY*2f;
					point2.current.z -= pushZ*2f;
				} 
				else if (point2.anchor) {
					point1.current.x += pushX*2f;
					point1.current.y += pushY*2f;
					point1.current.z += pushZ*2f;
				}

				if (diff.x/dist * bars[i].oldD.x + diff.y/dist*bars[i].oldD.y + diff.z/dist*bars[i].oldD.z<.99f) 
				{
					// bar has rotated: expensive full-matrix computation
					bars[i].matrix = Matrix4x4.TRS(new Vector3((point1.current.x + point2.current.x) * .5f,(point1.current.y + point2.current.y) * .5f,(point1.current.z + point2.current.z) * .5f),
										   Quaternion.LookRotation(new Vector3(diff.x,diff.y,diff.z)),
										   new Vector3(bars[i].thickness,bars[i].thickness,bars[i].length));
					bars[i].oldD.x = diff.x / dist;
					bars[i].oldD.y = diff.y / dist;
					bars[i].oldD.z = diff.z / dist;
				} 
				else 
				{
					// bar hasn't rotated: only update the position elements
					Matrix4x4 matrix = bars[i].matrix;
					matrix.m03 = (point1.current.x + point2.current.x) * .5f;
					matrix.m13 = (point1.current.y + point2.current.y) * .5f;
					matrix.m23 = (point1.current.z + point2.current.z) * .5f;
					bars[i].matrix = matrix;
				}

				if (Mathf.Abs(extraDist) > breakResistance) 
				{
					if (point2.neighborCount>1) 
					{
						point2.neighborCount--;
						Point newPoint = new Point();
						newPoint.CopyFrom(point2);
						newPoint.neighborCount = 1;
						points[pointCount].CopyFrom(newPoint);
						bars[i].point2.CopyFrom(newPoint);
						pointCount++;
					} 
					else if (point1.neighborCount>1) 
					{
						point1.neighborCount--;
						Point newPoint = new Point();
						newPoint.CopyFrom(point1);
						newPoint.neighborCount = 1;
						points[pointCount].CopyFrom(newPoint);
						bars[i].point1.CopyFrom(newPoint);
						pointCount++;
					}
				}

				matrices[i / instancesPerBatch][i % instancesPerBatch] = bars[i].matrix;
			}
		}
	}

	private void Update() {
		tornadoX = Mathf.Cos(Time.time/6f) * 30f;
		tornadoZ = Mathf.Sin(Time.time/6f * 1.618f) * 30f;
		cam.position = new Vector3(tornadoX,10f,tornadoZ) - cam.forward * 60f;

		if (matrices != null) {
			for (int i = 0; i < matrices.Length; i++) {
				Graphics.DrawMeshInstanced(barMesh,0,barMaterial,matrices[i],matrices[i].Length,matProps[i]);
			}
		}
	}
	
	public float3 GetDiff(float3 first, float3 second)
	{
		float3 diff = new float3(first.x - second.x, first.y - second.y, first.z - second.z);
		return diff;
	}
}
