using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region singleton
    public static CameraShake Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion


    [Header("Default Settings")]

    [SerializeField]
    private AnimationCurve defaultCruve;

    [SerializeField]
    private float defaultDuration;

    [Header("Default Curve Settings")]
    [SerializeField]
    [Tooltip("Default Length For custom in script curves")]
    private float defaultCurveDuration;


    //private floats
    private bool isShaking;




    void Update()
    {
     
    }

    public void Shake()
    {
        StartCoroutine(shakeEnumerator(defaultDuration, defaultCruve));
    }

    #region shakeVariations
    public void Shake(float duration)
    {
        StartCoroutine(shakeEnumerator(duration, defaultCruve));
    }

    public void Shake(AnimationCurve curve)
    {
        StartCoroutine(shakeEnumerator(defaultDuration, curve));
    }

    public void Shake(float duration, AnimationCurve curve)
    {
        StartCoroutine(shakeEnumerator(duration, curve));
    }

    // these create animation curves
    public void Shake(float duration, float pos1)
    {
         
       StartCoroutine(shakeEnumerator(duration, buildCurve(pos1, pos1, defaultCurveDuration)));
    }

    public void Shake(float duration, float pos1, float pos2)
    {

        StartCoroutine(shakeEnumerator(duration, buildCurve(pos1, pos2, defaultCurveDuration)));
    }

    public void Shake(float duration, float pos1, float pos2, float curveDuration)
    {

        StartCoroutine(shakeEnumerator(duration, buildCurve(pos1, pos2, curveDuration)));
    }



    #endregion

    private AnimationCurve buildCurve(float pos1, float pos2, float length)
    {
        AnimationCurve curve;
        curve = new AnimationCurve(new Keyframe(0, pos1), new Keyframe(length, pos2));
        curve.preWrapMode = WrapMode.PingPong;
        return curve;
    }





    private IEnumerator shakeEnumerator(float duration, AnimationCurve curve)
    {

        if (!isShaking)
        {
            isShaking = true;
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / duration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            }

            transform.position = startPosition;
            isShaking = false;
        }
        else
        {
         //   print("already shaking");
        }
    }
}
