using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for a field light, it goes inside the part that is named "Light"
public class FieldLightRandom : MonoBehaviour
{
    private Light TargetLight;
    [Tooltip("X min and X max of the target area")]
    public float[] xRange = {-1, 1};
    [Tooltip("Y min and Y max of the target area")]
    public float[] zRange = {-1, 1};
    public Vector3 targetPos;
    private Quaternion OldRotation = Quaternion.identity;
    private Quaternion NewRotationStep;
    private float PosLerp = 0.0f;

    void Start()
    {
        TargetLight = gameObject.GetComponent<Light>();
    }
    private Vector3 NewTargetPos() { // Find a new random position on the field
        float targetX = Random.Range(xRange[0], xRange[1]);
        float targetZ = Random.Range(zRange[0], zRange[1]);
        return new Vector3(targetX, 0, targetZ);
    }
    // Update is called once per frame
    void Update()
    {
        // increase the position interpolater
        Vector3 relativePos = targetPos - transform.position;
        Quaternion LookAtRotation = Quaternion.LookRotation(relativePos);
        NewRotationStep = Quaternion.Slerp(OldRotation, LookAtRotation, PosLerp);
        transform.rotation = NewRotationStep;
        Debug.DrawLine(transform.position, targetPos, Color.red, 0.0f);
        PosLerp += 0.5f * Time.deltaTime;
        // check if the interpolator has reached the target
        // and set a new target position
        if (PosLerp > 1.0f)
        {
            OldRotation = NewRotationStep;
            targetPos = NewTargetPos();
            PosLerp = 0.0f;
        }
    }
}
