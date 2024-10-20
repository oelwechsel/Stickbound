using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
//using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Rendering.Universal;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class Entity : MonoBehaviour
{
    // Things that every Enemy has!!!
    // Set velocity oder isGorunded usw... Dinge die jeder Enemy haben sollte

    public FiniteStateMachine stateMachine;

    public DataFor_Entity entityData;
    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }



    public AnimationToStateMachine atsm { get; private set; }

    [SerializeField]
    public Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;

    [SerializeField]
    public Transform playerCheck;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    public Transform playerBehindCheck;

    public Transform StartPos;



    public bool isHopping = false;

    [SerializeField]
    public float currentHealth;

    private float currentStunResistance;

    private float lastDamageTime;

    public int lastDamageDirection { get; private set; }


    public Vector2 velocityWorkspace;


    protected bool isStunned;

    protected bool isDead;


    public GameManager gameManager;



    //TouchDamage
    private float lastTouchDamageTime;
    public float touchDamageCooldown;
    public float touchDamage;
    public float touchDamageWidth;
    public float touchDamageHeight;

    private Vector2 touchDamageBotLeft;
    private Vector2 touchDamageTopRight;

    private AttackDetails attackDetails;

    [SerializeField]
    private Transform touchDamageCheck;







    
    public virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        facingDirection = 1;

        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;

        aliveGO = transform.Find("Alive").gameObject; // GAME OBJECT MUSS AUCH ALIVE HEI?EN!!!!!
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();
        
        //StartPos = aliveGO.transform;
        //StartPos.position = aliveGO.transform.position;

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
        CheckTouchDamage();

        anim.SetFloat("yVelocity", rb.velocity.y);


        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }

        if (gameManager.respawn)
        {
            currentHealth = entityData.maxHealth;
        }
        

    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float _velocity)
    {
        velocityWorkspace.Set(facingDirection * _velocity, rb.velocity.y); // vll hier fehler!
        rb.velocity = velocityWorkspace;
    }

    // Für Knockback hernehmen
    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();

        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        //return Physics2D.BoxCast(wallCheck.position, new Vector2(1f, 1f), 0f, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatisGround);
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatisGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatisGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatisGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.BoxCast(playerCheck.position, new Vector2(1f, 8f), 0f, aliveGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
        //return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    // Vll hier noch Check PLayer Above Chekc

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.BoxCast(playerCheck.position, new Vector2(1f, 8f), 0f, aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
        //return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);

    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.BoxCast(playerCheck.position, new Vector2(1f, 8f), 0f, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
        //return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerBehind()
    {
        return Physics2D.BoxCast(playerBehindCheck.position, new Vector2(1f, 8f), 0f, aliveGO.transform.right * -1, entityData.playerBehindCheckDistance, entityData.whatIsPlayer);
        //return Physics2D.Raycast(playerBehindCheck.position, aliveGO.transform.right * -1, entityData.playerBehindCheckDistance, entityData.whatIsPlayer);
    }

    public virtual void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, entityData.whatIsPlayer);

            if(hit!= null)
            {
               
                lastTouchDamageTime = Time.time;
                attackDetails.damageAmount = touchDamage;
                attackDetails.position = aliveGO.transform.position;

                if (!isDead) 
                {
                    hit.SendMessage("Damage", attackDetails);
                }
                
            }
        }
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void DamageHop(float velocity, float damageDirection)
    {
        
            isHopping = true;
            velocityWorkspace.Set(1.5f * damageDirection, velocity);
            rb.velocity = velocityWorkspace;

    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;

        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;
        AudioManager.Instance.PlaySound("EnemyDestroyed");



        //isHopping = false;

        Instantiate(entityData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if (attackDetails.position.x > aliveGO.transform.position.x)
        {
            lastDamageDirection = -1;
            
        }
        else
        {
            lastDamageDirection = 1;
        }

        DamageHop(entityData.damageHopSpeed, lastDamageDirection);


        if (currentStunResistance <= 0f)
        {
            isStunned = true;
        }

        if (currentHealth <= 0f)
        {
            isDead = true;
        }

    }





    public virtual void Respawn()
    {
        if (gameManager.respawn && aliveGO.activeInHierarchy == false)
        {
            isDead = false;
            aliveGO.transform.position = StartPos.position;
            Debug.Log(StartPos + "StartPos");
            Debug.Log(aliveGO.transform.position + "Transform alive");
            //aliveGO.transform.position = StartPos.position;
            aliveGO.SetActive(true);

            //aliveGO.transform.position = StartPos.position;

            currentHealth = entityData.maxHealth;
            currentStunResistance = entityData.stunResistance;
        }
    }

    public virtual float WhatIsCurrentHealth()
    {
        return currentHealth;
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
        Gizmos.DrawLine(playerBehindCheck.position, playerBehindCheck.position + (Vector3)(Vector2.left * facingDirection * entityData.playerBehindCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.maxAgroDistance), 0.2f);

        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2)); ;
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2)); ;
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2)); ;

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);


    }






}
