using UnityEngine;

public class midpointAl : MonoBehaviour
{
    [Header("Line Element")]
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float lineWidth;

    private LineRenderer lineRenderer;

    OriginalLine originLine;

    [Header("Pixel Element")]
    public GameObject pixelPrefab; // Circle Sprite�� Prefab�� �Ҵ��մϴ�.
    public float pixelSize = 0.1f; // Circle Sprite�� ũ�⸦ �����մϴ�.


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        originLine = GameObject.Find("Original Line").GetComponent<OriginalLine>();

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        originLine.SetOriginalLine(startPoint, endPoint);
        DrawLine(startPoint, endPoint);
        SetPixel();
    }

    private void Update()
    {

    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        int numPoints = CalculateNumberOfPoints(start, end);
        Vector3[] points = new Vector3[numPoints];

        int currentIndex = 0;
        float error = 0;
        float deltaError = Mathf.Abs((end.y - start.y) / (end.x - start.x)); //����

        if(deltaError > 1) //���Ⱑ 1 �̻�
        {
            deltaError = Mathf.Abs((end.x - start.x) / (end.y - start.y));

            int xStep = (start.y < end.y) ? 1 : -1;

            int x = Mathf.RoundToInt(start.x); //�ݿø�
            int y = Mathf.RoundToInt(start.y);


            while (y <= Mathf.RoundToInt(end.y)) //y�� ������ŭ �ݺ�
            {
                points[currentIndex] = new Vector3(x, y, 0);

                currentIndex++;

                error += deltaError;
                if (error >= 0.5f)
                {
                    x += xStep;
                    error -= 1;
                }

                y++;

            }
        }
        else //���Ⱑ 1 ����
        {
            int yStep = (start.y < end.y) ? 1 : -1;

            int x = Mathf.RoundToInt(start.x); //�ݿø�
            int y = Mathf.RoundToInt(start.y);


            while (x <= Mathf.RoundToInt(end.x)) //x�� ������ŭ �ݺ�
            {
                points[currentIndex] = new Vector3(x, y, 0);

                currentIndex++;

                error += deltaError;
                if (error >= 0.5f)
                {
                    y += yStep;
                    error -= 1;
                }

                x++;

            }
        }
        

        lineRenderer.positionCount = numPoints;
        lineRenderer.SetPositions(points);        

    }

    int CalculateNumberOfPoints(Vector3 start, Vector3 end) //pixel �� ���
    {
        int deltaX = Mathf.RoundToInt(end.x - start.x);
        int deltaY = Mathf.RoundToInt(end.y - start.y);
        return Mathf.Max(Mathf.Abs(deltaX), Mathf.Abs(deltaY)) + 1;
    }

    void SetPixel()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pointPosition = lineRenderer.GetPosition(i);

            GameObject circle = Instantiate(pixelPrefab, pointPosition, Quaternion.identity);
            circle.transform.localScale = new Vector3(pixelSize, pixelSize, 1f);
        }

    }

    void SetCartesian(Vector3 _startPoint, Vector3 _endPoint)
    {
        /*
        int deltaX = Mathf.RoundToInt(end.x - start.x);
        int deltaY = Mathf.RoundToInt(end.y - start.y);
        int maxX = Mathf.Max(Mathf.Abs(_startPoint.x), Mathf.Abs(_endPoint.y));
        */
    }
}
