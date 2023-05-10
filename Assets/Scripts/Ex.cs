using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "line")
        {
            Debug.Log("hit");
        }
    }
}
