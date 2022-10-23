using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public GameObject PhotoMask;
    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 inputVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputVec = Vector3.ClampMagnitude(inputVec, 1.0f);
        var moveVec = transform.right * inputVec.x + transform.up * inputVec.z;
        transform.position += moveVec * Time.deltaTime * MoveSpeed;
        var mp = Input.mousePosition - new Vector3( Screen.height / 2, Screen.width / 2, 0);
        var angle =Mathf.Atan2(mp.y, mp.x);
        PhotoMask.transform.rotation = Quaternion.Euler(0, 0, 45 +180+ (angle * Mathf.Rad2Deg));
    }
}
