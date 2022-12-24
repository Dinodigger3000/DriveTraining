using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveModule
{
    public Rotation2d[] oldWheelAngle = {new Rotation2d(), new Rotation2d(), new Rotation2d(), new Rotation2d()};
    public ModuleState[] UpdateSwerveFromInputs(float verticalInput, float horizontalInput, float rotationalInput, float robotAngle, bool isRobotCentric) { //swerveOnly
        if (!isRobotCentric) { // The inputs are field centric, convert them to be robot relative
            float temp = verticalInput * Mathf.Cos(robotAngle) + horizontalInput*Mathf.Sin(robotAngle);
            horizontalInput = -verticalInput*Mathf.Sin(robotAngle) + horizontalInput * Mathf.Cos(robotAngle);
            verticalInput = temp;
        }

        float A = horizontalInput - rotationalInput * 0.707f;
        float B = horizontalInput + rotationalInput * 0.707f;
        float C = verticalInput - rotationalInput * 0.707f;
        float D = verticalInput + rotationalInput * 0.707f;

        float[] wheelSpeed = {0, 0, 0, 0};
        Rotation2d[] newWheelAngle = new Rotation2d[4];
        ModuleState[] states = new ModuleState[4];

        wheelSpeed[0] = Mathf.Sqrt(Mathf.Pow(B,2) + Mathf.Pow(D,2));
        newWheelAngle[0] = new Rotation2d().FromDegrees(Mathf.Atan2(B,D) * 180/Mathf.PI);

        wheelSpeed[1] = Mathf.Sqrt(Mathf.Pow(B,2) + Mathf.Pow(C,2));
        newWheelAngle[1] = new Rotation2d().FromDegrees(Mathf.Atan2(B,C) * 180/Mathf.PI);

        wheelSpeed[2] = Mathf.Sqrt(Mathf.Pow(A,2) + Mathf.Pow(D,2));
        newWheelAngle[2] = new Rotation2d().FromDegrees(Mathf.Atan2(A,D) * 180/Mathf.PI);

        wheelSpeed[3] = Mathf.Sqrt(Mathf.Pow(A,2) + Mathf.Pow(C,2));
        newWheelAngle[3] = new Rotation2d().FromDegrees(Mathf.Atan2(A,C) * 180/Mathf.PI);
        
        float maxSpeed = 0f;
        for (int i = 0; i < 4; i++) {
            if (wheelSpeed[i] > maxSpeed) {
                maxSpeed = wheelSpeed[i];
            }
        }
        if (maxSpeed > 1) { // desaturate wheel speeds
            for (int i = 0; i < 4; i++) {
                wheelSpeed[i] = wheelSpeed[i] / maxSpeed;
            }
        }

        if (maxSpeed <= 0.01) { // if the robot isn't moving, don't move or change steering
            for (int i = 0; i < 4; i++) {
                states[i] = new ModuleState(0, oldWheelAngle[i]);
            }
        } else { // update the wheel speed and angles
            for (int i = 0; i < 4; i++) {
                states[i] = new ModuleState(wheelSpeed[i], newWheelAngle[i]);
                states[i] = ModuleState.optimize(states[i], oldWheelAngle[i]);
                oldWheelAngle[i] = states[i].angle;
            }
        }
        
        return states;
    }
}