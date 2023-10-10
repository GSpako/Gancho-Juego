using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    private SpringJoint springjoint;
    private LineRenderer rope;

    [Header("References")]
    [SerializeField] private GameObject grappleGunTip;

    [Header("Parameters")]
    [SerializeField] private float grappleLength;
    public float ropewidth = 0.01f;

    [Header("Inputs")]
    [SerializeField] private KeyCode grappleB1 = KeyCode.Mouse0;
    [SerializeField] private KeyCode grappleB2 = KeyCode.Mouse1;

    private void Start()
    {

    }
    private void Update()
    {
        if (rope != null)
        {
            rope.SetPositions(new Vector3[] { grappleGunTip.transform.position, springjoint.connectedAnchor });
        }
        if (Input.GetKeyDown(grappleB1)) {
            throwGrapple();
        }
        if (Input.GetKeyUp(grappleB1))
        {
            stopGrapple();
        }
    }
    public void throwGrapple() {
        RaycastHit hit;
        string[] layers = {"whatIsGrappleable","whatIsGround","whatIsWall"};
        LayerMask mask = LayerMask.GetMask(layers);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, grappleLength, mask))
        {
            Vector3 pos = hit.point;
            Visualize(pos);

            Debug.DrawLine(start: Camera.main.transform.position, end: pos, duration: 0.1f, color: Color.white, depthTest: true);

            springjoint = gameObject.AddComponent<SpringJoint>();
            springjoint.autoConfigureConnectedAnchor = false;

            springjoint.connectedAnchor = pos;

            float distance = Vector3.Distance(transform.position, pos);
            springjoint.minDistance = distance * 0.25f;
            springjoint.maxDistance = distance * 0.8f;

            springjoint.spring = 4.5f;
            springjoint.damper = 7f;
            springjoint.massScale = 4.5f;

            rope = gameObject.AddComponent<LineRenderer>();
            rope.SetWidth(ropewidth, ropewidth);
        }
        else Debug.Log("Demasiado lejos!");
    }
    public void stopGrapple()
    {
        Destroy(springjoint);
        Destroy(rope);
    }
    void Visualize(Vector3 pos) {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = Vector3.one;
        go.transform.position = pos;
        Destroy(go, 10);
    }
}
