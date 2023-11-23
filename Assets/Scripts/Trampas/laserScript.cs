using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {
	public float radius = 0.5f;
	public int vertices = 3;
	public Transform startPoint;
	public Transform endPoint;
	LineRenderer laserLine;
	// Use this for initialization
	void Start () {
		laserLine = GetComponentInChildren<LineRenderer> ();

		laserLine.SetWidth (radius, radius);
		laserLine.numCornerVertices = vertices;
		laserLine.numCapVertices = vertices;

		laserLine.SetPosition(0, startPoint.position);
		laserLine.SetPosition(1, endPoint.position);
	}

	// Update is called once per frame
	void LateUpdate () {
		laserLine.SetPosition (0, startPoint.position);
		laserLine.SetPosition (1, endPoint.position);

	}
}
