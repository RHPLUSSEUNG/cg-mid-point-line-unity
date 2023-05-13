using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    Button startButton;

    public void Load()
    {
        SceneManager.LoadScene("DrawingScene");
    }
}
