using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System.Linq;
using Assets.Scripts;

public class MyPlayer : MonoBehaviour
{
    public GameObject ellisePrefab;
    public Transform respawnPoint;
    


    public ExampleCharacterCamera OrbitCamera;
    public Transform CameraFollowPoint;
    public Transform CameraFollowPointCrouching;
    public MyCharacterController Character;

    private const string MouseXInput = "Mouse X";
    private const string MouseYInput = "Mouse Y";
    private const string MouseScrollInput = "Mouse ScrollWheel";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        // Tell camera to follow transform
        OrbitCamera.SetFollowTransform(CameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        OrbitCamera.IgnoredColliders.Clear();
        OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Cursor.lockState = CursorLockMode.Locked;
        }
        if (Character.CurrentCharacterState == CharacterState.Crouching)
        {
            if (OrbitCamera.FollowTransform != CameraFollowPointCrouching)
            {
                OrbitCamera.SetFollowTransform(CameraFollowPointCrouching);
            }
        }
        else
        {
            if (OrbitCamera.FollowTransform != CameraFollowPoint)
            {
                OrbitCamera.SetFollowTransform(CameraFollowPoint);
            }
        }

        HandleCharacterInput();
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayerRespawn();
        }
    }

    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
        characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
        characterInputs.CameraRotation = OrbitCamera.Transform.rotation;
        characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);

        //Chargins State Implementation Examples
        characterInputs.ChargingDown = Input.GetKeyDown(KeyCode.LeftShift);

        characterInputs.StealthDown = Input.GetKeyDown(KeyCode.F);

        characterInputs.ClimbLadder = Input.GetKeyUp(KeyCode.E);


        characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
        characterInputs.CrouchUp = Input.GetKeyDown(KeyCode.V);


        // Apply inputs to character
        Character.SetInputs(ref characterInputs);


        //Example on how to add force to the character => use with explosions or push effects
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Character.Motor.ForceUnground(0.1f);
        //    Character.AddVelocity(Vector3.one * 10f);
        //}
    }

    private void LateUpdate()
    {
        HandleCameraInput();
    }

    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
        float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Input for zooming the camera (disabled in WebGL because it can cause problems)
        float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

        // Apply inputs to the camera
        OrbitCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

        // Handle toggling zoom level
        if (Input.GetMouseButtonDown(1))
        {
            OrbitCamera.TargetDistance = (OrbitCamera.TargetDistance == 0f) ? OrbitCamera.DefaultDistance : 0f;
        }
    }

    private void PlayerRespawn()
    {
        if (GameManager.instance.userManager.GetClones() >0)
        {
            GameManager.instance.respawnManager.UpdatePods();
            GameManager.instance.userManager.ReduceClones(1);
            GameObject ellise = Instantiate(ellisePrefab, respawnPoint.transform.position, Quaternion.identity);
            CameraFollowPoint = ellise.transform.GetChild(5);
            CameraFollowPointCrouching = ellise.transform.GetChild(6);
            OrbitCamera.SetFollowTransform(CameraFollowPoint);
            Character.CharacterAnimator.SetTrigger("IsDead");
            Character.armRig.weight = 0;
            Character.armRig.weight = 0;
            Character.gameObject.layer = 8;

            Character = ellise.GetComponent<MyCharacterController>();
            

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }
    }

}

