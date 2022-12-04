using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class ModuleState
{
    public float speed;
    public Rotation2d angle = new Rotation2d().FromDegrees(0);
    public ModuleState() { }
    public ModuleState(float speed, Rotation2d angle)
    {
        this.speed = speed;
        this.angle = angle;
    }
    public static ModuleState optimize(ModuleState desiredState, Rotation2d currentAngle)
    {
        var delta = desiredState.angle.minus(currentAngle);
        if (Mathf.Abs(delta.getDegrees()) > 90)
        {
            return new ModuleState(
                -desiredState.speed,
                desiredState.angle.rotateBy(new Rotation2d().FromDegrees(180)));
        }
        else
        {
            return new ModuleState(desiredState.speed, desiredState.angle);
        }
    }
}
