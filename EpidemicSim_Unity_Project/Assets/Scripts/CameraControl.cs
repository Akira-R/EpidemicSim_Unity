using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction cameraMovement;
    private InputAction cameraRotation;
    private InputAction cameraZoom;
    private InputAction cameraSpeedMod;
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _childTransform;

    [BoxGroup("Camera Movement Modifier")]
    [Range(0f, 1f)] public float movementspeed = 0.1f;
    [BoxGroup("Camera Movement Modifier")]
    [Range(0f, 1f)] public float rotationSpeed = 0.1f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        _transform = this.GetComponent<Transform>();
        _childTransform = _transform.GetChild(0);
    }

    private void OnEnable()
    {
        cameraMovement = playerControls.Camera.PlanarMovement;
        cameraRotation = playerControls.Camera.RotationControl;
        cameraZoom = playerControls.Camera.ZoomControl;
        cameraSpeedMod = playerControls.Camera.CameraSpeedMod;
        playerControls.Camera.Enable();
    }
    private void OnDisable()
    {
        playerControls.Camera.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    private Vector3 CameraVerticalMovement() 
    {
        Vector3 forward = _transform.up;
        forward.y = 0f;
        return forward;
    }
    private Vector3 CameraHorizontalMovement()
    {
        Vector3 right = _transform.right;
        right.y = 0f;
        return right;
    }

    private void UpdateInput()
    {
        float cameraSpeedModValue = cameraSpeedMod.ReadValue<float>();
        movementspeed += cameraSpeedModValue;
        rotationSpeed += cameraSpeedModValue;

        if (movementspeed < 0f)
            movementspeed = 0f;
        else if (movementspeed > 1f)
            movementspeed = 1f;

        if (rotationSpeed < 0f)
            rotationSpeed = 0f;
        else if (rotationSpeed > 1f)
            rotationSpeed = 1f;

        Vector3 planarInputValue = cameraMovement.ReadValue<Vector2>().x * CameraHorizontalMovement() +
                                   cameraMovement.ReadValue<Vector2>().y * CameraVerticalMovement();
        planarInputValue = planarInputValue.normalized;
        _transform.position += planarInputValue * movementspeed;

        float rotationInputValue = cameraRotation.ReadValue<float>();
        _transform.rotation = Quaternion.Euler(
            _transform.rotation.eulerAngles.x, 
            _transform.rotation.eulerAngles.y, 
            rotationInputValue * rotationSpeed + _transform.rotation.eulerAngles.z);

        float zoomInputValue = cameraZoom.ReadValue<float>();
        GameObject cameraGameObject = _transform.gameObject;
        cameraGameObject.GetComponent<Camera>().fieldOfView += zoomInputValue; 
        _childTransform.GetComponent<Camera>().fieldOfView += zoomInputValue;
    }
}
