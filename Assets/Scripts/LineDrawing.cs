using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineDrawing : MonoBehaviour
{
    [Header("Cartesian Coordinate Element")]
    public LineRenderer cartesianLine;
    public Vector2Int origin = new Vector2Int(0, 0); // 좌표계의 원점
    public Vector2Int xAxisEnd = new Vector2Int(10, 0); // x축의 끝점
    public Vector2Int yAxisEnd = new Vector2Int(0, 10); // y축의 끝점

    [Header("Line Drawing Element")]
    public LineRenderer lineRenderer;
    public InputField startX;
    public InputField startY;
    public InputField endX;
    public InputField endY;

    
    

    void Start()
    {
        // 직각 좌표계 그리기
        cartesianLine.positionCount = 2;
        cartesianLine.SetPositions(new Vector3[] { new Vector3(origin.x, origin.y, 0f), new Vector3(xAxisEnd.x, xAxisEnd.y, 0f) });
        cartesianLine.positionCount += 1;
        cartesianLine.SetPosition(lineRenderer.positionCount - 1, new Vector3(yAxisEnd.x, yAxisEnd.y, 0f));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 start = new Vector3(float.Parse(startX.text), float.Parse(startY.text), 0f);
            Vector3 end = new Vector3(float.Parse(endX.text), float.Parse(endY.text), 0f);
            DrawLine(start, end);
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        float dx = Mathf.Abs(end.x - start.x);
        float dy = Mathf.Abs(end.y - start.y);
        float m = dy / dx;

        float error = 0f;
        int y = (int)start.y;

        for (int x = (int)start.x; x <= (int)end.x; x++)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(x, y, 0f));

            error += m;
            if (error >= 0.5f)
            {
                y += (int)Mathf.Sign(end.y - start.y);
                error -= 1f;
            }
        }
    }
}
