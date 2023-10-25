using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook2 : MonoBehaviour
{
    private Camera playerCamera;
    private CameraFov cameraFov;
    private CharacterController characterController;
    private State state;
    private Vector3 hookshotPosition;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private float hookshotSize;
    public AudioSource grapsound;
    [SerializeField] private float _maxHookDist;

    private const float NORMAL_FOV = 60f;
    private const float HOOKSHOT_FOV = 100f;

    [SerializeField] private Transform debugHitpointTransform;
    [SerializeField] private Transform hookshotTransform;
    private void Awake()
    {
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        cameraFov = playerCamera.GetComponent<CameraFov>();
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFlyingPlayer
    }

    private void HandleCharacterMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 20f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded)
        {
            characterVelocityY = 0f;
            // Jump
            if (TestInputJump())
            {
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
        if (characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < .0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }
    }

    private void Update()
    {
        switch (state) {
            default:
            case State.Normal:
                HandleHookshotStart();
                break;
            case State.HookshotThrown:
                HandleHookshotThrown();
                break;
            case State.HookshotFlyingPlayer:
                HandleHookshotMovement();
                break;
    }
 
    }

    private void HandleHookshotStart()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,out RaycastHit raycastHit, _maxHookDist))
            {
                debugHitpointTransform.position = raycastHit.point;
                state = State.HookshotThrown;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                hookshotPosition = raycastHit.point;
            }
        }
    }

    private void HandleHookshotThrown()
    {
        hookshotTransform.LookAt(hookshotPosition);
        float hookshotThrowSpeed = 70f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize>= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFlyingPlayer;
            cameraFov.SetCameraFov(HOOKSHOT_FOV);
            grapsound.Play();
        }
    }

    private void HandleHookshotMovement()
    {
        float hookshotSpeedMin = 30f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        hookshotTransform.LookAt(hookshotPosition);
        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;
        characterController.Move(hookshotDir * hookshotSpeed * Time.deltaTime);

        float reachedHookshotPositionDistance = 2f;
        if(Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)
        {
            state = State.Normal;
            ResetGravityEffect();
            hookshotTransform.gameObject.SetActive(false);
            cameraFov.SetCameraFov(NORMAL_FOV);
        }

        if (TestInputDownHookshot()) //Cancel with E
        {
            state = State.Normal;
            ResetGravityEffect();
            hookshotTransform.gameObject.SetActive(false);
            cameraFov.SetCameraFov(NORMAL_FOV);
        }

        if (TestInputJump())
        {
            // Cancelled with Jump
            float momentumExtraSpeed = 7f;
            characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
            float jumpSpeed = 20f;
            characterVelocityMomentum += Vector3.up * jumpSpeed; 
            state = State.Normal;
            ResetGravityEffect();
            hookshotTransform.gameObject.SetActive(false);
            cameraFov.SetCameraFov(NORMAL_FOV);
        }
    }


    private void ResetGravityEffect() //reset player gravity
    {
        characterVelocityY = 0f;
    }

    private bool TestInputDownHookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private bool TestInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

}
