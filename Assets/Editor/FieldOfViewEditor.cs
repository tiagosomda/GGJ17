using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof (FieldOfView))]
public class FieldOfViewEditor :Editor {

	void OnSceneGUI(){
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.blue;

		//2d Assign: (posit,Vector3.forward,vector3.up,360,radius) 3d Assign:(posit,Vector3.Forward,Vector3.Up,360,radius)

		Handles.DrawWireArc (fow.transform.position,Vector3.up,Vector3.forward, 360, fow.viewRadius);
		Vector3 viewAngleA = fow.DirAngle (-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirAngle (fow.viewAngle / 2, false);

		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		Handles.color = Color.green;


		foreach (Transform visTargs in fow.visTargets) {
			Handles.DrawLine (fow.transform.position, visTargs.position);
		}
	}
}
