using UnityEngine;

/// <summary>
/// Pig movement behaviour
/// </summary>
public class PigMovement : Actions
{
    //Public variables through blackboard
    private float speed = BlackBoard.PigSpeed;
    private int layer = BlackBoard.PigLayer;

    [SerializeField]
    private MeshFilter model;

    private Transform player;
    private Rigidbody pigRigidbody;
    private Vector3 direction;
    private Vector3 destination;
    private float pigLength;

    private bool moving;
    private bool arrived;

    private void Awake()
    {
        commands["Move away"] = true;
        commands["Come back"] = false;
        idle = FindObjectOfType<PigIdle>();

        player = FindObjectOfType<PlayerMovement>().transform;
        pigRigidbody = GetComponent<Rigidbody>();
        pigLength = model.mesh.bounds.size.x;
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, destination, Color.red);
        if (!moving) { Stop(); return; }

        //check if the pig has arrived at destination
        arrived = CheckArrived();
        moving = !arrived;

        //move the pig
        Move();
    }

    /// <summary>
    /// Move the pig closer to its destination
    /// </summary>
    private void Move()
    {
        pigRigidbody.MovePosition(transform.position + direction.normalized * speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Check based on distance if arrived at destination
    /// </summary>
    /// <returns>true if arrived</returns>
    private bool CheckArrived()
    {
        float distance = Vector3.Distance(transform.position, destination);

        if (distance < pigLength) { return true; }

        return false;
    }

    /// <summary>
    /// Make the pig start moving. If away is true he moves away.
    /// </summary>
    /// <param name="away"></param>
    public override void Run(dynamic param)
    {
        // base.Run(param);

        moving = true;

        if (param)
        {
            direction = transform.position - player.position;
            Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);
            RotateTowards(lookDirection);
            destination = GetDestination();
        }
        else
        {
            direction = player.position - transform.position;
            Vector3 lookDirection = new Vector3(direction.x, 0f, direction.z);
            RotateTowards(lookDirection);
            destination = player.position;
        }
    }

    /// <summary>
    /// Make the pig rotate towards where he is going
    /// </summary>
    /// <param name="direction">The direction you want the pig to face</param>
    private void RotateTowards(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>
    /// Makes the pig stop moving
    /// </summary>
    public override void Stop()
    {
        moving = false;
    }

    /// <summary>
    /// Returns the farthest point the pig can move
    /// </summary>
    /// <returns>Destination</returns>
    private Vector3 GetDestination()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, 1 << layer)) { return hit.point; }

        return transform.position;
    }
}
