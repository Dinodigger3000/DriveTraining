using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;


public class Rotation2d
{
    private float m_value;
    private float m_cos;
    private float m_sin;

    public Rotation2d()
    {
        m_value = 0.0f;
        m_cos = 1.0f;
        m_sin = 0.0f;
    }
    public Rotation2d(float radians)
    {
        m_value = radians;
        m_cos = Mathf.Cos(radians);
        m_sin = Mathf.Sin(radians);
    }
    public Rotation2d(float x, float y)
    {
        float magnitude = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)); ;
        if (magnitude > 1e-6)
        {
            m_sin = y / magnitude;
            m_cos = x / magnitude;
        }
        else
        {
            m_sin = 0.0f;
            m_cos = 1.0f;
        }
        m_value = Mathf.Atan2(m_sin, m_cos);
    }
    public Rotation2d fromRadians(float radians)
    {
        return new Rotation2d(radians);
    }
    public Rotation2d FromDegrees(float degrees)
    {
        return new Rotation2d(degrees * Mathf.Deg2Rad);
    }
    public float GetCos()
    {
        return m_cos;
    }
    public float GetSin()
    {
        return m_sin;
    }
    public Rotation2d plus(Rotation2d other)
    {
        return rotateBy(other);
    }
    public Rotation2d rotateBy(Rotation2d other)
    {
        return new Rotation2d(m_cos * other.m_cos - m_sin * other.m_sin, m_cos * other.m_sin + m_sin * other.m_cos);
    }
    public Rotation2d minus(Rotation2d other)
    {
        return rotateBy(other.unaryMinus());
    }
    public Rotation2d unaryMinus()
    {
        return new Rotation2d(-m_value);
    }
    public Rotation2d times(float scalar)
    {
        return new Rotation2d(m_value * scalar);
    }
    public float getDegrees()
    {
        return (Mathf.Rad2Deg * m_value);
    }
}

