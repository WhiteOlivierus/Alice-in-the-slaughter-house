using UnityEngine;

public class PigMovement : MonoBehaviour
{
    //Public variables through blackboard
    private float speed = BlackBoard.PigSpeed;

    [SerializeField]
    private LayerMask layer;

    private Transform player;
    private Rigidbody pigRigidbody;
    private Vector3 direction;
    private Vector3 destination;
    private float pigLength;

    private bool moving;
    private bool arrived;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        pigRigidbody = GetComponent<Rigidbody>();
        pigLength = GetComponent<MeshFilter>().mesh.bounds.size.x / 2;
    }

    private void FixedUpdate()
    {
        //check if the pig has arrived at destination
        arrived = CheckArrived();
        moving = !arrived;

        if (!moving) { return; }

        //move the pig
        Move();
    }

    /// <summary>
    /// Move the pig closer to its destination
    /// </summary>
    private void Move()
    {
        pigRigidbody.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
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
    public void Begin(bool away)
    {
        moving = true;

        if (away)
        {
            direction = transform.position - player.position;
            destination = GetDestination();
        }
        else
        {
            direction = player.position - transform.position;
            destination = player.position;
        }
    }

    /// <summary>
    /// Makes the pig stop moving
    /// </summary>
    public void Stop()
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
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, layer)) { return hit.point; }

        return transform.position;
    }
}
