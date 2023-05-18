using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PixelType
{
    PIXEL,
    CARTESIAN,
    MAX_SIZE,
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [SerializeField] GameObject pixel;
    [SerializeField] int pixelPoolCount;

    [SerializeField] GameObject cartesian;
    [SerializeField] int cartesianPoolCount;


    List<GameObject>[] pixelPool = new List<GameObject>[(int)PixelType.MAX_SIZE];

    private void Awake()
    {
        Instance = this;
        
        pixelPool[(int)PixelType.PIXEL] = new List<GameObject>();
        pixelPool[(int)PixelType.CARTESIAN] = new List<GameObject>();

        for (int i = 0; i < pixelPoolCount; i++)
        {
            GameObject _pixel = Instantiate(pixel);
            _pixel.SetActive(false);
            _pixel.transform.SetParent(transform);

            pixelPool[(int)PixelType.PIXEL].Add(_pixel);
        }

        for (int i = 0; i < cartesianPoolCount; i++)
        {
            GameObject _cartesian = Instantiate(cartesian);
            _cartesian.SetActive(false);
            _cartesian.transform.SetParent(transform);

            pixelPool[(int)PixelType.CARTESIAN].Add(_cartesian);
        }
    }

    public GameObject GetPixel(PixelType _pixelType)
    {
        if (pixelPool[(int)_pixelType] == null)
        {
            return null;
        }

        for (int i = 0; i < pixelPool[(int)_pixelType].Count; i++)
        {
            if (!pixelPool[(int)_pixelType][i].activeInHierarchy)
            {
                return pixelPool[(int)_pixelType][i];
            }
        }
        return null;
    }

    public void PixelExpired()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.transform.position = Vector3.zero;
            child.gameObject.SetActive(false);
        }
    }
}