using System;
using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine.InputSystem;

  
public class DrivetrainController : MonoBehaviour {

    [Serializable] public class Wheel {
        [Tooltip("A reference to the transform of the wheel.")]
        public Transform wheelTransform;
        [Tooltip("A reference to the WheelCollider of the wheel.")]
        public WheelCollider collider;
        
        Quaternion m_SteerlessLocalRotation;

        public string[] debug = new string[5];


        public void ApplyPositionToVisuals() {
            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);
            wheelTransform.transform.position = position;
            wheelTransform.transform.rotation = rotation;
        }

        public void Set(int index, ModuleState state, float motorTorque, float brakeTorque, float maxMotorRPM) {
            //                      -1 - 1             5                 5                 10
            float outputAngle = state.angle.getDegrees();                 
            float outputMotorTorque = 0;
            float outputBrakeTorque = 0;

            float targetRPM = state.speed * maxMotorRPM; 
            //       -10            -1             10         
            float currentRPM = collider.rpm;
            //        -15                     
            if (Mathf.Abs(state.speed) >= 0.01f) { //Robot is not stopped
                if ((targetRPM < 0 && currentRPM > 0) || (targetRPM > 0 && currentRPM < 0)) {//Robot is moving the wrong way, use brakes
                    outputMotorTorque = state.speed * motorTorque;
                    outputBrakeTorque = brakeTorque;
                // } else if (Mathf.Abs(currentRPM) > Mathf.Abs(targetRPM)) {//the wheel is spinning too fast, slow down 
                //     Debug.Log("slowing down wheel");
                //     outputMotorTorque = 0;
                //     outputBrakeTorque = Mathf.Abs(currentRPM) - Mathf.Abs(targetRPM);
                } else {//wheel is not fast enough, speed up
                    outputMotorTorque = state.speed * motorTorque;
                    outputBrakeTorque = 0;
                }
            } else { //robot is stopped
                outputMotorTorque = 0;
                outputBrakeTorque = brakeTorque;
            }

            collider.brakeTorque = outputBrakeTorque;
            collider.motorTorque = outputMotorTorque;
            collider.steerAngle = outputAngle;
            ApplyPositionToVisuals();
            debug[0] = ("OutputTorque " + outputMotorTorque.ToString());
            debug[1] = ("OutputBrake  " + outputBrakeTorque.ToString());
            debug[2] = ("OutputAngle  " + outputAngle.ToString());
            debug[3] = ("TargetRPM    " + targetRPM.ToString());
            debug[4] = ("RPM          " + currentRPM.ToString());

        
            // print(index + "," + outputMotorTorque + "," + outputAngle + "," + Mathf.Abs(collider.rpm)/maxMotorRPM + "," + ((state.speed * maxMotorRPM)-collider.rpm));
        }

        public void Setup() => m_SteerlessLocalRotation = wheelTransform.localRotation;
    }
    
    public String DriveType;
    public SwerveModule Swerve;
    public Wheel FL;
    public Wheel FR;
    public Wheel BL;
    public Wheel BR;
    public Rigidbody body;
    
    public float motorTorque;
    public float brakeTorque;
    public float maxMotorRPM;
    
    public float debug_throttleVertical;
    public float debug_throttleHorizontal;
    public float debug_turn;

    private InputActionMap controls;
    private float gyroOffset;
    public float robotYAngle;

    public PlayerDetails playerDetails;

    // public Wheel[] test = new Wheel[4]; new way of instantiating arrays
    
    Wheel[] Wheels = {new Wheel(), new Wheel(), new Wheel(), new Wheel()};


    void Awake() {
        Wheels[0] = FL;
        Wheels[1] = FR;
        Wheels[2] = BL;
        Wheels[3] = BR;

        controls = GetComponent<PlayerInput>().actions.FindActionMap("Swerve");

        playerDetails = GetComponent<PlayerDetails>();
    }
    void Start() {
        FL.Setup();
        FR.Setup();
        BL.Setup();
        BR.Setup();
        body.centerOfMass = new Vector3(0f, 0f, 0f);
        if (DriveType == "Swerve") {
            InitializeSwerve();
        }
        transform.position = playerDetails.spawn.position;
        transform.rotation = playerDetails.spawn.rotation;
        gyroOffset = playerDetails.spawn.eulerAngles.y;
        for (int i =0; i < 4; i++) {   // make the robots weight the same between all wheels.
            Wheels[i].collider.sprungMass = (body.mass / 4);
        }
    }

    public void OnEnable()
    {
        controls.Enable();
    }

    public void OnDisable()
    {
        controls.Disable();
    }
    public SwerveModule InitializeSwerve() {
        if (Swerve == null) {
            Swerve = new SwerveModule();
        }
        return Swerve;
    }
    public float GetRobotAngle() {
        float rot = transform.eulerAngles.y + gyroOffset;
        if (rot <= 180f) {
            return rot;
        } else {
            return rot - 360f;
        }
    }

    void FixedUpdate() {
        robotYAngle = GetRobotAngle();
        float verticalInput = controls.FindAction("VerticalAxis").ReadValue<float>();
        float horizontalInput = controls.FindAction("HorizontalAxis").ReadValue<float>();
        float rotationalInput = controls.FindAction("RotationalAxis").ReadValue<float>();

        debug_throttleVertical = verticalInput;
        debug_throttleHorizontal = horizontalInput;
        debug_turn = rotationalInput;
        if (DriveType == "Swerve") 
        {
            ModuleState[] moduleStates = Swerve.UpdateSwerveFromInputs(verticalInput, horizontalInput, rotationalInput, GetRobotAngle() * Mathf.Deg2Rad);
            for (int i = 0; i < 4; i++) {
                Wheels[i].Set(i, moduleStates[i], motorTorque, brakeTorque, maxMotorRPM);
            }
        }
    }
}
