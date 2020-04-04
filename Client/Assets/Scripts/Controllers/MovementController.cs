using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CharacterController Controller;
    public float Speed = 12f;
    public float Gravity = -9.81f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    public float JumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    public GameObject PlayerMesh;

    private void Start() {
        PlayerMesh.SetActive(false);
    }

    private void Update() {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = transform.right * x + transform.forward * z;
        Controller.Move(move * Speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);

        velocity.y += Gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);
    }

}
