using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursorController : MonoBehaviour
{

    public Texture2D[] cursorTexture = new Texture2D[3];

    public MovementPlayer movementPlayer;
    public PlayerCombatTry playerCombat;
    
    //public TrailRenderer trailRenderer;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(cursorTexture[0], new Vector2(cursorTexture[0].width / 2f, cursorTexture[0].height / 2f), CursorMode.Auto);

        movementPlayer = GameObject.Find("Player").GetComponent<MovementPlayer>();
        playerCombat = GameObject.Find("Player").GetComponent<PlayerCombatTry>();
        //trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        //transform.position = movementPlayer.GetMousePosition();
       
        if(movementPlayer != null && playerCombat != null)
        {
            //trailRenderer.emitting = true;  
            if (playerCombat.isAttacking)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.SetCursor(cursorTexture[2], new Vector2(cursorTexture[2].width / 2f, cursorTexture[2].height / 2f), CursorMode.Auto);
            }
            else
            if (movementPlayer.couldGrappleHit)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.SetCursor(cursorTexture[0], new Vector2(cursorTexture[0].width / 2f, cursorTexture[0].height / 2f), CursorMode.Auto);
                //Debug.Log(movementPlayer.IsInGrappleRange());
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.SetCursor(cursorTexture[1], new Vector2(cursorTexture[1].width / 2f, cursorTexture[1].height / 2f), CursorMode.Auto);
                //Debug.Log(movementPlayer.IsInGrappleRange());
            }

        }

    }

}

