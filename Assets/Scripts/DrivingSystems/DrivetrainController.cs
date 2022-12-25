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
        public WheelHit hit;
        Quaternion m_SteerlessLocalRotation;

        public string[] debug = new string[8];


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
                    outputBrakeTorque = Mathf.Abs(currentRPM) * brakeTorque;
                } else if (currentRPM > targetRPM && targetRPM > 0 || currentRPM < targetRPM && targetRPM < 0) {//the wheel is spinning too fast, activate the brakes
                    outputMotorTorque = state.speed * motorTorque;
                    outputBrakeTorque = (Mathf.Abs(targetRPM) / Mathf.Abs(currentRPM)) * brakeTorque;
                } else {//wheel is not fast enough, speed up
                    outputMotorTorque = state.speed * motorTorque;
                    outputBrakeTorque = 0;
                }
            } else { //robot is stopped
                outputMotorTorque = 0;
                outputBrakeTorque =  brakeTorque + Mathf.Abs(currentRPM) * brakeTorque;
            }

            collider.brakeTorque = outputBrakeTorque;
            collider.motorTorque = outputMotorTorque;
            collider.steerAngle = outputAngle;
            ApplyPositionToVisuals();
            collider.GetGroundHit(out hit);
            debug[0] = ("OutputTorque " + outputMotorTorque.ToString());
            debug[1] = ("OutputBrake  " + outputBrakeTorque.ToString());
            debug[2] = ("OutputAngle  " + outputAngle.ToString());
            debug[3] = ("TargetRPM    " + targetRPM.ToString());
            debug[4] = ("RPM          " + currentRPM.ToString());
            if (collider.isGrounded){
                debug[5] = ("Force        " + hit.force.ToString());
                debug[6] = ("ForwardSlip  " + hit.forwardSlip.ToString());
                debug[7] = ("SidewaysSlip " + hit.sidewaysSlip.ToString());
            }
        }

        public void Setup() => m_SteerlessLocalRotation = wheelTransform.localRotation;
    }
    
    public SwerveModule Swerve;
    public TankModule Tank;
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

    private InputActionMap Controls;
    private InputAction RobotCentricButton;
    private InputAction PointDirectionButton;
    private InputAction RespawnRobotbutton;

    public String DriveType;
    public bool isRobotCentric;
    public bool RobotCentricButtonEnabled = true;
    public bool PointDirectionButtonEnabled = true;
    public bool ResetPositionButtonEnabled = true;
    public float gyroOffset;
    public float robotYAngle;

    public PlayerDetails playerDetails;
   
    Wheel[] Wheels = {new Wheel(), new Wheel(), new Wheel(), new Wheel()};


    void Awake() {
        Controls = GetComponent<PlayerInput>().actions.FindActionMap("Drivetrain");
        RobotCentricButton = Controls.FindAction("RobotCentricButton", true);
        PointDirectionButton = Controls.FindAction("PointDirectionButton", true);
        RespawnRobotbutton = Controls.FindAction("ResetPositionButton", true);

        RobotCentricButton.performed += ctx => ToggleRobotCentric();
        RobotCentricButton.canceled += ctx => ToggleRobotCentric();
        RespawnRobotbutton.performed += ctx => Respawn();

        Wheels[0] = FL;
        Wheels[1] = FR;
        Wheels[2] = BL;
        Wheels[3] = BR;

        if (Swerve == null) {
            Swerve = new SwerveModule();
        }
        if (Tank == null) {
            Tank = new TankModule();
        }
        playerDetails = GetComponent<PlayerDetails>();
        Controls.Enable();
    }
    void Start() {
        FL.Setup();
        FR.Setup();
        BL.Setup();
        BR.Setup();
        body.centerOfMass = new Vector3(0f, 0f, 0f);

        if (playerDetails.spawn != null)
        {
            transform.position = playerDetails.spawn.position;
            transform.rotation = playerDetails.spawn.rotation;
            gyroOffset = playerDetails.spawn.eulerAngles.y;
            Debug.Log(playerDetails.profileName +" Spawned");
        }
        else
        {
            Debug.LogWarning("Player Spawn Details unasigned", playerDetails);
        }
        for (int i =0; i < 4; i++) {   // make the robots weight the same between all wheels.
            Wheels[i].collider.sprungMass = (body.mass / 4);
        }
    }
    public void OnEnable()
    {
        Controls.Enable();
    }

    public void OnDisable()
    {
        Controls.Disable();
    }
    public void ToggleRobotCentric() {
        if (RobotCentricButtonEnabled) {
            isRobotCentric = !isRobotCentric;
            Debug.Log(playerDetails.profileName +" RobotCentric "+ isRobotCentric.ToString());
        }
    }
    public void Respawn() {
        if (ResetPositionButtonEnabled) {
            transform.position = playerDetails.spawn.position;
            transform.rotation = playerDetails.spawn.rotation;
            gyroOffset = playerDetails.spawn.eulerAngles.y;
            Debug.Log(playerDetails.profileName +" Respawned");
        }
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

        if (DriveType == "Swerve") 
        {
            robotYAngle = GetRobotAngle();
            float verticalInput = Controls.FindAction("LeftY").ReadValue<float>();
            float horizontalInput = Controls.FindAction("LeftX").ReadValue<float>();
            float rotationalInput = Controls.FindAction("RightX").ReadValue<float>();

            debug_throttleVertical = verticalInput;
            debug_throttleHorizontal = horizontalInput;
            debug_turn = rotationalInput;
            ModuleState[] moduleStates = Swerve.UpdateSwerveFromInputs(verticalInput, horizontalInput, rotationalInput, GetRobotAngle() * Mathf.Deg2Rad, isRobotCentric);
            for (int i = 0; i < 4; i++) {
                Wheels[i].Set(i, moduleStates[i], motorTorque, brakeTorque, maxMotorRPM);
            }
        }
        else if (DriveType == "Tank")
        {
            float leftInput = Controls.FindAction("LeftY").ReadValue<float>();
            float rightInput = Controls.FindAction("RightY").ReadValue<float>();
            debug_throttleVertical = leftInput;
            debug_throttleHorizontal = rightInput;
            ModuleState[] moduleStates = Tank.UpdateTankFromInputs(leftInput, rightInput);
            for (int i = 0; i < 4; i++) {
                Wheels[i].Set(i, moduleStates[i], motorTorque, brakeTorque, maxMotorRPM);
            }
        }
    }
}
