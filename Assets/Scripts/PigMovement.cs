using UnityEngine;

public class PigMovement : MonoBehaviour
{
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

    private void Move()
    {
        pigRigidbody.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
    }

    private bool CheckArrived()
    {
        float distance = Vector3.Distance(transform.position, destination);

        if (distance < pigLength) { return true; }

        return false;
    }

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

    private Vector3 GetDestination()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, layer)) { return hit.point; }

        return transform.position;
    }

    private void Stop()
    {
        moving = false;
    }
}
