using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Delta = 0.1f;
    public Animator anim;
    public bool DebugLine;
    public Transform CameraTransform;
    //public VirtualGamepad vg;
    [SerializeField] private float speed;
    [SerializeField] private float JumpForce;
    private float prevAngle;
    private Vector2 forwardVec;
    private float angle;
    private Rigidbody rb;
    private float deltaAngle;
    private Vector2 lastInput;
    private Quaternion PrevRotation;
    private Vector3 CmaeraVec;

    private void Start()
    {
        prevAngle = 0;
        rb = GetComponent<Rigidbody>();
        PrevRotation = transform.rotation;
        CmaeraVec = CameraTransform.position - transform.position;
    }

    private void Update()
    {
        var inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //anim.WalkSpeed = 0;
        anim.SetFloat("Speed", 0);

        if (Input.GetKeyDown(KeyCode.Space) && CheckGrounded())
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }

        if (inputVec != Vector2.zero)
        {
            //inputVec = inputVec.y * transform.forward + inputVec.x * transform.right;

            //forwardVec = Vector2.Lerp(forwardVec, inputVec, 0.1f);
            var camF = new Vector2(CameraTransform.forward.x, CameraTransform.forward.z);
            var camR = new Vector2(CameraTransform.right.x, CameraTransform.right.z);
            forwardVec = camF * inputVec.y + camR * inputVec.x;
            //forwardVec = inputVec.y * transform.forward + inputVec.x * transform.right;
            //angle = Mathf.Sign(forwardVec.x) * Vector2.Angle(Vector2.up, forwardVec);//angle = forwardVec.x > 0 ? angle : - angle;
            angle = Mathf.Sign(forwardVec.x) * Vector2.Angle(Vector2.up, forwardVec);//angle = forwardVec.x > 0 ? angle : - angle;

            var delta = angle - prevAngle;
            deltaAngle = Mathf.Abs(delta) > 180 ? delta + (- Mathf.Sign(delta) * 360) : delta;
            deltaAngle = Mathf.Clamp(deltaAngle, -10, 10);

            transform.rotation = Quaternion.Euler(0, angle, - deltaAngle);

            var forwardVec3 = new Vector3(forwardVec.x, 0, forwardVec.y).normalized * speed * Time.deltaTime;

            anim.SetFloat("Speed", forwardVec3.magnitude);

            transform.position += forwardVec3;

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
            //forwardVec = Vector2.Lerp(forwardVec, lastInput, 0.1f);
            //angle = forwardVec.x > 0 ? Vector2.Angle(Vector2.up, forwardVec) : -Vector2.Angle(Vector2.up, forwardVec);
            //deltaAngle = Mathf.Lerp(deltaAngle, 0, 0.1f);

            //var qangle = Quaternion.Euler(0, angle, -deltaAngle);
            ////todo
            ////anim.StepLength = Mathf.Clamp(forwardVec.magnitude, MinStepLength, MaxStepLength);
            //transform.rotation = qangle;
        }

        //transform.rotation = Quaternion.Euler(10, 0, 0) * transform.rotation;
        prevAngle = angle;
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.NameToLayer("IgnoreProjector")); 
    }
}
