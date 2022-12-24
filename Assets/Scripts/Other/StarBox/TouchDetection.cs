using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchDetection : MonoBehaviour
{
    public UnityEvent trigger;
    public string tagDetect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagDetect)
        {
            trigger.Invoke();
        }
    }
}
