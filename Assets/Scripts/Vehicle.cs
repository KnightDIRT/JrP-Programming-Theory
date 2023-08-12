using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
    public float steeringMultiplier = 1;
}

public class Vehicle : MonoBehaviour
{
    [SerializeField] protected List<AxleInfo> axleInfos;
    [SerializeField] protected float maxMotorTorque;
    [SerializeField] protected float maxRPM;
    [SerializeField] protected float maxSteering;
    [SerializeField] protected float brakeTorque;

    [SerializeField] private float measuredSpeed;

    [SerializeField] private float _measuredAvgRPM;

    // ENCAPSULATION
    private float measuredAvgRPM
    {
        get { return _measuredAvgRPM; }
        set { _measuredAvgRPM = Math.Abs(value); }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
    }

    public void FixedUpdate()
    {
        measuredSpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        float totalRPM = 0;

        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteering * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.motorTorque = 0;
            axleInfo.rightWheel.motorTorque = 0;
            DoSteering(axleInfo, steering);
            DoSpeeding(axleInfo, motor);
            DoBraking(axleInfo);
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            totalRPM += axleInfo.leftWheel.rpm;
            totalRPM += axleInfo.rightWheel.rpm;
        }

        measuredAvgRPM = totalRPM / (axleInfos.Count * 2);
    }

    // ABSTRACTION
    protected virtual void DoSteering(AxleInfo axleInfo, float steering)
    {
        if (axleInfo.steering)
        {
            axleInfo.leftWheel.steerAngle = steering * axleInfo.steeringMultiplier;
            axleInfo.rightWheel.steerAngle = steering * axleInfo.steeringMultiplier;
        }
    }

    protected virtual void DoSpeeding(AxleInfo axleInfo, float motor)
    {
        if (axleInfo.motor && axleInfo.leftWheel.rpm <= maxRPM && motor > 0)
        {
            axleInfo.leftWheel.motorTorque += motor;
        }
        if (axleInfo.motor && axleInfo.leftWheel.rpm >= -maxRPM && motor < 0)
        {
            axleInfo.leftWheel.motorTorque += motor;
        }
        if (axleInfo.motor && axleInfo.rightWheel.rpm <= maxRPM && motor > 0)
        {
            axleInfo.rightWheel.motorTorque += motor;
        }
        if (axleInfo.motor && axleInfo.rightWheel.rpm >= -maxRPM && motor < 0)
        {
            axleInfo.rightWheel.motorTorque += motor;
        }
    }

    protected virtual void DoBraking(AxleInfo axleInfo)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            axleInfo.leftWheel.brakeTorque = brakeTorque;
            axleInfo.rightWheel.brakeTorque = brakeTorque;
        }
        else
        {
            axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.rightWheel.brakeTorque = 0;
        }
    }
}


