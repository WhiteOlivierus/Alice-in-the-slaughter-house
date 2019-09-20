﻿using UnityEngine;

/// <summary>
/// Pig can carry a object
/// </summary>
public class PigCarry : Actions
{
    //Public variables through blackboard
    private float range = BlackBoard.PigRange;

    [SerializeField]
    private LayerMask layer;
    [SerializeField]
    private Transform carryPoint;

    private Transform carryObject;

    private void Awake()
    {
        commands["Grab"] = true;
        commands["Drop"] = false;
    }

    /// <summary>
    /// Go and grab the clossest object if true,
    /// Else drop the object you have.
    /// </summary>
    /// <param name="active"></param>
    public override void Run(bool active)
    {
        if (active && !carryObject)
        {
            //grab object in range
            RaycastHit[] pickupable;
            pickupable = Physics.SphereCastAll(transform.position, range, Vector3.up, range, layer.value);

            //if there are non return
            if (pickupable.Length < 1) { return; }

            GetClossestObject(pickupable);

            //make the object follow the pig
            Transform closest = carryObject.transform;
            closest.SetParent(transform);
            closest.GetComponent<Rigidbody>().isKinematic = true;
            closest.position = carryPoint.position;
        }
        else
        {
            //drop object
            Transform closest = carryObject.transform;
            closest.parent = null;
            closest.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    /// <summary>
    /// Get the clossest object
    /// </summary>
    /// <param name="pickupable"></param>
    private void GetClossestObject(RaycastHit[] pickupable)
    {
        float smallestDistance = Mathf.Infinity;
        foreach (RaycastHit pickup in pickupable)
        {
            float distance = Vector3.Distance(transform.position, pickup.transform.position);

            //find clossest pickupable
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                carryObject = pickup.transform;
            }
        }
    }

    /// <summary>
    /// Stop what your doing
    /// </summary>
    public override void Stop() { }
}
