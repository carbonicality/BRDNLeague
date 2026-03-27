using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [Header("Gravity")]
    public float cGravity = -15f;
    public float maxFallspeed = 50f;

    [Header("Bounce")]
    public float groundBounceDamp = 0.6f;
    public float wallBounceDamp = 0.65f;

    [Header("Rolling")]
    public float rollFriction = 0.98f;
    public float spinFriction = 0.97f;

    [Header("Feel")]
    public float angularVeloMax = 6f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.maxAngularVelocity=angularVeloMax;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ApplyRFriction();
        ClampFS();
    }

    void ApplyGravity()
    {
        rb.AddForce(new Vector3(0, cGravity*rb.mass,0),ForceMode.Acceleration);
    }

    void ApplyRFriction()
    {
        if (IsGrounded())
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x*rollFriction,
                rb.linearVelocity.y,
                rb.linearVelocity.z*rollFriction
            );
            rb.angularVelocity *=spinFriction;
        }
    }

    void ClampFS()
    {
        if (rb.linearVelocity.y <-maxFallspeed)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                -maxFallspeed,
                rb.linearVelocity.z
            );
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, GetComponent<SphereCollider>().radius*1.1f, LayerMask.GetMask("Default")); // cs pmo bro just let me use '' instead of ""
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 hitDirection = collision.relativeVelocity;
        rb.AddTorque(
            new Vector3(hitDirection.z,0,-hitDirection.x)*0.3f,
            ForceMode.Impulse
        );
    }
}
