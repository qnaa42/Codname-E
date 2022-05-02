using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.ClimbingLadders;
using UnityEngine.Animations.Rigging;

public enum CharacterState
{
    Default,
    Tool1,
    Tool2,
    Charging,
    Stealth,
    Crouching,
    Climbing,
}

public enum ClimbingState
{
    Anchoring,
    Climbing,
    DeAnchoring
}

public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpDown;
    public bool StealthDown;

    public bool ClimbLadder;

    //Crouching 
    public bool CrouchDown;
    public bool CrouchUp;

    //Chargins State Implementation Examples
    public bool ChargingDown;

    //Default state - no tool equiped
    public bool utilityItem0;
    //IT multitool - interacting with hacking puzzles and cameras
    public bool utilityItem1;
    //Fixing mutlitool - interacting with fixing mechanic // enemies (dead space)
    public bool utilityItem2;
}
public class MyCharacterController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;

    [Header("Stable Movement")]
    public float MaxStableMoveSpeed = 10f;
    public float StableMovementSharpness = 15;
    public float OrientationSharpness = 10;

    //Chargins State Implementation Examples
    public float MaxStableDistanceFromLedge = 5f;
    [Range(0f, 180f)]
    public float MaxStableDenivelationAngle = 180f;

    [Header("Air Movement")]
    public float MaxAirMoveSpeed = 10f;
    public float AirAccelerationSpeed = 5f;
    public float Drag = 0.1f;

    [Header("Animation Parameters")]
    public Animator CharacterAnimator;
    public float ForwardAxisSharpness = 10;
    public float TurnAxisSharpness = 5;

    [Header("Jumping")]
    public bool AllowJumpingWhenSliding = false;
    public bool AllowDoubleJump = false;
    public bool AllowWallJump = false;
    public float JumpSpeed = 10f;
    public float JumpPreGroundingGraceTime = 0f;
    public float JumpPostGroundingGraceTime = 0f;

    [Header("Stealth Ability Misc")]
    public float StealthTime = 3f;
    public float StealthCooldown = 45f;
    private float _timeSinceStealth = 0f;
    private bool canStealth = true;
    private bool _isStealth;
    public float _stealthCooldownTimer;

    //Chargins State Implementation Examples
    [Header("Charging")]
    public float ChargeSpeed = 15f;
    public float MaxChargeTime = 1.5f;
    public float StoppedTime = 1f;

    [Header("Ladder Climbing")]
    public float ClimbingSpeed = 4f;
    public float AnchoringDuration = 0.25f;
    public LayerMask InteractionLayer;

    [Header("Misc")]
    public bool _isDead = false;
    public bool RotationObstruction;
    public Vector3 Gravity = new Vector3(0, -30f, 0);
    public Transform MeshRoot;
    public List<Collider> IgnoredColliders = new List<Collider>();

    private Collider[] _probedColliders = new Collider[8];

    [Header("Rigging Misc")]
    public Rig armRig;
    public Rig armRigCrouching;

    [Header("Tools")]
    public bool hasFixingTool = false;
    public bool hasHackingTool = false;
    [Header("Raust")]
    [Header("Audio Clips")]
    
    public AudioSource wakeUpAudio;
    public AudioSource defendClonesAudio;
    public AudioSource goodJobAudio;
    public AudioSource breakOfcontactAudio;
    public AudioSource yourShipIsBroken;
    
    
    [Header("Ermet")]
    public AudioSource fixingToolAudio;

    [Header("Ai")] 
    public AudioSource afterBeatingCloneBay;
    public AudioSource afterTaskDone;
    public AudioSource[] AiIdle;
    public AudioSource droneBayMonologue;
    public AudioSource firstCamera;
    
    
    
    
    

    
    //Animator Misc
    private float _forwardAxis;
    private float _rightAxis;
    private float _targetForwardAxis;
    private float _targetRightAxis;
    private Vector3 _rootMotionPositionDelta;
    private Quaternion _rootMotionRotationDelta;

    [Header("AimMisc")]
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    //[SerializeField] private Transform debugTransform;
    [SerializeField] private Transform aimTarget;

    public CharacterState CurrentCharacterState { get; private set; }

    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;
    private bool _jumpRequested = false;
    private bool _jumpConsumed = false;
    private bool _jumpedThisFrame = false;
    private float _timeSinceJumpRequested = Mathf.Infinity;
    private float _timeSinceLastAbleToJump = 0f;
    private bool _doubleJumpConsumed = false;
    private bool _canWallJump = false;
    private Vector3 _wallJumpNormal;
    private Vector3 _internalVelocityAdd = Vector3.zero;

    private Vector3 _currentChargeVelocity;
    private bool _isStopped;
    private bool _mustStopVelocity = false;
    private float _timeSinceStartedCharge = 0;
    private float _timeSinceStopped = 0;

    private bool _shouldBeCrouching = false;
    private bool _isCrouching = false;
    private bool _isGoingToCrouch = false;

    // Ladder vars
    private float _ladderUpDownInput;
    private MyLadder _activeLadder { get; set; }
    private ClimbingState _internalClimbingState;
    private ClimbingState _climbingState
    {
        get
        {
            return _internalClimbingState;
        }
        set
        {
            _internalClimbingState = value;
            _anchoringTimer = 0f;
            _anchoringStartPosition = Motor.TransientPosition;
            _anchoringStartRotation = Motor.TransientRotation;
        }
    }
    private Vector3 _ladderTargetPosition;
    private Quaternion _ladderTargetRotation;
    private float _onLadderSegmentState = 0;
    private float _anchoringTimer = 0f;
    private Vector3 _anchoringStartPosition = Vector3.zero;
    private Quaternion _anchoringStartRotation = Quaternion.identity;
    private Quaternion _rotationBeforeClimbing = Quaternion.identity;



    // Start is called before the first frame update
    void Start()
    {
        _rootMotionPositionDelta = Vector3.zero;
        _rootMotionRotationDelta = Quaternion.identity;

        Motor.CharacterController = this;

        // Handle initial state
        TransitionToState(CharacterState.Default);
    }

    void Update()
    {
        var tranform = this.GetComponent<Transform>();
        tranform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        // Handle animation
        _forwardAxis = Mathf.Lerp(_forwardAxis, _targetForwardAxis, 1f - Mathf.Exp(-ForwardAxisSharpness * Time.deltaTime));
        _rightAxis = Mathf.Lerp(_rightAxis, _targetRightAxis, 1f - Mathf.Exp(-ForwardAxisSharpness * Time.deltaTime));
        CharacterAnimator.SetFloat("ForwardSpeed", _forwardAxis);
        CharacterAnimator.SetFloat("VerticalSpeed", _rightAxis);
        CharacterAnimator.SetBool("Grounded", Motor.GroundingStatus.IsStableOnGround);
        CharacterAnimator.SetFloat("AirborneVerticalSpeed", _timeSinceJumpRequested);
        CharacterAnimator.SetBool("JumpRequested", _jumpedThisFrame);
        //CharacterAnimator.SetBool("IsDead", _isDead); 
        if (CurrentCharacterState == CharacterState.Crouching)
        {
            CharacterAnimator.SetBool("Crouching", true);
        }
        else
        {
            CharacterAnimator.SetBool("Crouching", false);
        }
        if ((_forwardAxis <=0.1 && _forwardAxis >= -0.1 && _rightAxis <= 0.1 & _rightAxis >= -0.1) && Motor.GroundingStatus.IsStableOnGround && !CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Crouching Down") && !CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Standing") && !CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Falling To Landing") &&!CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            if (!_isCrouching)                                
            {
                for (int i = 0; i < 10; i++)
                {
                    armRigCrouching.weight -= 0.1f;
                    armRig.weight += 0.1f;
                }
            }
            else
            {
                armRig.weight = 0;
                for (int i = 0; i < 10; i++)
                {
                    armRig.weight -= 0.1f;
                    armRigCrouching.weight += 0.1f;
                }
            }

        }
        else
        {
            if (!_isCrouching)
            {
                for (int i = 0; i < 10; i++)
                {
                    armRig.weight -= 0.1f;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    armRigCrouching.weight -= 0.1f;
                }
            }
        }


        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            aimTarget.position = raycastHit.point;
            //debugTransform.position = raycastHit.point;
        }
        if (_stealthCooldownTimer > 0)
        {
            _stealthCooldownTimer -= Time.deltaTime;
            if (_stealthCooldownTimer <= 0)
            {
                _stealthCooldownTimer = 0;
                canStealth = true;
            }
        }
    }

    /// <summary>
    /// Handles movement state transitions and enter/exit callbacks
    /// </summary>
    public void TransitionToState(CharacterState newState)
    {
        CharacterState tmpInitialState = CurrentCharacterState;
        OnStateExit(tmpInitialState, newState);
        CurrentCharacterState = newState;
        OnStateEnter(newState, tmpInitialState);
    }

    /// <summary>
    /// Event when entering a state
    /// </summary>
    public void OnStateEnter(CharacterState state, CharacterState fromState)
    {
        switch (state)
        {
            case CharacterState.Default:
                {

                    gameObject.layer = 6;
                    break;
                }

            case CharacterState.Climbing:
                {

                    _rotationBeforeClimbing = Motor.TransientRotation;

                    Motor.SetMovementCollisionsSolvingActivation(false);
                    Motor.SetGroundSolvingActivation(false);
                    _climbingState = ClimbingState.Anchoring;

                    // Store the target position and rotation to snap to
                    _ladderTargetPosition = _activeLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
                    _ladderTargetRotation = _activeLadder.transform.rotation;
                    break;
                }

            //Chargins State Implementation Examples
            case CharacterState.Charging:
                {
                    gameObject.layer = 6;
                    _currentChargeVelocity = Motor.CharacterForward * ChargeSpeed;
                    _isStopped = false;
                    _timeSinceStartedCharge = 0f;
                    _timeSinceStopped = 0f;
                    break;
                }

            case CharacterState.Stealth:
                {
                    this.gameObject.layer = 8;
                    _timeSinceStealth = 0f;
                    _isStealth = true;
                    break;
                }

            case CharacterState.Crouching:
                {

                    _isGoingToCrouch = true;
                    _shouldBeCrouching = true;
                    CharacterAnimator.SetBool("IsGoingToCrouch", _isGoingToCrouch);
                    if (!_isCrouching)
                    {
                        _isCrouching = true;
                        Motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
                        //MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// Event when exiting a state
    /// </summary>
    public void OnStateExit(CharacterState state, CharacterState toState)
    {
        switch (state)
        {
            case CharacterState.Default:
                {
                    break;
                }
            case CharacterState.Climbing:
                {
                    Motor.SetMovementCollisionsSolvingActivation(true);
                    Motor.SetGroundSolvingActivation(true);
                    break;
                }
        }
    }

    /// <summary>
    /// This is called every frame by MyPlayer in order to tell the character what its inputs are
    /// </summary>
    public void SetInputs(ref PlayerCharacterInputs inputs)
    {

        if (inputs.StealthDown && canStealth)
        {
            TransitionToState(CharacterState.Stealth);
        }
        //Chargins State Implementation Examples
        // Handle state transition from input
        if (inputs.ChargingDown)
        {
            TransitionToState(CharacterState.Charging);
        }

        if (inputs.CrouchDown && !_isCrouching)
        {
            TransitionToState(CharacterState.Crouching);
        }
        if (inputs.CrouchUp && _isCrouching)
        {
            _shouldBeCrouching = false;
            TransitionToState(CharacterState.Default);
        }

        _ladderUpDownInput = inputs.MoveAxisForward;
        if (inputs.ClimbLadder)
        {
            if (Motor.CharacterOverlap(Motor.TransientPosition, Motor.TransientRotation, _probedColliders, InteractionLayer, QueryTriggerInteraction.Collide) > 0)
            {
                if (_probedColliders[0] != null)
                {
                    // Handle ladders
                    MyLadder ladder = _probedColliders[0].gameObject.GetComponent<MyLadder>();
                    if (ladder)
                    {
                        // Transition to ladder climbing state
                        if (CurrentCharacterState == CharacterState.Default)
                        {
                            _activeLadder = ladder;
                            TransitionToState(CharacterState.Climbing);
                        }
                        // Transition back to default movement state
                        else if (CurrentCharacterState == CharacterState.Climbing)
                        {
                            _climbingState = ClimbingState.DeAnchoring;
                            _ladderTargetPosition = Motor.TransientPosition;
                            _ladderTargetRotation = _rotationBeforeClimbing;
                        }
                    }
                }
            }
        }
        // Clamp input
        //Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

        // Calculate camera direction and rotation on the character plane
        Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
        }
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {
                    //Move and look inputs
                    //_moveInputVector = cameraPlanarRotation * moveInputVector;
                    _lookInputVector = cameraPlanarDirection;

                    // Axis inputs
                    _targetForwardAxis = inputs.MoveAxisForward;
                    _targetRightAxis = inputs.MoveAxisRight;

                    // Jumping input
                    if (inputs.JumpDown)
                    {
                        _timeSinceJumpRequested = 0f;
                        _jumpRequested = true;
                    }

                    break;
                }
            case CharacterState.Crouching:
                {
                    //Move and look inputs
                    //_moveInputVector = cameraPlanarRotation * moveInputVector;
                    _lookInputVector = cameraPlanarDirection;

                    // Axis inputs
                    _targetForwardAxis = inputs.MoveAxisForward;
                    _targetRightAxis = inputs.MoveAxisRight;
                    break;
                }
        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called before the character begins its movement update
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {                    
                    break;
                }

            //Chargins State Implementation Examples
            case CharacterState.Charging:
                {                    
                    // Update times
                    _timeSinceStartedCharge += deltaTime;
                    if (_isStopped)
                    {
                        _timeSinceStopped += deltaTime;
                    }
                    break;
                }

            case CharacterState.Stealth:
                {
                    canStealth = false;
                    if(_isStealth)
                    _timeSinceStealth += deltaTime;
                    break;
                }

            case CharacterState.Crouching:
                {
                    _isGoingToCrouch = false;
                    CharacterAnimator.SetBool("IsGoingToCrouch", _isGoingToCrouch);
                    break;
                }


        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its rotation should be right now. 
    /// This is the ONLY place where you should set the character's rotation
    /// </summary>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    switch (CurrentCharacterState)
    {
        case CharacterState.Default:
            {
                    if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
                    {
                        // Smoothly interpolate from current to target look direction
                        Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                        // Set the current rotation (which will be used by the KinematicCharacterMotor)
                        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                    }
                    currentRotation = _rootMotionRotationDelta * currentRotation;
                    break;
            }
        case CharacterState.Stealth:
            {
                if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
                {
                    // Smoothly interpolate from current to target look direction
                    Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                    // Set the current rotation (which will be used by the KinematicCharacterMotor)
                    currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                }
                currentRotation = _rootMotionRotationDelta * currentRotation;
                break;
            }
        case CharacterState.Crouching:
            {
                if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
                {
                    // Smoothly interpolate from current to target look direction
                    Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                    // Set the current rotation (which will be used by the KinematicCharacterMotor)
                    currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                }
                currentRotation = _rootMotionRotationDelta * currentRotation;
                break;
            }

        case CharacterState.Climbing:
            {
                switch (_climbingState)
                {
                    case ClimbingState.Climbing:
                        currentRotation = _activeLadder.transform.rotation;
                        break;
                    case ClimbingState.Anchoring:
                    case ClimbingState.DeAnchoring:
                        currentRotation = Quaternion.Slerp(_anchoringStartRotation, _ladderTargetRotation, (_anchoringTimer / AnchoringDuration));
                        break;
                }
                break;
            }
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
    switch (CurrentCharacterState)
    {
        case CharacterState.Default:
            {                
                if (Motor.GroundingStatus.IsStableOnGround)
                {                        
                    if (deltaTime > 0)
                    {
                        // The final velocity is the velocity from root motion reoriented on the ground plane
                        currentVelocity = _rootMotionPositionDelta / deltaTime;
                        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                    }
                    else
                    {
                        // Prevent division by zero
                        currentVelocity = Vector3.zero;
                    }
                }
                else
                {                   
                    if (_forwardAxis > 0f)
                    {
                        // If we want to move, add an acceleration to the velocity
                        Vector3 _targetMovementVelocity = Motor.CharacterForward * _forwardAxis * MaxAirMoveSpeed;
                        Vector3 velocityDiff = Vector3.ProjectOnPlane(_targetMovementVelocity - currentVelocity, Gravity);
                        currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                    }
                        
                    if (_rightAxis != 0f)
                    {
                        Vector3 _targetMovementVelocity = Motor.CharacterRight * _rightAxis * MaxAirMoveSpeed;
                        Vector3 velocityDiff = Vector3.ProjectOnPlane(_targetMovementVelocity - currentVelocity, Gravity);
                        currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                    }


                        // Gravity
                        currentVelocity += Gravity * deltaTime;

                    // Drag
                    currentVelocity *= (1f / (1f + (Drag * deltaTime)));
                }

                // Handle jumping
                
                    _jumpedThisFrame = false;
                    _timeSinceJumpRequested += deltaTime;
                    if (_jumpRequested)
                    {
                        // Handle double jump
                        if (AllowDoubleJump)
                        {
                            if (_jumpConsumed && !_doubleJumpConsumed && (AllowJumpingWhenSliding ? !Motor.GroundingStatus.FoundAnyGround : !Motor.GroundingStatus.IsStableOnGround))
                            {
                                Motor.ForceUnground(0.1f);

                                // Add to the return velocity and reset jump state
                                currentVelocity += (Motor.CharacterUp * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                _jumpRequested = false;
                                _doubleJumpConsumed = true;
                                _jumpedThisFrame = true;
                            }
                        }

                        // See if we actually are allowed to jump
                        if (_canWallJump ||
                            (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime)))
                        {
                            // Calculate jump direction before ungrounding
                            Vector3 jumpDirection = Motor.CharacterUp;
                            if (_canWallJump)
                            {
                                jumpDirection = _wallJumpNormal;
                            }
                            else if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                            {
                                jumpDirection = Motor.GroundingStatus.GroundNormal;
                            }

                            // Makes the character skip ground probing/snapping on its next update. 
                            // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                            Motor.ForceUnground(0.1f);

                            // Add to the return velocity and reset jump state
                            currentVelocity += (jumpDirection * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                            _jumpRequested = false;
                            _jumpConsumed = true;
                            _jumpedThisFrame = true;
                        }
                    }

                    // Reset wall jump
                    _canWallJump = false;
                

                // Take into account additive velocity
                if (_internalVelocityAdd.sqrMagnitude > 0f)
                {
                    currentVelocity += _internalVelocityAdd;
                    _internalVelocityAdd = Vector3.zero;
                }
                break;
            }

            //Chargins State Implementation Examples
            case CharacterState.Charging:
                {
                    // If we have stopped and need to cancel velocity, do it here
                    if (_mustStopVelocity)
                    {
                        currentVelocity = Vector3.zero;
                        _mustStopVelocity = false;
                    }

                    if (_isStopped)
                    {
                        // When stopped, do no velocity handling except gravity
                        currentVelocity += Gravity * deltaTime;
                    }
                    else
                    {
                        // When charging, velocity is always constant
                        float previousY = currentVelocity.y;
                        currentVelocity = _currentChargeVelocity;
                        currentVelocity.y = previousY;
                        currentVelocity += Gravity * deltaTime;
                    }
                    break;
                }
            case CharacterState.Stealth:
                {
                    if (Motor.GroundingStatus.IsStableOnGround)
                    {
                        if (deltaTime > 0)
                        {
                            // The final velocity is the velocity from root motion reoriented on the ground plane
                            currentVelocity = _rootMotionPositionDelta / deltaTime;
                            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                        }
                        else
                        {
                            // Prevent division by zero
                            currentVelocity = Vector3.zero;
                        }
                    }
                    else
                    {
                        if (_forwardAxis > 0f)
                        {
                            // If we want to move, add an acceleration to the velocity
                            Vector3 _targetMovementVelocity = Motor.CharacterForward * _forwardAxis * MaxAirMoveSpeed;
                            Vector3 velocityDiff = Vector3.ProjectOnPlane(_targetMovementVelocity - currentVelocity, Gravity);
                            currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                        }

                        if (_rightAxis != 0f)
                        {
                            Vector3 _targetMovementVelocity = Motor.CharacterRight * _rightAxis * MaxAirMoveSpeed;
                            Vector3 velocityDiff = Vector3.ProjectOnPlane(_targetMovementVelocity - currentVelocity, Gravity);
                            currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                        }


                        // Gravity
                        currentVelocity += Gravity * deltaTime;

                        // Drag
                        currentVelocity *= (1f / (1f + (Drag * deltaTime)));
                    }

                    // Handle jumping

                    _jumpedThisFrame = false;
                    _timeSinceJumpRequested += deltaTime;
                    if (_jumpRequested)
                    {
                        // Handle double jump
                        if (AllowDoubleJump)
                        {
                            if (_jumpConsumed && !_doubleJumpConsumed && (AllowJumpingWhenSliding ? !Motor.GroundingStatus.FoundAnyGround : !Motor.GroundingStatus.IsStableOnGround))
                            {
                                Motor.ForceUnground(0.1f);

                                // Add to the return velocity and reset jump state
                                currentVelocity += (Motor.CharacterUp * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                _jumpRequested = false;
                                _doubleJumpConsumed = true;
                                _jumpedThisFrame = true;
                            }
                        }

                        // See if we actually are allowed to jump
                        if (_canWallJump ||
                            (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime)))
                        {
                            // Calculate jump direction before ungrounding
                            Vector3 jumpDirection = Motor.CharacterUp;
                            if (_canWallJump)
                            {
                                jumpDirection = _wallJumpNormal;
                            }
                            else if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                            {
                                jumpDirection = Motor.GroundingStatus.GroundNormal;
                            }

                            // Makes the character skip ground probing/snapping on its next update. 
                            // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                            Motor.ForceUnground(0.1f);

                            // Add to the return velocity and reset jump state
                            currentVelocity += (jumpDirection * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                            _jumpRequested = false;
                            _jumpConsumed = true;
                            _jumpedThisFrame = true;
                        }
                    }

                    // Reset wall jump
                    _canWallJump = false;


                    // Take into account additive velocity
                    if (_internalVelocityAdd.sqrMagnitude > 0f)
                    {
                        currentVelocity += _internalVelocityAdd;
                        _internalVelocityAdd = Vector3.zero;
                    }
                    break;
                }
            case CharacterState.Crouching:
                {
                    if (Motor.GroundingStatus.IsStableOnGround)
                    {
                        if (deltaTime > 0)
                        {
                            // The final velocity is the velocity from root motion reoriented on the ground plane
                            currentVelocity = _rootMotionPositionDelta / deltaTime;
                            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                        }
                        else
                        {
                            // Prevent division by zero
                            currentVelocity = Vector3.zero;
                        }
                    }
                    else
                    {
                        if (_forwardAxis > 0f)
                        {
                            // If we want to move, add an acceleration to the velocity
                            Vector3 _targetMovementVelocity = Motor.CharacterForward * _forwardAxis * MaxAirMoveSpeed;
                            Vector3 velocityDiff = Vector3.ProjectOnPlane(_targetMovementVelocity - currentVelocity, Gravity);
                            currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                        }

                        if (_rightAxis != 0f)
                        {
                            Vector3 _targetMovementVelocity = Motor.CharacterRight * _rightAxis * MaxAirMoveSpeed;
                            Vector3 velocityDiff = Vector3.ProjectOnPlane(_targetMovementVelocity - currentVelocity, Gravity);
                            currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                        }


                        // Gravity
                        currentVelocity += Gravity * deltaTime;

                        // Drag
                        currentVelocity *= (1f / (1f + (Drag * deltaTime)));
                    }

                    // Take into account additive velocity
                    if (_internalVelocityAdd.sqrMagnitude > 0f)
                    {
                        currentVelocity += _internalVelocityAdd;
                        _internalVelocityAdd = Vector3.zero;
                    }                    
                    break;
                }
            case CharacterState.Climbing:
                {
                    currentVelocity = Vector3.zero;

                    switch (_climbingState)
                    {
                        case ClimbingState.Climbing:
                            currentVelocity = (_ladderUpDownInput * _activeLadder.transform.up).normalized * ClimbingSpeed;
                            break;
                        case ClimbingState.Anchoring:
                        case ClimbingState.DeAnchoring:
                            Vector3 tmpPosition = Vector3.Lerp(_anchoringStartPosition, _ladderTargetPosition, (_anchoringTimer / AnchoringDuration));
                            currentVelocity = Motor.GetVelocityForMovePosition(Motor.TransientPosition, tmpPosition, deltaTime);
                            break;
                    }
                    break;
                }
        }
    }



    // Update is called once per frame


    public void AfterCharacterUpdate(float deltaTime)
    {
        _rootMotionPositionDelta = Vector3.zero;
        _rootMotionRotationDelta = Quaternion.identity;
        switch (CurrentCharacterState)
    {
        case CharacterState.Default:
            {
                // Handle jump-related values
                
                    // Handle jumping pre-ground grace period
                    if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                    {
                        
                        _jumpRequested = false;
                    }

                    if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                    {
                        // If we're on a ground surface, reset jumping values
                        if (!_jumpedThisFrame)
                        {
                            _doubleJumpConsumed = false;
                            _jumpConsumed = false;
                        }
                        _timeSinceLastAbleToJump = 0f;
                    }
                    else
                    {
                        // Keep track of time since we were last able to jump (for grace period)
                        _timeSinceLastAbleToJump += deltaTime;
                    }
                    if (_isCrouching && !_shouldBeCrouching)
                    {
                        // Handle uncrouching
                        // Do an overlap test with the character's standing height to see if there are any obstructions
                        Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                        if (Motor.CharacterCollisionsOverlap(
                                Motor.TransientPosition,
                                Motor.TransientRotation,
                                _probedColliders) > 0)
                        {
                            // If obstructions, just stick to crouching dimensions
                            Motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
                        }
                        else
                        {
                            // If no obstructions, uncrouch
                            //MeshRoot.localScale = new Vector3(1f, 1f, 1f);
                            _isCrouching = false;
                        }
                    }

                    break;
                    // Reset root motion deltas
                    }

            //Chargins State Implementation Examples
            case CharacterState.Charging:
                {
                    // Detect being stopped by elapsed time
                    if (!_isStopped && _timeSinceStartedCharge > MaxChargeTime)
                    {
                        _mustStopVelocity = true;
                        _isStopped = true;
                    }

                    // Detect end of stopping phase and transition back to default movement state
                    if (_timeSinceStopped > StoppedTime)
                    {
                        TransitionToState(CharacterState.Default);
                    }
                    break;
                }

            case CharacterState.Stealth:
                {
                    if (_isStealth && _timeSinceStealth > StealthTime)
                    {
                        _isStealth = false;                        
                        _stealthCooldownTimer = 45f;
                        TransitionToState(CharacterState.Default);
                    }
                    break;
                }
            case CharacterState.Crouching:
                {
                    break;
                }
            case CharacterState.Climbing:
                {
                    switch (_climbingState)
                    {
                        case ClimbingState.Climbing:
                            // Detect getting off ladder during climbing
                            _activeLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
                            if (Mathf.Abs(_onLadderSegmentState) > 0.05f)
                            {
                                _climbingState = ClimbingState.DeAnchoring;

                                // If we're higher than the ladder top point
                                if (_onLadderSegmentState > 0)
                                {
                                    _ladderTargetPosition = _activeLadder.TopReleasePoint.position;
                                    _ladderTargetRotation = _activeLadder.TopReleasePoint.rotation;
                                }
                                // If we're lower than the ladder bottom point
                                else if (_onLadderSegmentState < 0)
                                {
                                    _ladderTargetPosition = _activeLadder.BottomReleasePoint.position;
                                    _ladderTargetRotation = _activeLadder.BottomReleasePoint.rotation;
                                }
                            }
                            break;
                        case ClimbingState.Anchoring:
                        case ClimbingState.DeAnchoring:
                            // Detect transitioning out from anchoring states
                            if (_anchoringTimer >= AnchoringDuration)
                            {
                                if (_climbingState == ClimbingState.Anchoring)
                                {
                                    _climbingState = ClimbingState.Climbing;
                                }
                                else if (_climbingState == ClimbingState.DeAnchoring)
                                {
                                    TransitionToState(CharacterState.Default);
                                }
                            }

                            // Keep track of time since we started anchoring
                            _anchoringTimer += deltaTime;
                            break;
                    }
                    break;
                }
        }
    }



    public bool IsColliderValidForCollisions(Collider coll)
    {
        if (IgnoredColliders.Contains(coll))
        {
            return false;
        }
        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    switch (CurrentCharacterState)
    {
        case CharacterState.Default:
            {
                // We can wall jump only if we are not stable on ground and are moving against an obstruction
                if (AllowWallJump && !Motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable)
                {
                    _canWallJump = true;
                    _wallJumpNormal = hitNormal;
                }
                break;
            }

            //Chargins State Implementation Examples
            case CharacterState.Charging:
                {
                    // Detect being stopped by obstructions
                    if (!_isStopped && !hitStabilityReport.IsStable && Vector3.Dot(-hitNormal, _currentChargeVelocity.normalized) > 0.5f)
                    {
                        _mustStopVelocity = true;
                        _isStopped = true;
                    }
                    break;
                }
            case CharacterState.Stealth:
                {
                    // We can wall jump only if we are not stable on ground and are moving against an obstruction
                    if (AllowWallJump && !Motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable)
                    {
                        _canWallJump = true;
                        _wallJumpNormal = hitNormal;
                    }
                    break;
                }
        }
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        // Handle landing and leaving ground
        if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
        {
            OnLanded();
        }
        else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
        {
            OnLeaveStableGround();
        }
    }

    protected void OnLanded()
    {
        Debug.Log("Landed");
    }

    protected void OnLeaveStableGround()
    {
        Debug.Log("Left ground");
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void AddVelocity(Vector3 velocity)
    {
    switch (CurrentCharacterState)
        {
            case CharacterState.Default:
            {
            _internalVelocityAdd += velocity;
            break;
            }
    }
    }
    private void OnAnimatorMove()
    {
        // Accumulate rootMotion deltas between character updates 
        _rootMotionPositionDelta += CharacterAnimator.deltaPosition;
        //_rootMotionRotationDelta = CharacterAnimator.deltaRotation * _rootMotionRotationDelta;
    }

}
