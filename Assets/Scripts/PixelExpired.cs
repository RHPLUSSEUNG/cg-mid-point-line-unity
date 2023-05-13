using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelExpired : MonoBehaviour
{
    midpointAl line;
    bool isTrans = false;

    private void Awake()
    {
        line = GameObject.Find("Line").GetComponent<midpointAl>();
    }
    private void Update()
    {
        if (line.GetisTrans())
        {
            this.transform.position = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}
