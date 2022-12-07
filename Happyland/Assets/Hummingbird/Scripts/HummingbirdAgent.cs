using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

/// <summary>
/// A hummingbird ML agent
/// </summary>

public class HummingbirdAgent : Unity.MLAgents.Agent
{
    [Tooltip("Force to apply when moving")]
    public float moveForce = 2f;

    [Tooltip("Speed to pitch up or down")]
    public float pitchSpeed = 100f;

    [Tooltip("Speed to rotate around the up axis")]
    public float yawSpeed = 100f;

    [Tooltip("Transform at the tip pf the beak")]
    public Transform beakTip;

    [Tooltip("Whether this is training or gameplay mode")]
    public bool trainingMode;
}
