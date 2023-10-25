﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour {

    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;

    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private Camera playerCamera;
    private CameraFov cameraFov;
    private ParticleSystem speedLinesParticleSystem;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    private enum State {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer,
    }

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        cameraFov = playerCamera.GetComponent<CameraFov>();
        speedLinesParticleSystem = transform.Find("Camera").Find("SpeedLinesParticleSystem").GetComponent<ParticleSystem>();
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private void Update() {
        switch (state) {
        default:
        case State.Normal:
            HandleCharacterLook();
            HandleCharacterMovement();
            HandleHookshotStart();
            break;
        case State.HookshotThrown:
            HandleHookshotThrow();
            HandleCharacterLook();
            HandleCharacterMovement();
            break;
        case State.HookshotFlyingPlayer:
            HandleCharacterLook();
            HandleHookshotMovement();
            break;
        }
    }

    private void HandleCharacterLook() {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);
        cameraVerticalAngle -= lookY * mouseSensitivity;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    private void HandleCharacterMovement() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 20f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded) {
            characterVelocityY = 0f;
            // Jump
            if (TestInputJump()) {
                float jumpSpeed = 30f;
                characterVelocityY = jumpSpeed;
            }
        }

        // Apply gravity to the velocity
        float gravityDownForce = -60f;
        characterVelocityY += gravityDownForce * Time.deltaTime;


        // Apply Y velocity to move vector
        characterVelocity.y = characterVelocityY;

        // Apply momentum
        characterVelocity += characterVelocityMomentum;

        // Move Character Controller
        characterController.Move(characterVelocity * Time.deltaTime);

        // Dampen momentum
        if (characterVelocityMomentum.magnitude > 0f) {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < .0f) {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }

    private void ResetGravityEffect() {
        characterVelocityY = 0f;
    }

    private void HandleHookshotStart() {
        if (TestInputDownHookshot()) {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit)) {
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;
            }
        }
    }

    private void HandleHookshotThrow() {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 500f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition)) {
            state = State.HookshotFlyingPlayer;
            cameraFov.SetCameraFov(HOOKSHOT_FOV);
            speedLinesParticleSystem.Play();
        }
    }

    private void HandleHookshotMovement() {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 5f;

        // Move Character Controller
        characterController.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance) {
            // Reached Hookshot Position
            StopHookshot();
        }

        if (TestInputDownHookshot()) {
            // Cancel Hookshot
            StopHookshot();
        }

        if (TestInputJump()) {
            // Cancelled with Jump
            float momentumExtraSpeed = 7f;
            characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
            float jumpSpeed = 40f;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            StopHookshot();
        }
    }

    private void StopHookshot() {
        state = State.Normal;
        ResetGravityEffect();
        hookshotTransform.gameObject.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
        speedLinesParticleSystem.Stop();
    }

    private bool TestInputDownHookshot() {
        return Input.GetKeyDown(KeyCode.E);
    }

    private bool TestInputJump() {
        return Input.GetKeyDown(KeyCode.Space);
    }


}
