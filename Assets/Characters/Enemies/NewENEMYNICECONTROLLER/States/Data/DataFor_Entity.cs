using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class DataFor_Entity : ScriptableObject
{

    public float maxHealth = 70;

    public float damageHopSpeed = 10f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.75f;
    public float groundCheckRadius = 0.3f;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 6f;

    public float playerBehindCheckDistance = 3f;

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    public float closeRangeActionDistance = 1f;

    


    public GameObject hitParticle;













    public LayerMask whatisGround;
    public LayerMask whatIsPlayer;
}
