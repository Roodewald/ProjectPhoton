using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    [SerializeField] bool grounded;
    Vector3 smothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody playerRigidbody;
    PhotonView PV;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

    }
    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(playerRigidbody);
        }  
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;
        Look();

        Move();

        Jump();
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            playerRigidbody.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedSate(bool _grounded)
    {
        grounded = _grounded;
    }
}
