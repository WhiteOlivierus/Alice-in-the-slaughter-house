using System;
using UnityEngine;

/// <summary>
/// Pig can carry a object
/// </summary>
public class PigCarry : Actions
{
    //Public variables through blackboard
    private float range = BlackBoard.PigRange;

    [SerializeField]
    private LayerMask layerPickup;
    [SerializeField]
    private LayerMask layerDrop;
    [SerializeField]
    private Transform carryPoint;
    private RangeCounter counter;
    private Transform carryObject;

    private void Awake()
    {
        commands["Grab"] = true;
        commands["Drop"] = false;
        idle = FindObjectOfType<PigIdle>();
        counter = FindObjectOfType<RangeCounter>();
    }

    /// <summary>
    /// Go and grab the closest object if true,
    /// Else drop the object you have.
    /// </summary>
    /// <param name="param"></param>
    public override void Run(dynamic param)
    {
        if (param == true && !carryObject)
        {
            //grab object in range
            GrabObject();
            Stop();
        }
        else if (carryObject)
        {
            //drop object
            DropObject();
            Stop();
        }
        if (idle)
            idle.isActive = false;
    }

    private void GrabObject()
    {
        RaycastHit[] pickupable;
        pickupable = Physics.SphereCastAll(transform.position, range, Vector3.up, range, layerPickup.value);

        //if there are non return
        if (pickupable.Length < 1) { return; }

        GetClosestObject(pickupable);

        //make the object follow the pig
        Transform closest = carryObject.transform;
        closest.SetParent(transform);
        closest.GetComponent<Collider>().isTrigger = true;
        closest.GetComponent<Rigidbody>().isKinematic = true;
        closest.position = carryPoint.position;
    }

    private void DropObject()
    {
        if (CheckNearDropPoint())
        {
            carryObject.gameObject.layer = 1 << 0;
            counter.Min = 0;
        }

        Transform closest = carryObject.transform;
        closest.parent = null;
        closest.GetComponent<Collider>().isTrigger = false;
        closest.GetComponent<Rigidbody>().isKinematic = false;
        carryObject = default;
    }

    private bool CheckNearDropPoint()
    {
        RaycastHit[] objects;
        objects = Physics.SphereCastAll(transform.position, range, Vector3.up, range, layerDrop.value);

        foreach (RaycastHit hit in objects)
        {
            if (hit.collider.name == "DropPoint") { return true; }
        }

        return false;
    }

    /// <summary>
    /// Get the closest object
    /// </summary>
    /// <param name="pickupable"></param>
    private void GetClosestObject(RaycastHit[] pickupable)
    {
        float smallestDistance = Mathf.Infinity;
        foreach (RaycastHit pickup in pickupable)
        {
            float distance = Vector3.Distance(transform.position, pickup.transform.position);

            //find closest pickupable
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                carryObject = pickup.transform;
            }
        }
    }
}
