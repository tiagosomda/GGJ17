using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Set up at Unity3d Script because this is now a long term asset for multiple projects
/// </summary>
/// 

public class FieldOfView : MonoBehaviour {
	
	[Range(0,10)]
	public float setRadius = 5;
	[Range(0,360)]
	public float setAngle=25;

	public float lerpSpeed=.5f;

	[HideInInspector]
	public float viewRadius=5;


	[Range(0,360),HideInInspector]
	public float viewAngle=25;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	//[HideInInspector]
	public List<Transform> visTargets=new List<Transform>();

	[Range(0,20)]
	public float meshRes;//A higher mesh Resolution creates more stable LOS

	[Range(0,10)]
	public int edgeResolveInt = 6;
	public MeshFilter viewMeshFilter;

	[Range(0,1)]
	public float edgedisThreshold=.5f;

	Mesh viewMesh;


	public Transform target;
	EnemyAI anAI;
	void Start(){
		anAI = GetComponent<EnemyAI> ();
		viewMesh = new Mesh ();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
	//How to set up: Add empty child to gameobject, give the empty child a mesh filter componenet as well as mesh renderer component
	// and set cast and recieve shadows to off

		StartCoroutine ("FindTargswithDelay", .25f);
	}

	void LateUpdate(){
		//Late update to avoid jitter
		//the higher the mesh resolution the more stable the mesh
		//DrawFOW ();

		//Slowly expand radius and angle
		if (viewRadius != setRadius)
			viewRadius = Mathf.Lerp (viewRadius, setRadius, lerpSpeed * Time.deltaTime);
		else
			return;
		
		if (viewAngle != setAngle)
			viewAngle = Mathf.Lerp (viewAngle, setAngle, lerpSpeed * Time.deltaTime);
		else
			return;
		
	}

	IEnumerator FindTargswithDelay(float delay){
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}
	//Takes rays casted and converts into a mesh
	public void DrawFOW(){
		
		int stepCount = Mathf.RoundToInt(viewAngle* meshRes);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3> ();

		ViewCastInfo oldViewCast = new ViewCastInfo ();
		for(int i=0;i<=stepCount;i++){
			
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast=ViewCast(angle);


			if(i>0){
				bool edgeDstThresholdExceeded = Mathf.Abs (oldViewCast.dst-newViewCast.dst)>edgedisThreshold;

				if (oldViewCast.hit != newViewCast.hit ||oldViewCast.hit&&newViewCast.hit&&edgeDstThresholdExceeded) {
					EdgeInfo edge = findEdge (oldViewCast, newViewCast);

					if (edge.pointA !=Vector3.zero) {
						viewPoints.Add (edge.pointA);
					}
					if (edge.pointB !=Vector3.zero) {
						viewPoints.Add (edge.pointB);
					}
				}
			}
			viewPoints.Add (newViewCast.point);
			oldViewCast = newViewCast;
		}
		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[]triangles=new int[(vertexCount-2)*3];
		vertices [0] =Vector3.zero;

		for (int i = 0; i < vertexCount - 1; i++) {
			vertices [i * 1] = transform.InverseTransformPoint(viewPoints [i]);

			if(i<vertexCount-2){
			triangles [i * 3] = 0;
			triangles [i * 3 + 1] = i + 1;
			triangles [i * 3 + 2] = i + 2;
			}
		}
		viewMesh.Clear ();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}

	EdgeInfo findEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast){
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveInt; i++) {
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast (angle);

			bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.dst-maxViewCast.dst)>edgedisThreshold;
			if (newViewCast.hit == minViewCast.hit&&!edgeDstThresholdExceeded) {
				minAngle = angle;
				minPoint =new Vector3(newViewCast.point.x,newViewCast.point.y,0);
			} else {
				maxAngle = angle;
				maxPoint = new Vector3 (newViewCast.point.x, newViewCast.point.y, 0);;
			}
		}
		return new EdgeInfo (minPoint,maxPoint);
	}

	public struct EdgeInfo{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA,Vector3 _pointB){
			pointA=_pointA;
			pointB=_pointB;
		}
	}
	ViewCastInfo ViewCast(float globalAngle){

		Vector3 dir = DirAngle (globalAngle, true);

		RaycastHit hit;

		if (Physics.Raycast (transform.position, dir, out hit,viewRadius, obstacleMask)) {
				return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
			} else {
				return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);

			}
		}

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit,Vector3 _point,float _dst, float _angle){
			hit=_hit;
			point=_point;
			dst=_dst;
			angle=_angle;
		}
	}


	void FindVisibleTargets(){
		visTargets.Clear ();
		//3D
		//Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		//2D
		Collider[] targetsInViewRadius=Physics.OverlapSphere(transform.position,viewRadius,targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			target = targetsInViewRadius [i].transform;
			Vector3 dirtoPlayer = (target.position-transform.position).normalized;
			;

			if(Vector3.Angle(Vector3.forward,dirtoPlayer)<viewAngle/2){
				float dstToTarget = Vector3.Distance (transform.position, target.position);

				if(Physics.Raycast (transform.position,Vector3.forward,viewRadius,targetMask)){
					visTargets.Add (target);
					if(target.CompareTag("Player")){
					anAI.lastKnownPosit = target.position;
					anAI.SetDestination (target.position, 5);
					}
				}
			}
		}
	}


	public Vector3 DirAngle(float angleDegrees, bool angleIsGlobal){
		if(!angleIsGlobal){
			angleDegrees += transform.eulerAngles.y;
		}
		//2d assign=(angle,angle,0); 3d assign=(angle,0,angle)
		return new Vector3 (Mathf.Sin (angleDegrees * Mathf.Deg2Rad),0,Mathf.Cos (angleDegrees * Mathf.Deg2Rad));
	}
		
}
