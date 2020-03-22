using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100f)]
    private float speed = 1;

    [SerializeField, Range(0, 90f)]
    private float minAngle = 60f;

    private Vector2 velocity;

    private Rigidbody2D body;

    private Vector2 direction;

    private Vector2 prevDirection;
    private Vector2 realDirection;

    private Collision2D contactPoint;

    private void OnDrawGizmos()
    {
        if (contactPoint == null)
        {
            return;
        }

        ContactPoint2D point = contactPoint.GetContact(0);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(point.point, 0.1f);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(point.point, -1 * prevDirection * 10f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(point.point, point.normal * 10f);
        Gizmos.color = Color.red;

        Gizmos.DrawRay(point.point, direction * 10f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(point.point, realDirection * 10f);

    }

    private void Awake()
    {

        direction = transform.up;

        //body = GetComponent<Rigidbody2D>();

        //body.velocity = transform.up * speed;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        prevDirection = direction;
        ContactPoint2D contact = other.GetContact(0);

        Vector2 normal = contact.normal;

        if (contact.point.magnitude < (contact.point + contact.normal).magnitude)
        {
            Debug.Log("Entered here");

            normal *= -1;
        }

        Vector2 rotatedDirection = -1 * direction;

        float angleBetweenVectors = Vector2.SignedAngle(rotatedDirection, normal);

        realDirection = Vector2.Reflect(direction, normal);

        if (Mathf.Abs(angleBetweenVectors) < 90 - minAngle)
        {
            direction = Vector2.Reflect(direction, normal);
        }
        else
        {
            int vectorsDirection = angleBetweenVectors < 0 ? -1 : 1;

            direction = Quaternion.AngleAxis(vectorsDirection * (90 - minAngle), Vector3.forward) * normal;
        }

        contactPoint = other;
    }
}
