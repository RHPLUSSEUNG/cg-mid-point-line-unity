using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmManager : MonoBehaviour
{
    [Header("Environment Element")]
    [SerializeField] Button startButton;
    [SerializeField] Button translationButton;
    [SerializeField] Button transStartButton;
    [SerializeField] GameObject preStartObject;
    [SerializeField] GameObject inGameObject;

    [Header("Line Element")]
    [SerializeField] TMP_InputField startX;
    [SerializeField] TMP_InputField startY;
    [SerializeField] TMP_InputField endX;
    [SerializeField] TMP_InputField endY;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float lineWidth;

    private LineRenderer lineRenderer;

    OriginalLine originLine;

    [Header("Pixel Element")]
    public GameObject pixelPrefab;
    public float pixelSize = 0.1f;

    [Header("Translation Element")]
    [SerializeField] TMP_InputField transX;
    [SerializeField] TMP_InputField transY;
    public Vector3 translationVector;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        originLine = GameObject.Find("Original Line").GetComponent<OriginalLine>();
    }

    private void Start()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        inGameObject.SetActive(false);
    }

    void MidpointLineAlgorithm(Vector3 start, Vector3 end) //Midpoint Line Algorithm
    {
        int numPoints = CalculateNumberOfPoints(start, end); // ���п� ���Ե� ������ ���� ���
        Vector3[] points = new Vector3[numPoints]; // ������ ������ �迭 ����

        int currentIndex = 0; // ���� �ε��� ���� �ʱ�ȭ
        float error = 0; // ���� ���� �ʱ�ȭ
        float deltaError = Mathf.Abs((end.y - start.y) / (end.x - start.x)); // ������ ���� ���

        if (deltaError > 1) // ���Ⱑ 1 �̻��� ��� x, y ����
        {
            deltaError = Mathf.Abs((end.x - start.x) / (end.y - start.y)); // ���� ���� x,y ���� ���

            int xStep = (start.y < end.y) ? 1 : -1;

            int x = Mathf.RoundToInt(start.x); //�ݿø�
            int y = Mathf.RoundToInt(start.y);

            while (y <= Mathf.RoundToInt(end.y)) // y���� ������ y������ �۰ų� ���� ������ �ݺ�
            {
                points[currentIndex] = new Vector3(x, y, 0); // ���� ��ǥ�� �迭�� ����

                currentIndex++; 

                error += deltaError; // ���� ����
                if (error >= 0.5f) // ������ 0.5 �̻��̸�
                {
                    x += xStep; // x���� ���� �������� �̵�
                    error -= 1; // �������� 1�� ����
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

    public void TranslateObject() //Translation
    {
        inGameObject.SetActive(false);
        translationButton.gameObject.SetActive(true);

        ObjectPooler.Instance.PixelExpired();

        translationVector = new Vector3(float.Parse(transX.text), float.Parse(transY.text), 0);

        
        Matrix4x4 translationMatrix = Matrix4x4.Translate(new Vector3(translationVector.x, translationVector.y, 0f)); // ���� ��ǥ ��ȯ ��� ����
        
        startPoint = translationMatrix.MultiplyPoint3x4(startPoint);// startPoint�� endPoint�� �̵� ��ȯ�� ����
        endPoint = translationMatrix.MultiplyPoint3x4(endPoint);
        
        startPoint.z = 0f; // Z �� ���� 2D���� ������ �����Ƿ� 0���� ����
        endPoint.z = 0f;

        originLine.SetOriginalLine(startPoint, endPoint);
        MidpointLineAlgorithm(startPoint, endPoint);
        SetPixel();

    }

    void SetPixel() //Pixel ����
    {
        Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);

        for (int i = 0; i < linePositions.Length; i++)
        {
            Vector3 pointPosition = linePositions[i];
            
            GameObject _circle = ObjectPooler.Instance.GetPixel(PixelType.PIXEL);
            _circle.transform.position = pointPosition;
            _circle.transform.localScale = new Vector3(pixelSize, pixelSize, 1f);
            _circle.SetActive(true);
        }

    }

    public void LineStart()
    {
        startPoint = new Vector3(float.Parse(startX.text), float.Parse(startY.text), 0);
        endPoint = new Vector3(float.Parse(endX.text), float.Parse(endY.text), 0);

        preStartObject.SetActive(false);

        originLine.SetOriginalLine(startPoint, endPoint);
        MidpointLineAlgorithm(startPoint, endPoint);
        SetPixel();
    }

    public void ActiveTransMenu()
    {
        inGameObject.SetActive(true);
        translationButton.gameObject.SetActive(false);
    }

    public void Retry()
    {
        startX.text = "";
        startY.text = "";
        endX.text = "";
        endY.text = "";
        originLine.SetOriginalLine(new Vector3(0, 0, 0), new Vector3(0,0,0));
        preStartObject.SetActive(true);
        inGameObject.SetActive(false);
        ObjectPooler.Instance.PixelExpired();   
    }

    public void GameExit()
    {
        Application.Quit();
    }


}
