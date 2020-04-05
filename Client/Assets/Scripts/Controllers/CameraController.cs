using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerBody;

    private float xRotation = 0f;
    private float mouseSensitivity = 100f;

    private float mouseX;
    private float mouseY;
    
    private void Start() {
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.Confined : CursorLockMode.Locked ;
        }
        if (Cursor.lockState == CursorLockMode.Locked) {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            PlayerBody.Rotate(Vector3.up * mouseX);
        }

    }
}
