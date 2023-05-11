using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    Vector3 startPointPos, endPointPos;

    float[][] hitCoor = new float[100][];
    int indexhit = 0;

    private void Awake()
    {
        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        edgeCollider = gameObject.GetComponentInChildren<EdgeCollider2D>();
        UpdateCollider();
    }

    private void Start()
    {
        startPointPos = gameObject.GetComponent<Transform>().position;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, startPointPos);
        lineRenderer.SetPosition(1, GameObject.Find("End Point").GetComponent<Transform>().position);
        UpdateCollider();
    }



    private void UpdateCollider()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        Vector2[] colliderPoints = new Vector2[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            colliderPoints[i] = positions[i];
        }

        edgeCollider.points = colliderPoints;
    }

    public void CalCoord(float x, float y)
    {
        if(x - (int)x == 0 || y - (int)y == 0)
        {
            hitCoor[indexhit][0] = x;
            hitCoor[indexhit][1] = y;
        }
    }
    public void WritePixel(float x, float y, float value)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Cartesian")
        {
            Debug.Log("Hit");
            //ContactPoint contact = collision.GetContact(0);
            //Vector3 collisionPoint = contact.point;

           //Debug.Log("Collision Point: " + collisionPoint);
        
    }
    }
}
