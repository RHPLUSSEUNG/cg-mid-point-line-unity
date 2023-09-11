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
        int numPoints = CalculateNumberOfPoints(start, end); // 선분에 포함될 점들의 개수 계산
        Vector3[] points = new Vector3[numPoints]; // 점들을 저장할 배열 선언

        int currentIndex = 0; // 현재 인덱스 변수 초기화
        float error = 0; // 오차 변수 초기화
        float deltaError = Mathf.Abs((end.y - start.y) / (end.x - start.x)); // 기울기의 절댓값 계산

        if (deltaError > 1) // 기울기가 1 이상인 경우 x, y 반전
        {
            deltaError = Mathf.Abs((end.x - start.x) / (end.y - start.y)); // 기울기 절댓값 x,y 반전 계산

            int xStep = (start.y < end.y) ? 1 : -1;

            int x = Mathf.RoundToInt(start.x); //반올림
            int y = Mathf.RoundToInt(start.y);

            while (y <= Mathf.RoundToInt(end.y)) // y값이 끝점의 y값보다 작거나 같을 때까지 반복
            {
                points[currentIndex] = new Vector3(x, y, 0); // 현재 좌표를 배열에 저장

                currentIndex++; 

                error += deltaError; // 오차 갱신
                if (error >= 0.5f) // 오차가 0.5 이상이면
                {
                    x += xStep; // x값을 증가 방향으로 이동
                    error -= 1; // 오차에서 1을 빼줌
                }

                y++;

            }
        }
        else //기울기가 1 이하
        {
            int yStep = (start.y < end.y) ? 1 : -1;

            int x = Mathf.RoundToInt(start.x); //반올림
            int y = Mathf.RoundToInt(start.y);

            while (x <= Mathf.RoundToInt(end.x)) //x값 정수만큼 반복
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

    int CalculateNumberOfPoints(Vector3 start, Vector3 end) //pixel 수 계산
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

        
        Matrix4x4 translationMatrix = Matrix4x4.Translate(new Vector3(translationVector.x, translationVector.y, 0f)); // 동차 좌표 변환 행렬 생성
        
        startPoint = translationMatrix.MultiplyPoint3x4(startPoint);// startPoint와 endPoint에 이동 변환을 적용
        endPoint = translationMatrix.MultiplyPoint3x4(endPoint);
        
        startPoint.z = 0f; // Z 축 값은 2D에서 사용되지 않으므로 0으로 설정
        endPoint.z = 0f;

        originLine.SetOriginalLine(startPoint, endPoint);
        MidpointLineAlgorithm(startPoint, endPoint);
        SetPixel();

    }

    void SetPixel() //Pixel 배정
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
