using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class TankVehicle : Vehicle
{
    // POLYMORPHISM
    protected override void DoSteering(AxleInfo axleInfo, float steering)
    {
        if (axleInfo.steering)
        {
            axleInfo.leftWheel.motorTorque += steering * axleInfo.steeringMultiplier;
            axleInfo.rightWheel.motorTorque += -steering * axleInfo.steeringMultiplier;
        }
    }
    protected override void DoSpeeding(AxleInfo axleInfo, float motor)
    {
        base.DoSpeeding(axleInfo, motor);
        float maxTorque = Math.Max(maxMotorTorque, maxSteering);
        axleInfo.leftWheel.motorTorque = Math.Clamp(axleInfo.leftWheel.motorTorque, -maxTorque, maxTorque);
        axleInfo.rightWheel.motorTorque = Math.Clamp(axleInfo.rightWheel.motorTorque, -maxTorque, maxTorque);
    }
}