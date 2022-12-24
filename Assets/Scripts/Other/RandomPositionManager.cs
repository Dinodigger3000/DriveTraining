using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionManager : MonoBehaviour
{

    public static RandomPositionManager instance;
    public Transform topLeft;
    public Transform bottomRight;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Vector3 randomVector()
    {
        float x = Random.Range(topLeft.position.x, bottomRight.position.x);
        float z = Random.Range(topLeft.position.z, bottomRight.position.z);
        //     print(new Vector2(x, z));
        return new Vector3(x, 0, z);
    }
}
