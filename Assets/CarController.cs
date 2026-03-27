using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Driving")]
    public float driveForce=3000f;
    public float maxSpeed=23f;
    public float turnSpeed=120f;
    public float traction=8f;

    [Header("Gravity")]
    public float cGravity=-20f;
    public float downforce = 10f;

    [Header("GroundCheck")]
    public float gCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private float driveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity=false;
        rb.centerOfMass = new Vector3(0,-0.5f,0);
        rb.collisionDetectionMode=CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        driveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        Debug.Log("grounded"+isGrounded+"driveinput"+driveInput);
        Debug.DrawRay(transform.position,Vector3.down*gCheckDistance,Color.red);
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ApplyDownforce();
        if (isGrounded)
        {
            ApplyDriving();
            ApplyTurning();
            ApplyTraction();
        }
        ClampSpeed();
    }

    void ApplyGravity()
    {
        rb.AddForce(new Vector3(0,cGravity*rb.mass,0),ForceMode.Force);
    }

    void ApplyDownforce()
    {
        rb.AddForce(-transform.up*downforce*rb.linearVelocity.magnitude);
    }

    void ApplyDriving()
    {
        Vector3 driveDirection = transform.forward*driveInput*driveForce;
        rb.AddForce(driveDirection*Time.fixedDeltaTime,ForceMode.Impulse);
    }

    void ApplyTurning()
    {
        if (rb.linearVelocity.magnitude>0.5f)
        {
            float turn = turnInput*turnSpeed*Time.fixedDeltaTime;
            if (driveInput <0) turn=-turn;
            transform.Rotate(0f,turn,0f);
        }
    }

    void ApplyTraction()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        localVelocity.x*=1f - (traction*Time.fixedDeltaTime);
        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }

    void ClampSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
        if (flatVelocity.magnitude>maxSpeed)
        {
            Vector3 clamped=flatVelocity.normalized*maxSpeed;
            rb.linearVelocity =new Vector3(clamped.x, rb.linearVelocity.y,clamped.z);
        }
    }

    bool IsGrounded()
    {
        //return Physics.Raycast(transform.position,Vector3.down,gCheckDistance+0.5f,groundLayer);
        Vector3 rayStart = transform.position+Vector3.up*0.5f;
        Debug.DrawRay(rayStart,Vector3.down*(gCheckDistance+0.5f),Color.red);
        return Physics.Raycast(rayStart,Vector3.down,gCheckDistance+0.5f);
    }
}
