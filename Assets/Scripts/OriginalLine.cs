using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalLine : MonoBehaviour
{
    LineRenderer originLr;
    [SerializeField] float lineWidth;
    private void Start()
    {
        originLr = GetComponent<LineRenderer>();
        originLr.startWidth = lineWidth;
        originLr.endWidth = lineWidth;

    }

    public void SetOriginalLine(Vector3 _startPoint, Vector3 _endPoint)
    {        
        originLr.SetPosition(0, _startPoint);
        originLr.SetPosition(1, _endPoint);
    }
}
