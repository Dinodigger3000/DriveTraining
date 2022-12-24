using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankModule
{
    public ModuleState[] UpdateTankFromInputs(float leftInput, float rightInput) {
        float[] wheelSpeed = {0, 0, 0, 0};
        Rotation2d[] newWheelAngle = new Rotation2d[4];
        ModuleState[] states = new ModuleState[4];

        // Wheel FL
        wheelSpeed[0] = leftInput;
        newWheelAngle[0] = new Rotation2d(0);
        // Wheel FR
        wheelSpeed[1] = rightInput;
        newWheelAngle[1] = new Rotation2d(0);
        // Wheel BL
        wheelSpeed[2] = leftInput;
        newWheelAngle[2] = new Rotation2d(0);
        // Wheel BR
        wheelSpeed[3] = rightInput;
        newWheelAngle[3] = new Rotation2d(0);

        for (int i = 0; i < 4; i++) {
            states[i] = new ModuleState(wheelSpeed[i], newWheelAngle[i]);
        }
        return states;
    }
}