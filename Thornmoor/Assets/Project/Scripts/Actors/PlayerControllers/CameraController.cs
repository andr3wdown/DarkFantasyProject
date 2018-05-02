using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        staticFollow,
        smoothedFollow
    }
    [Header("Mode")]
    public CameraMode camMode = CameraMode.smoothedFollow;
    [Space(5)]
    [Header("Properties")]
    public Transform cameraTarget;
    float yaw = 45;
    float pitch = 60;
    [Range(5f, 50f)]
    public float cameraDistance = 10f;
    
    Vector3 currentRotation;
    Vector3 rotationSmoothVelocity;
    [Space(5)]
    [Header("Smoothing Properties")]
    [Range(0f, 1f)]
    public float rotSmoothTime = 0.12f;
    [Range(1f, 20f)]
    public float cameraSpeed;
    [Range(0.0001f, 3f)]
    public float smoothingAmount;
    [Space(5)]
    [Header("Controls")]
    public KeyCode turnRightButton = KeyCode.Joystick1Button5;
    public KeyCode turnLeftButton = KeyCode.Joystick1Button4;
    public KeyCode altRightButton = KeyCode.E;
    public KeyCode altLeftButton = KeyCode.Q;

    private void Update()
    {
        if (Input.GetKeyDown(altLeftButton) || Input.GetKeyDown(turnLeftButton))
        {
            yaw += 90;
        }
        if (Input.GetKeyDown(altRightButton) || Input.GetKeyDown(turnRightButton))
        {
            yaw -= 90;
        }
    }

    private void FixedUpdate()
    {

        if(cameraTarget != null)
        {
            Vector3 targetRotation = new Vector3(pitch, yaw);
            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotSmoothTime);
            transform.eulerAngles = currentRotation;
            Vector3 desiredPos = cameraTarget.position - transform.forward * cameraDistance;
            if (camMode == CameraMode.staticFollow)
            {
                transform.position = desiredPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, desiredPos, (cameraSpeed / smoothingAmount) * Time.fixedDeltaTime);
            }
        }
               
    }
}
