using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {
    private const float radius = 2.25f;
	private const int vertices = 8;
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
