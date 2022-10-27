using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementStates
{
    Normal,
    Stance
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float Delta = 0.1f;
    public Animator anim;
    public bool DebugLine;
    public Transform CameraTransform;
    //public VirtualGamepad vg;
    public bool SetMoving(bool move) => moving = move;

    [SerializeField] private float speed;
    [SerializeField] private float stanceSpeed;
    [SerializeField] private float JumpForce;
    private float prevAngle;
    private Vector2 forwardVec;
    private Rigidbody rb;
    private float deltaAngle;
    private Vector2 lastInput;
    private Quaternion PrevRotation;
    public MovementStates state;
    private Transform target;
    protected bool moving;
    protected float angle;
    protected Vector3 forwardVec3;

    private void Start()
    {
        prevAngle = 0;
        rb = GetComponent<Rigidbody>();
        PrevRotation = transform.rotation;
    }

    private void Update()
    {
        anim.SetFloat("Speed", 0);
        
        Moving();

        anim.SetFloat("State", (int)state);
        anim.SetFloat("Speed", forwardVec3.magnitude);
    }

    public void SetState(MovementStates state)
    {
        this.state = state;
    }

    public virtual void Moving()
    {
        var inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            state = (MovementStates)((int)(state + 1) % 2);
        }

        //if (Input.GetKeyDown(KeyCode.Space) && CheckGrounded())
        //{
        //    rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        //    anim.SetTrigger("Jump");
        //}

        switch (state)
        {
            case MovementStates.Normal: NormalMovement(inputVec); break;
            case MovementStates.Stance: StanceMovement(inputVec); break;
            default: break;
        }

        //transform.rotation = Quaternion.Euler(10, 0, 0) * transform.rotation;
        if (moving)
        {
            transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.position += forwardVec3 * Time.deltaTime;
        }

        prevAngle = angle;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void StanceMovement(Vector2 inputVec)
    {
        if (target == null)
        {
            state = MovementStates.Normal;
        }
        else
        {
            var forward = (target.position - transform.position).normalized;
            forward.y = forward.z;
            anim.SetFloat("Right", inputVec.x);
            angle = Mathf.Sign(forward.x) * Vector2.Angle(Vector2.up, forward);
            forwardVec3 = (transform.right * inputVec.x + transform.forward * inputVec.y).normalized * stanceSpeed;
        }
    }

    private void NormalMovement(Vector2 inputVec)
    {
        if (inputVec != Vector2.zero)
        {
            //inputVec = inputVec.y * transform.forward + inputVec.x * transform.right;

            //forwardVec = Vector2.Lerp(forwardVec, inputVec, 0.1f);
            Vector2 camF = new();
            Vector2 camR = new();
            if (CameraTransform)
            {
                camF = new Vector2(CameraTransform.forward.x, CameraTransform.forward.z);
                camR = new Vector2(CameraTransform.right.x, CameraTransform.right.z);
            }
            forwardVec = camF * inputVec.y + camR * inputVec.x;
            //forwardVec = inputVec.y * transform.forward + inputVec.x * transform.right;
            //angle = Mathf.Sign(forwardVec.x) * Vector2.Angle(Vector2.up, forwardVec);//angle = forwardVec.x > 0 ? angle : - angle;
            angle = Mathf.Sign(forwardVec.x) * Vector2.Angle(Vector2.up, forwardVec);//angle = forwardVec.x > 0 ? angle : - angle;

            var delta = angle - prevAngle;
            deltaAngle = Mathf.Abs(delta) > 180 ? delta + (-Mathf.Sign(delta) * 360) : delta;
            deltaAngle = Mathf.Clamp(deltaAngle, -10, 10);

            forwardVec3 = new Vector3(forwardVec.x, 0, forwardVec.y).normalized * speed;

            //print((transform.rotation * Quaternion.Inverse(PrevRotation)).eulerAngles.y);
            ////lastInput = inputVec;
            //PrevRotation = transform.rotation;
            if (DebugLine)
            {
                Debug.DrawLine(transform.position, transform.position + forwardVec3);

                Debug.DrawLine(transform.position, (transform.position + forwardVec3), Color.red, 5);
                Debug.DrawLine(transform.position, (transform.position + transform.up), Color.green, 5);
            }
        }
        else
        {
            forwardVec3 = Vector3.zero;
            //forwardVec = Vector2.Lerp(forwardVec, lastInput, 0.1f);
            //angle = forwardVec.x > 0 ? Vector2.Angle(Vector2.up, forwardVec) : -Vector2.Angle(Vector2.up, forwardVec);
            //deltaAngle = Mathf.Lerp(deltaAngle, 0, 0.1f);

            //var qangle = Quaternion.Euler(0, angle, -deltaAngle);
            ////todo
            ////anim.StepLength = Mathf.Clamp(forwardVec.magnitude, MinStepLength, MaxStepLength);
            //transform.rotation = qangle;
        }
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.NameToLayer("IgnoreProjector")); 
    }
}
