using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [Header("Movement Speeds")]
    [Tooltip("Speed at which player walks")]
    public float walkingSpeed = 1.5f;

    [Tooltip("Speed at which player runs")]
    public float runningSpeed = 5f;

    [Tooltip("Speed at which player sprints")]
    public float sprintingSpeed = 7f;

    [Tooltip("Speed at which player rotates")]
    public float rotationSpeed = 15f;

    [HideInInspector] public bool isSprinting;

    private InputManager _inputManager;

    private Vector3 _moveDirection;
    private Transform _cameraObjectTransform;

    private Rigidbody _playeRigidbody;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playeRigidbody = GetComponent<Rigidbody>();
        _cameraObjectTransform = Camera.main.transform;
    }

    public void HandleAllMovements()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        _moveDirection = _cameraObjectTransform.forward * _inputManager.verticalInput;
        _moveDirection += _cameraObjectTransform.right * _inputManager.horizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        float movementSpeed;
        if (isSprinting)
        {
            movementSpeed = sprintingSpeed;
        }
        else
        {
            movementSpeed = _inputManager.moveAmount > 0.5 ? runningSpeed : walkingSpeed;
        }

        var movementVelocity = _moveDirection * movementSpeed;

        _playeRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        var targetDirection = _cameraObjectTransform.forward * _inputManager.verticalInput;
        targetDirection += _cameraObjectTransform.right * _inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        var targetRotation = Quaternion.LookRotation(targetDirection);
        var playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}