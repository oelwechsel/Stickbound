using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
//using UnityEditor.Build.Content;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovementPlayer : MonoBehaviour
{
    #region Tipps


    // Str K + Str C auf markierten Bereich kommentiert alles aus!
    // Str K + Str D = Dopkument formatieren !
    // Str K + Str F = Bereich formatieren!


    // Jump Buffering


    //private bool canFlip = true;

    #endregion


    public bool couldGrappleHit;





    public AnimationClip wakeUpAnimation;
    public AnimationClip idleAnimation;
    public bool hasPlayedWakeUp = false;







    public float footstepDelay = 0.3f; // Die Verzögerung zwischen den Fußschritten
    private float lastFootstepTime;



    private bool knockback;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;
    [SerializeField]
    private Vector2 knockbackSpeed;


    private PlayerCombatTry playerCombat;
    private PlayerStats playerStats;

    SpriteRenderer spriteRenderer;



    [Header("Animation fix")]
    private bool isWalking;

    [Header("Base")]
    public Rigidbody2D rb;
    private Vector2 moveVector = Vector2.zero;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float airMoveSpeed = 8f;


    public Animator animator;




    [Header("CamerFollowObjectLerp")]
    [SerializeField] private CameraFollowObject _cameraFollowObject;



    [Header("Camera Manager")]
    private float _fallSpeedYDampingChangeThreshold;



    // zweiter Ansatz Jump
    [Header("Jump")]
    [SerializeField] private float gravityScale = 7f;
    [SerializeField] private float fallGravityScale = 15f;
    [SerializeField] private float JumpHeight = 4f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private bool _canJump = true;




    public bool canJump
    {
        get
        {
            return _canJump;
        }
        set
        {
            _canJump = value;
        }
    }



    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.08f;
    [SerializeField] private float coyoteTimeCounter;

    [Header("Jump Buffering - Funkt nicht bei kleinen Jumps")]
    [SerializeField] private float jumpBufferTime = 0f;
    [SerializeField] private float jumpBufferTimeCounter;




    [Header("Dash - Soll der Spieler während Dash Jumpen dürfen?(Am Boden)")]
    [SerializeField] public bool canDash = true;
    [SerializeField] private bool isInDashCooldown = false;
    [SerializeField] public bool isDashing;
    [SerializeField] private float dashingPower = 35f;
    [SerializeField] private float dashingTime = 0.1f;
    [SerializeField] private float dashingCooldown = 0.8f;
    [SerializeField] private TrailRenderer tr;






    [Header("GrapplingMechanik")]
    private LineRenderer lineRenderer;
    public LayerMask WhatYouCanGrappleOnTo;
    public LayerMask WhatYouShouldNotGrappleOnTo;
    public Camera cam;

    [Header("Grapple Attributes")]
    public float moveSpeedFromGrapple = 0.35f;
    public float maxGrappleLength = 15f;

    public bool didGrappleHit = false;

    public bool IsInGrapplingCooldown = false;
    public float grappleCooldown = 0.2f;
    public bool IsGrappling = false;

    public bool canGrapple = true;

    [Header("Terraria Grapple")]
    public int maxPoints = 1; // Wie viele Terraria GrappleHooks? In unserem Game nur 1
    private List<Vector2> points = new List<Vector2>();

    public Vector2 moveTo; // vll nicht public und öffentlich nötig


    [Header("Mouse Duration")]
    float mouseClickDuration = 0f;
    bool isMousePressed = false;
    float xVelocityAfterGrappleInfluencedByMouseDuration = 0;
    float xVelocityAfterGrappleInfluencedByMouseDurationClamped;








    // Properties .... Kann man Später in Touching Directions bzw jeweils in die entsprechende eigene Klasse ziehen damit auch Gegner dies benutzen können :)
    // IsOnCeiling muss noch geaddet werden

    [Header("GroundCheck - würde später in eine eigene Klasse geschrieben genauso wie wall und Ceiling check")]
    [SerializeField]
    private bool _isGrounded = true;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
        }
    }


    public ContactFilter2D CastFilter;
    public float groundDistance = 0.05f;
    BoxCollider2D touchingCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];





    [Header("Wall Check")]
    [SerializeField] private bool _isOnWall = false;

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        set
        {
            _isOnWall = value;
        }
    }

    [SerializeField] private Vector2 _wallCheckDirection;
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private float wallDistance = 0.1f;

    public Vector2 WallCheckDirection
    {
        get
        {
            if (transform.rotation.eulerAngles.y == 0)
            {
                _wallCheckDirection = Vector2.right;

            }
            else
            {
                _wallCheckDirection = Vector2.left;

            }
            return _wallCheckDirection;
        }
        set
        {

            _wallCheckDirection = value;

        }
    }






    //Air Move soll vll langsamer sein als am Boden
    [Header("MoveCheck - vll noch Air Move wichtig sonst kann sich der Spieler sehr frei in der Luft bewegen!")]
    [SerializeField]

    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
        }
    }



    [Header("flip the scale")]
    [SerializeField]
    private bool _isFacingRight = false;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            _isFacingRight = value;
        }
    }







    #region Standard Functions

    private void Awake()
    {
        //Time.timeScale = 0.5f; Für Tests gedacht
        rb = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        playerCombat = GetComponent<PlayerCombatTry>();
        playerStats = GetComponent<PlayerStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer.positionCount = 0; // no lines showing nur für grapplemechanik

        Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rotator);

        animator = GetComponent<Animator>(); // Animation Controller Reference in Movement Script

        hasPlayedWakeUp = false;
    }




    void Start()
    {

        PlayWakeUpAnimation();

        if (CameraManager.instance == null)
        {
            throw new NullReferenceException("CameraManager.instance is not assigned.");
            // Nur wegen NullPointerExceptions gemacht
            // or display an error message
            // or handle it in another way
        }
        else
        {
            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedDampingChangeThreshold;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Speedrun
        //if(playerStats.currentHealth <= 20f)
        // {
        //     moveSpeed = 20f;
        //     airMoveSpeed = 18f;
        // }
        // else
        // {
        //     moveSpeed = 14f;
        //     airMoveSpeed = 12f;
        // }

        RaycastHit2D falseHit = Physics2D.Raycast(rb.position, GetMousePositionAndReturnDirection(), maxGrappleLength, WhatYouShouldNotGrappleOnTo);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, GetMousePositionAndReturnDirection(), maxGrappleLength, WhatYouCanGrappleOnTo);

        if (hit.collider != null && falseHit.collider != null)
        {
            float distanceToHookable = Vector2.Distance(transform.position, hit.point);
            float distanceToCancelHook = Vector2.Distance(transform.position, falseHit.point);


            if (distanceToCancelHook < distanceToHookable)
            {
                couldGrappleHit = false;

            }
            else
            {
                couldGrappleHit = true;
            }
        }
        else if(hit.collider != null)
        {
            couldGrappleHit = true;
        }
        else
        {
            couldGrappleHit = false;
        }



        // Raycasts hier fehlt noch IsOnCeiling
        //IsGrounded = touchingCol.Cast(Vector2.down, CastFilter, groundHits, groundDistance) > 0;

        //Check Sorroundings!
        IsGrounded = touchingCol.Cast(Vector2.down, CastFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(WallCheckDirection, CastFilter, wallHits, wallDistance) > 0;

        //Debug.Log(couldGrappleHit);
        //Animation Parameters

        //Time.timeScale = 0.5f;
        //IsMove();
        //Idle();

        //Debug.Log(knockback);



        //FootstepSound
        //if (IsGrounded && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        //{
        //    if (Time.time - lastFootstepTime >= footstepDelay)
        //    {
        //        AudioManager.Instance.PlaySound("PlayerMove");
        //        lastFootstepTime = Time.time; // Aktualisiere den Zeitstempel
        //    }
        //}

        if (IsGrounded && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !IsOnWall && hasPlayedWakeUp)
        {
            if (Time.time - lastFootstepTime >= footstepDelay)
            {
                AudioManager.Instance.PlayRandomFootStepSound();
                lastFootstepTime = Time.time; // Aktualisiere den Zeitstempel
            }
        }



        UpdateAnimations();

        CombatStopMove();
        GetMousePosition();
        SetFacingDirection(moveVector);
        JumpBuffer();
        Coyotetimer();
        AdjustGravity();
        AdjustCameraYDamping();
        MouseDuration();
        OnGrapple();
    }



    private void FixedUpdate() //Physics based
    {

        // !isONWALL BEEINTRÄCHTIGT DEN GEGNER BUG
        if (!isDashing && !isInDashCooldown && !IsGrappling && !IsInGrapplingCooldown && !knockback && !playerCombat.isAttacking && hasPlayedWakeUp)
        {

            if (!IsGrounded && !knockback && canDash)
            {

                rb.velocity = new Vector2(moveVector.x * airMoveSpeed, rb.velocity.y); // Man könnte hier auch mit Forces Arbeiten!
            }
            else if (!knockback)
            {

                rb.velocity = new Vector2(moveVector.x * moveSpeed, rb.velocity.y); // Man könnte hier auch mit Forces Arbeiten!
                //rb.AddForce(new Vector2(moveVector.x * moveSpeed, 0),ForceMode2D.Impulse);
            }

        }
        // kein Wall Slide oder Jump
        //else if (IsOnWall && !IsGrappling && !IsGrounded)
        //{
        //rb.velocity = new Vector2(0f, rb.velocity.y);

        //}

        // Acceleration Decelleration und Top Speed vll noch einbauen wenn lustig
        CheckKnockback();
        //SetFacingDirection(moveVector);


    }

    #endregion




    #region Management


    private void AdjustGravity()
    {



        // Adjust Gravity when falling
        if (rb.velocity.y <= 0)
        {
            rb.gravityScale = fallGravityScale;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed)); // Clamps the max fall Speed des Spielers - Kann nicht ewig beschleunigen beim fallen
        }
        // Adjust Gravity when dashing
        if (isDashing)
        {
            rb.gravityScale = 0;
        }
        // Adjust Gravity when standing (merkt man eh nicht)
        if (IsGrounded)
        {
            rb.gravityScale = gravityScale;
        }

        if (IsGrappling)
        {
            rb.gravityScale = gravityScale;
        }

        if (IsInGrapplingCooldown && !isDashing)
        {
            rb.gravityScale = gravityScale;
        }






    }

    private void AdjustCameraYDamping()
    {
        // if falling past a certain speed threshold
        if (rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        //if standing still or moving up
        if (rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            //reset so it can be called again
            CameraManager.instance.LerpedFromPlayerFalling = false;

            CameraManager.instance.LerpYDamping(false);
        }
    }



    private void SetFacingDirection(Vector2 _moveVector)
    {

        // erste If ANweisung funkt nur wenn man noch nicht gerespawned ist
        Vector2 mouse = GetMousePosition();
        if (Input.GetMouseButtonDown(0))
        {

            if (rb.transform.position.x > mouse.x && IsFacingRight)
            {
                Turn();
            }
            else if (rb.transform.position.x < mouse.x && !IsFacingRight)
            {
                Turn();
            }
            
        }
        else
        {
            //when Grappling check
            if (IsGrappling && didGrappleHit)
            {
                if (moveTo.x > rb.transform.position.x + 0.2f) // +1 fixt wenn man grade nach oben grapplet
                {

                    if (!IsFacingRight)
                    {
                        Turn();
                    }
                }
                else if (moveTo.x < rb.transform.position.x - 0.2f)
                {

                    if (IsFacingRight)
                    {
                        Turn();
                    }
                }
            }

            else
            if (!playerCombat.isAttacking)
            {
                //normal check
                if (_moveVector.x > 0 && !IsFacingRight)
                {
                    Turn();
                    //IsFacingRight = true;
                }
                else if (_moveVector.x < 0 && IsFacingRight)
                {
                    Turn();
                    //IsFacingRight = false;
                }
            }

            
        }






        // For Animations
        //vorher mit  rb.velocity.x != 0f
        if (rb.velocity.x != 0f && !isDashing && IsGrounded && !isInDashCooldown && !IsOnWall && !knockback)
        {
            isWalking = true;
            //AudioManager.Instance.PlaySound("PlayerMove");
        }
        else
        {
            isWalking = false;
        }





    }

    // Rotation nicht mit scale = -1 sondern rotation !!!
    private void Turn()
    {

        if (hasPlayedWakeUp)
        {
            if (IsFacingRight)
            {
                Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                IsFacingRight = !IsFacingRight;


                //turn the camerafollowObject
                if (_cameraFollowObject == null)
                {
                    //Debug.LogWarning("camerFollowObject==null");
                }

                //_cameraFollowObject.CallTurn();
            }
            else
            {
                Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                IsFacingRight = !IsFacingRight;

                // turn the camerafollowObject
                if (_cameraFollowObject == null)
                {
                    //Debug.LogWarning("camerFollowObject==null");
                }

                //_cameraFollowObject.CallTurn();
            }
        }
            
        







    }


    #region try out animations

    private void PlayWakeUpAnimation()
    {
        if (!hasPlayedWakeUp)
        {
            animator.Play(wakeUpAnimation.name);
            

        }
    }

    public void StopWakeUp()
    {

        hasPlayedWakeUp = true;
        animator.SetBool("idle", true);
        animator.Play(idleAnimation.name);
    }


    private void IsMove()
    {
        if (IsMoving && IsGrounded && !isDashing && !IsGrappling && hasPlayedWakeUp)
        {
            animator.SetBool("run", true);
        }
        else if (IsMoving && !IsGrounded && !isDashing && !IsGrappling && hasPlayedWakeUp)
        {
            animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("run", false);
        }

    }

    private void Idle()
    {
        if (!IsMoving && IsGrounded && !isDashing && !IsGrappling)
        {
            animator.SetBool("idle", true);
        }
        else
        {
            animator.SetBool("idle", false);
        }
    }


    #endregion

    #endregion





    #region Move
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();

        if (context.duration > 0)
        {
            //AudioManager.Instance.PlaySound("PlayerMove");
        }
        if (context.performed)
        {
            
            IsMoving = true;
            IsInGrapplingCooldown = false; // Cancel GH when Moving
        }
        else
        {
            IsMoving = false;
        }

        // Vorher  hier SetFacingDirection(moveVector);
    }
    #endregion



    #region Jump
    private void Coyotetimer()
    {
        if (IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void JumpBuffer()
    {
        if (jumpBufferTimeCounter > 0)
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }

    }




    // wenn jump buffer end merken wie lange jump gedrückt
    // aufschreiben 
    // dann Variablen WAs Jump High und Was Jump low? 
    // Dann gravity Scale dementsprechend setzen.


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }

        // Main Jump
        if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f && canJump && hasPlayedWakeUp)   // Vorher:  context.started && IsGrounded
        {
            FindObjectOfType<AudioManager>().PlaySound("PlayerJump");
            rb.gravityScale = gravityScale;
            Vector2 ResetVelocityVector = rb.velocity;
            ResetVelocityVector.y = 0;
            rb.velocity = ResetVelocityVector;


            float JumpForce = Mathf.Sqrt(JumpHeight * (Physics2D.gravity.y * rb.gravityScale) * -2) * rb.mass;
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

            jumpBufferTimeCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // longer time in air vll nícht relevant! wird wshl iwo überschrieben!
        if (rb.velocity.y.Equals(0))
        {

            rb.gravityScale = 0f;

            //Debug.Log("flying");

        }


        // Variable Sprung Höhe setzt beim tasten loslassen gravity auf fall Gravity
        if (context.canceled && rb.velocity.y >= 0)
        {

            if (gravityScale < fallGravityScale)
            {
                rb.gravityScale = fallGravityScale;
            }
            else
            {
                rb.gravityScale += gravityScale;   // KA WARUM FUNKTIONIERT??? NUR NOTWENDIG WENN SCHNELL NACH OBEN UND LANGSAM NACH UNTEN (gravityScale > fallgravityScale)
            }


            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); alter Ansatz

            coyoteTimeCounter = 0f;
            jumpBufferTimeCounter = 0f;


        }


    }
    #endregion











    #region Dash
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash == true && hasPlayedWakeUp)
        {
            StartCoroutine(DashingAbility());
        }
    }

    private void DashInputMethod()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector2.right * dashingPower;

        }

        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = Vector2.left * dashingPower;
        }

        else
        {
            if (transform.rotation.y == 0f)
            {
                rb.velocity = new Vector2(dashingPower, 0f);
            }
            else
            {
                rb.velocity = new Vector2(-1 * dashingPower, 0f);
            }

        }
    }



    private IEnumerator DashingAbility()

    {
        canJump = false;
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        IsMoving = true;
        //playerStats.isInvulnerable = true;

        Vector2 tempVector = rb.velocity;
        tempVector.y = 0f;
        rb.velocity = tempVector;
        FindObjectOfType<AudioManager>().PlaySound("PlayerDash");
        DashInputMethod();

        yield return new WaitForSeconds(dashingTime);
        //playerStats.isInvulnerable = false;
        isDashing = false;
        isInDashCooldown = true;


        if (transform.rotation.y == 0f)
        {
            rb.velocity = new Vector2(dashingPower * 0.5f, 0f);
        }
        else
        {
            rb.velocity = new Vector2(-1 * dashingPower * 0.5f, 0f);
        }

        canJump = true;



        tr.emitting = false;

        yield return new WaitForSeconds(0.03f); // Für wie lange die Kraft nach dem Dash einwirken soll!
        isInDashCooldown = false;

        IsMoving = false;
        rb.velocity = tempVector;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion







    #region Mouse related Functions

    void MouseDuration()
    {

        if (Input.GetMouseButtonDown(1))
        {
            isMousePressed = true;
            mouseClickDuration = 0f;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isMousePressed = false;
            // Hier kannst du die Gesamtdauer der Maustaste verwenden
            //Debug.Log("Maustaste wurde " + mouseClickDuration + " Sekunden lang gedrückt.");
        }

        if (isMousePressed)
        {
            mouseClickDuration += Time.deltaTime;
            // Hier kannst du während des Klicks weitere Aktionen ausführen

            xVelocityAfterGrappleInfluencedByMouseDuration += Time.deltaTime;
            xVelocityAfterGrappleInfluencedByMouseDurationClamped = Mathf.Clamp(xVelocityAfterGrappleInfluencedByMouseDuration, 0f, 0.5f);






            //Debug.Log("Maustaste wurde geclamped"+ xVelocityAfterGrappleInfluencedByMouseDurationClamped);
        }

    }


    public Vector2 GetMousePositionAndReturnDirection()
    {


        //Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Input.mousePosition;
        Vector2 GetWorldPositionOnPlaneForMouse = GetWorldPositionOnPlane(mousePos, -1f);

        Vector2 direction = (GetWorldPositionOnPlaneForMouse - rb.position).normalized; // Im tutorial (Vector2)transform.position statt rb.position
        return direction;
    }

    public Vector2 GetMousePosition()
    {


        //Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Input.mousePosition;
        Vector2 GetWorldPositionOnPlaneForMouse = GetWorldPositionOnPlane(mousePos, -1f);
        return GetWorldPositionOnPlaneForMouse;

    }

    #endregion









    #region Grappling Hook








    Vector2 centroid(Vector2[] points)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }
        center /= points.Length; // Find average of all Points
        return center;
    }



    public void OnGrapple()
    {

        if (Input.GetMouseButtonDown(1) && canGrapple)
        {

            //Debug.Log(IsGrappling);

            RaycastHit2D falseHit = Physics2D.Raycast(rb.position, GetMousePositionAndReturnDirection(), maxGrappleLength, WhatYouShouldNotGrappleOnTo);
            RaycastHit2D hit = Physics2D.Raycast(rb.position, GetMousePositionAndReturnDirection(), maxGrappleLength, WhatYouCanGrappleOnTo);

            if (hit.collider != null && falseHit.collider != null)
            {
                float distanceToHookable = Vector2.Distance(transform.position, hit.point);
                float distanceToCancelHook = Vector2.Distance(transform.position, falseHit.point);

                if (distanceToCancelHook < distanceToHookable)
                {
                    didGrappleHit = false;

                    return;
                }
            }

            if (hit.collider != null) // hat der Raycast was gehittet?
            {
                Vector2 hitPoint = hit.point;
                points.Add(hitPoint);

                didGrappleHit = true;
                canGrapple = false;
                IsGrappling = true;
                //canDash = false;
            }
            else
            {
                didGrappleHit = false;
            }

            if (points.Count > maxPoints) // nur nötig wenn mehr als ein Grapple
            {
                points.RemoveAt(0);
            }

        }


        if (points.Count > 0 && didGrappleHit)
        {
            // Find Direction
            moveTo = centroid(points.ToArray());
            Vector2 moveDirection = (moveTo - rb.position).normalized;



            //lineRenderer Shit
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = points.Count * 2;

            for (int n = 0, j = 0; n < points.Count * 2; n += 2, j++) //?
            {
                lineRenderer.SetPosition(n, rb.position);
                lineRenderer.SetPosition(n + 1, points[j]);
            }




            // Move Player To Grapple
            float tempVelocityGrapple = moveSpeedFromGrapple;

            if (moveTo.y < rb.position.y)
            {
                tempVelocityGrapple *= 1.1f;
            }
            else
            {
                tempVelocityGrapple = moveSpeedFromGrapple;
            }

            rb.MovePosition(Vector2.MoveTowards(rb.position, moveTo, tempVelocityGrapple));












            if (Input.GetMouseButtonUp(1) && IsGrappling)
            {

                //canDash = true;
                IsGrappling = false;
                Detatch();
                IsInGrapplingCooldown = true;

                Vector2 tempVector = rb.velocity;



                // Vll hier noch einbauen was passiert wenn man die Tasten drückt nach dem Grappling Hook
                if (moveVector.x == 0f && didGrappleHit)
                {

                    //Debug.Log(xVelocityAfterGrappleInfluencedByMouseDurationClamped);
                    if (rb.position.y < moveTo.y && IsFacingRight)
                    {

                        tempVector = new Vector2(20 * xVelocityAfterGrappleInfluencedByMouseDurationClamped, 0.4f);
                        xVelocityAfterGrappleInfluencedByMouseDuration = 0; // reset
                    }
                    else if (rb.position.y < moveTo.y && !IsFacingRight)
                    {
                        tempVector = new Vector2(-(20 * xVelocityAfterGrappleInfluencedByMouseDurationClamped), 0.4f);
                        xVelocityAfterGrappleInfluencedByMouseDuration = 0;
                    }
                    else if (rb.position.y > moveTo.y)
                    {
                        tempVector = new Vector2(0, -60); // Für krassen Downfall Movement!
                    }
                }
                else if (moveVector.x != 0f && didGrappleHit)
                {

                    if (IsFacingRight && moveVector.x > 0f)
                    {
                        tempVector = new Vector2(30 * xVelocityAfterGrappleInfluencedByMouseDurationClamped, 0.4f);
                        xVelocityAfterGrappleInfluencedByMouseDuration = 0; // reset
                    }
                    else if (IsFacingRight && moveVector.x < 0f)
                    {
                        tempVector = new Vector2((10 * xVelocityAfterGrappleInfluencedByMouseDurationClamped), 0.4f);
                        xVelocityAfterGrappleInfluencedByMouseDuration = 0;
                    }
                    else if (!IsFacingRight && moveVector.x > 0f)
                    {
                        tempVector = new Vector2(-(10 * xVelocityAfterGrappleInfluencedByMouseDurationClamped), 0.4f);
                        xVelocityAfterGrappleInfluencedByMouseDuration = 0; // reset
                    }
                    else if (!IsFacingRight && moveVector.x < 0f)
                    {
                        tempVector = new Vector2(-(30 * xVelocityAfterGrappleInfluencedByMouseDurationClamped), 0.4f);
                        xVelocityAfterGrappleInfluencedByMouseDuration = 0; // reset
                    }
                }









                rb.velocity = tempVector;





                //canGrapple = true;

                StartCoroutine(GrappleCooldown());
            }

            else if (IsGrappling)
            {
                if(isDashing && didGrappleHit)
                {
                    IsGrappling = false;

                    IsInGrapplingCooldown = true;
                    StartCoroutine(GrappleCooldown());
                    Detatch();
                    StartCoroutine(DashingAbility());
                }
               

            }







        }




    }


    void Detatch()
    {
        lineRenderer.positionCount = 0;
        points.Clear();
    }

    private IEnumerator GrappleCooldown()
    {

        yield return new WaitForSeconds(grappleCooldown);
        IsInGrapplingCooldown = false;

        canGrapple = true;
    }



    #endregion





    private void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isGrounded", IsGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }


    public void DisableFlip()
    {
        //canFlip = true;
    }

    public void EnableFlip()
    {
        //canFlip = false;
    }




    public void Knockback(int direction)
    {

            knockback = true;
            knockbackStartTime = Time.time;
            rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
         
    }

    public void KnockbackSpikes(float direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(0f, 10 * direction);
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {

            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            knockback = false;
        }
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    private void CombatStopMove()
    {
        if (playerCombat.isAttacking && IsGrounded && !isDashing)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        else if ((playerCombat.isAttacking && !IsGrounded && !isDashing))
        {
            rb.velocity = new Vector2(airMoveSpeed * moveVector.x, rb.velocity.y);
        }


    }

    public bool IsInGrappleRange()
    {

        //Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Input.mousePosition;
        Vector2 GetWorldPositionOnPlaneForMouse = GetWorldPositionOnPlane(mousePos, -1f);

        float distanceToMouse = Vector2.Distance(transform.position, GetWorldPositionOnPlaneForMouse);



        if (distanceToMouse <= maxGrappleLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // fix für perspective camera

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxGrappleLength);
    }
}
