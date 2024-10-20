using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class PlayerCombatTry : MonoBehaviour
{
    


    private PlayerStats PS;


    // IN MOVEMENT SCRIPT NOCH DISABLEFLIP UND ENABELE FLIP METHODE EINFÜGEN
    // Player Stats usw ... nochmnal video anschauen um zu fixen Knockback?

    MovementPlayer PL;


    [SerializeField]
    private bool combatEnabled;

    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;

    [SerializeField]
    private Transform attack1HitBoxPos;

    [SerializeField]
    private LayerMask whatIsDamageable;

    private bool gotInput;
    public bool isAttacking;
    private bool isFirstAttack;

    private float lastInputTime = Mathf.NegativeInfinity; // Be ready to attack from the start of the game

    private AttackDetails attackDetails;

    private Animator anim;

    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        PS = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        PL = GetComponent<MovementPlayer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        
        //anim.SetBool("canAttack", combatEnabled);
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttack();

        if(PL.hasPlayedWakeUp)
        {
            combatEnabled = true;
        }
        else
        {
            combatEnabled= false;
        }


        
    }
    private void CheckCombatInput()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if(combatEnabled)
            {
                
                //Attempt combat
                gotInput = true;
                lastInputTime = Time.time;  
            }
        }
    }

    private void CheckAttack()
    {
        if(gotInput)
        {
            // perform Attack1
            if(!isAttacking && PS.ShowCurrentHealth() > 0f)
            {
                //pl.SetVelocityZero();
                gotInput = false;
                isAttacking = true;
                //isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                //anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if(Time.time > lastInputTime + inputTimer)
        {
            // Wait for new Input
            gotInput = false;
        }
    }


    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position,attack1Radius,whatIsDamageable);

        FindObjectOfType<AudioManager>().PlaySound("PlayerAttack");

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;

        foreach (Collider2D collider in detectedObjects)
        {
            
            collider.transform.parent.SendMessage("Damage", attackDetails);
            //Instantiate hit particles kann aucg in enemy passieren - unterschieldich pro enemy
        }
    }

    public void FinishAttack1()
    {
       
        isAttacking = false;
        //Debug.Log("EndAttackAnimation method called.");
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }


    public void Damage(AttackDetails attackDetails)
    {
        if (!PL.GetDashStatus() && !PS.isInvulnerable)
        {

            

            int direction;

            PS.DecreaseHealth(attackDetails.damageAmount);
            

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            
                //Debug.Log("lol");
                PL.Knockback(direction);
            
               
            
            
        }
            
        
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }

    
}
