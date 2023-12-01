using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Jump : BaseAbility{
    [SerializeField] private Faction faction;
    [SerializeField] private float raycastDistance = 0.55f;
    [SerializeField] private bool isOnGround;
    [SerializeField] private bool isGliding;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isOnPeakHeight;
    [SerializeField] private bool inputReceived;
    [SerializeField] private bool firstJumpComplete = false;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float glideGravityMultiplier = 0.3f;
    [SerializeField] private float jumpTimer = 2;
    // Start is called before the first frame update
    private void Start() {
        this.faction = GetComponentInParent<Faction>();
        this.abilityName = "Jump";
        this.abilityOwner = faction.player.gameObject;
        this.abilityType = AbilityType.JUMP;
        this.cooldown = 0;
        this.damage = 0;
        this.duration = 0;
    }

    private Vector2 previousFrameVelocity = Vector2.zero;
    public override void UseAbility(bool inputReceived){
        RaycastHit2D hitBelow = Physics2D.Raycast(transform.position, -transform.up, raycastDistance, LayerMask.GetMask("ForegroundEnvironment"));
        RaycastHit2D hitAbove = Physics2D.Raycast(transform.position, transform.up, raycastDistance, LayerMask.GetMask("ForegroundEnvironment"));
        Debug.DrawLine(transform.position, transform.position-(transform.up*raycastDistance), Color.red); //Hit below visualization
        Debug.DrawLine(transform.position, transform.position+(transform.up*raycastDistance), Color.red); //Hit above visualization
        isOnGround = hitBelow.collider != null;
        isOnPeakHeight = hitAbove.collider != null;
        isGliding = !isOnGround & inputReceived;
        isFalling = !isOnGround & !inputReceived;

        /*
         * If below is for the case when the player keeps providing input
         * while the character is gliding and the moment it touches the ground
         * it jumps again without waiting. If statement below makes it so that
         * the player has to let go of the input and provide it again.
         */
        if(previousFrameVelocity.y < 0 && inputReceived && isOnGround) {
            faction.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        if(isOnGround){
            firstJumpComplete = false;
            jumpTimer = 2;
        }
        if(inputReceived && !firstJumpComplete){ //Jump
            jumpTimer = jumpTimer >= 0 ? jumpTimer*0.95f : 0;
            faction.player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce*jumpTimer));
        }
        else if((!inputReceived && !isOnGround) || isOnPeakHeight){ 
            firstJumpComplete = true;
        }
        if(isOnPeakHeight){
            firstJumpComplete = true;
        }
        
        if(firstJumpComplete && inputReceived && faction.player.GetComponent<Rigidbody2D>().velocity.y < 0){ //Glide
            faction.player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, glideGravityMultiplier * jumpForce));
        }
        previousFrameVelocity = faction.player.GetComponent<Rigidbody2D>().velocity;  
    }
}