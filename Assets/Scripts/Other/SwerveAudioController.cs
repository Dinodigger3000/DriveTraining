using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwerveAudioController : MonoBehaviour
{

    private InputActionMap controls;
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.Play("Move");
        controls = GetComponent<PlayerInput>().actions.FindActionMap("Swerve");
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator wait()
        {
        yield return new WaitForSeconds(waitTime);


        float verticalInput = controls.FindAction("VerticalAxis").ReadValue<float>();
        float horizontalInput = controls.FindAction("HorizontalAxis").ReadValue<float>();
        float rotationalInput = controls.FindAction("RotationalAxis").ReadValue<float>();

        if (verticalInput != 0 || horizontalInput != 0 || rotationalInput != 0)
        {
            AudioManager.instance.Play("Move");
        }
        StartCoroutine(wait());
    }
}
