using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoad : MonoBehaviour
{
    Button startButton;
    [SerializeField] TMP_InputField startX, startY, endX, endY;
    
    public void Load()
    {
        SceneManager.LoadScene("DrawingScene");
    }

    public void PointRelay()
    {

    }
}
