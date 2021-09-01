using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim; 
    private Collider2D coll;

    public static PlayerController player;

    //                   0       1        2        3       4     5      6
    private enum State {idle, running, jumping, falling, hurt, climb, death};
    private State state = State.idle;

    
    public bool DoubleJumpAllow = false;
    private bool invincible = false;

    //Ladder variables
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;

    public ladder ladder;
    private float naturalGravity;
    [SerializeField] float climbSpeed = 3f;
    
    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource gemSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource DeathSound;
    
    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        naturalGravity = rb.gravityScale;
    }

    private void Update()
    {
        if(state == State.climb){
            Climb();
        }
        else if(state != State.hurt){
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state); //sets animation based on Enumerator state
    }

    private void OnTriggerEnter2D(Collider2D collision){
        //if player takes cherry
        if(collision.tag == "Collectable"){
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
        }
        //if player takes gem
        if(collision.tag == "Gem"){
            Destroy(collision.gameObject);
            gemSound.Play();
            PermanentUI.perm.gem += 1;
            PermanentUI.perm.gemText.text = PermanentUI.perm.gem.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        //if player is already touch the enemy
        if(!invincible){
            //if player touches the enemy
            if(other.gameObject.tag == "Enemy"){

                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                //if player is in state falling and touch the enemy, the enemy dies
                if(state == State.falling){
                    enemy.JumpedOn();
                    Jump();
                } else {
                    //if player is not in state falling, the player takes damage
                    HandleHealth(); // Deals with health, updating ui, and will reset level if health is <= 0
                    Invoke("resetInvulnerability", 0.3f);
                    if(other.gameObject.transform.position.x > transform.position.x){
                        //Enemy is to my right therefore I should be damaged and move left
                        rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    } else {
                        //Enemy is to my left therefore I should be damaged and move right
                        rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    }
                }
            }
        }
    }

    void resetInvulnerability(){
        invincible = false;
    }

    public void DeathState(){
        state = State.death;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f; 
        rb.gravityScale = 0f;
    }

    public void AliveState(){
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = naturalGravity;
    }

    private void HandleHealth(){
        if(PermanentUI.perm.health > 1){
            state = State.hurt;
            hurtSound.Play();
            invincible = true;
            PermanentUI.perm.health -= 1;
            PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        }
        else if(PermanentUI.perm.health <= 1){
            DeathSound.Play();
            PermanentUI.perm.health -= 1;
            PermanentUI.perm.healthAmount.text = "0";
            DeathState();
            invincible = true;
            rb.bodyType = RigidbodyType2D.Static;
            PermanentUI.perm.lives -= 1;
            if(PermanentUI.perm.lives >= 1){
                PermanentUI.perm.Invoke("Reset", 1f);
                Invoke("Restart", 1f);
                resetInvulnerability();
            } else {
                //Game Over
                PermanentUI.perm.Reset();
                PermanentUI.perm.gameOverUI.SetActive(true);
                }
        }
    }

    public void Restart(){
        if(PermanentUI.perm.lives >= 1){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            AliveState();
        } else {
           //Game Over
            PermanentUI.perm.gameOverUI.SetActive(true);
        }
    }

    private void Movement(){
        if(state != State.death){
            float hDirection = Input.GetAxis("Horizontal");

            // Unlock doublejump
            if( (state == State.running || state == State.idle) && rb.velocity.y == 0 ){
                DoubleJumpAllow = true;
            }

            rb.velocity = new Vector2(speed * hDirection,rb.velocity.y);

            //Moving Left
            if(hDirection < 0){
                transform.localScale = new Vector2(-1, 1); 
            } 
            //Moving Right
            else if(hDirection > 0 ){
                transform.localScale = new Vector2(1, 1);
            } 
            //Jumping and DoubleJump
            if( Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground) && rb.velocity.y == 0) {
                if( (state == State.running || state == State.idle) && rb.velocity.y == 0 ){
                    Jump();
                }    
            }
            else if(Input.GetButtonDown("Jump") && DoubleJumpAllow == true){
                Jump();
                DoubleJumpAllow = false;
            }

            if(canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f){
                state = State.climb;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | 
                        RigidbodyConstraints2D.FreezeRotation;
                transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
                rb.gravityScale = 0;
            }
        }
    }

    private void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
        jumpSound.Play();
    }

    private void AnimationState(){

        if(state != State.death){
            if(state == State.climb){

            }
            else if (state == State.jumping){
                if(rb.velocity.y < .1f){
                    state = State.falling;
                }
            }
            else if(state == State.falling){
                if(coll.IsTouchingLayers(ground)){
                    state = State.idle;
                } 
            }
            else if(!coll.IsTouchingLayers(ground) && rb.velocity.y < .1f && state != State.hurt){
                    state = State.falling;
                }
            else if(state == State.hurt){
                if(Mathf.Abs(rb.velocity.x) < .1f){
                    state = State.idle;
                }
            }
            else if(Mathf.Abs(rb.velocity.x) > 2f){
                //Moving
                state = State.running;
            } 
            else {
                state = State.idle;
            }
        }
    }

    private void Footstep(){
        footstep.Play();
    }

    private void Climb(){
 
            if(Input.GetButtonDown("Jump")){
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                canClimb = false;
                rb.gravityScale = naturalGravity;
                anim.speed = 1f;
                Jump();
                DoubleJumpAllow = true;
                return;
            }
            float vDirection = Input.GetAxis("Vertical");
            //Climbing up
            if(vDirection > .1f && !topLadder){
                rb.velocity = new Vector2(0f, vDirection * climbSpeed);
                anim.speed = 1f;
            }
            //Climbing down
            else if(vDirection < -.1f && !bottomLadder){
                rb.velocity = new Vector2(0f, vDirection * climbSpeed);
                anim.speed = 1f;
            }
            //Still
            else {
                anim.speed = 0;
                rb.velocity = Vector2.zero;
            }

    }

}
