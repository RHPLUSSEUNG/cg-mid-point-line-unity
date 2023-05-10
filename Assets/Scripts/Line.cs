using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    Vector3 startPointPos, endPointPos;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
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

}
