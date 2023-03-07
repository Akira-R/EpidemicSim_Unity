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
    [SerializeField] private Transform _transform;

    [BoxGroup("Camera Movement Modifier")]
    public float movementspeed = 1f;
    public float rotationSpeed = 1f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        _transform = this.GetComponent<Transform>();
    }

    private void OnEnable()
    {
        cameraMovement = playerControls.Camera.PlanarMovement;
        cameraRotation = playerControls.Camera.RotationMovementControl;
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
        Vector3 planarInputValue = cameraMovement.ReadValue<Vector2>().x * CameraHorizontalMovement() +
                                   cameraMovement.ReadValue<Vector2>().y * CameraVerticalMovement();
        planarInputValue = planarInputValue.normalized;
        _transform.position += planarInputValue * movementspeed;

        float rotationInputValue = cameraRotation.ReadValue<float>();
        _transform.rotation = Quaternion.Euler(
            _transform.rotation.eulerAngles.x, 
            _transform.rotation.eulerAngles.y, 
            rotationInputValue * rotationSpeed + _transform.rotation.eulerAngles.z);
    }
}
