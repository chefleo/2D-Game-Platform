using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLenght = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    
    

    private bool facingLeft = true;

    protected override void Start(){

        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private void Update(){
        //Transition from Jump to fall
        if(anim.GetBool("jumping")){
            if(rb.velocity.y < .1){
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);
            }
        }

        //Transition from Fall to idle
        if(coll.IsTouchingLayers(ground) && anim.GetBool("falling")){
            anim.SetBool("falling", false);

        }
    }

    private void Move(){
        if(facingLeft){
            //Test to see if we are beyond the leftCap
            if(transform.position.x > leftCap){

                //Make sure sprite is facing right position, and if it is not, then face the right direction
                if(transform.localScale.x != 1){
                    transform.localScale = new Vector3(1, 1);
                }

                //Test to see if i am on ground, if so jump
                if(coll.IsTouchingLayers(ground)){
                    //Jump
                    rb.velocity = new Vector2(-jumpLenght, jumpHeight);
                    anim.SetBool("jumping", true);
                }

            } else {
                facingLeft = false;
            }
        } else {
            if(transform.position.x < rightCap){

                //Make sure sprite is facing right position, and if it is not, then face the right direction
                if(transform.localScale.x != -1){
                    transform.localScale = new Vector3(-1, 1);
                }

                //Test to see if i am on ground, if so jump
                if(coll.IsTouchingLayers(ground)){
                    //Jump
                    rb.velocity = new Vector2(jumpLenght, jumpHeight);
                    anim.SetBool("jumping", true);
                }

            } else {
                facingLeft = true;
            }
        }
    }

    
}
