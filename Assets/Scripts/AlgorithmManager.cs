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
    public GameObject pixelPrefab; // Circle Sprite의 Prefab을 할당합니다.
    public float pixelSize = 0.1f; // Circle Sprite의 크기를 조정합니다.
    bool isTrans;

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

        isTrans = false;
        inGameObject.SetActive(false);
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
        float deltaError = Mathf.Abs((end.y - start.y) / (end.x - start.x)); //절댓값

        if(deltaError > 1) //기울기가 1 이상
        {
            deltaError = Mathf.Abs((end.x - start.x) / (end.y - start.y));

            int xStep = (start.y < end.y) ? 1 : -1;

            int x = Mathf.RoundToInt(start.x); //반올림
            int y = Mathf.RoundToInt(start.y);


            while (y <= Mathf.RoundToInt(end.y)) //y값 정수만큼 반복
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
    public void TranslateObject()
    {
        inGameObject.SetActive(false);
        translationButton.gameObject.SetActive(true);

        isTrans = true;                

        translationVector = new Vector3(float.Parse(transX.text), float.Parse(transY.text), 0);

        // 동차 좌표 변환 행렬 생성
        Matrix4x4 translationMatrix = Matrix4x4.Translate(new Vector3(translationVector.x, translationVector.y, 0f));

        // 현재 객체의 위치를 가져옴
        Vector3 currentPosition = transform.position;

        // startPoint와 endPoint에 이동 변환을 적용
        startPoint = translationMatrix.MultiplyPoint3x4(startPoint);
        endPoint = translationMatrix.MultiplyPoint3x4(endPoint);

        // Z 축 값은 2D에서 사용되지 않으므로 0으로 설정
        startPoint.z = 0f;
        endPoint.z = 0f;

        originLine.SetOriginalLine(startPoint, endPoint);
        DrawLine(startPoint, endPoint);
        SetPixel();

    }

    void SetPixel()
    {
        isTrans = false;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pointPosition = lineRenderer.GetPosition(i);

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
        DrawLine(startPoint, endPoint);
        SetPixel();
    }
    
    public void ActiveTransMenu()
    {
        inGameObject.SetActive(true);
        translationButton.gameObject.SetActive(false);
    }
    public bool GetisTrans()
    {
        return isTrans;
    }

    
    public void GameExit()
    {
        Application.Quit();
    }


}
