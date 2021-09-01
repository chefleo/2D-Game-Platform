using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opposum : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float speed = 3f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    
    

    private bool facingLeft = true;

    protected override void Start(){

        base.Start();
        coll = GetComponent<Collider2D>();
    }

    private void Update(){
        Move();
        anim.SetBool("running", true);
    }

    private void Move(){
        if(facingLeft){
            //Test to see if we are beyond the leftCap
            if(transform.position.x > leftCap){

                //Make sure sprite is facing right position, and if it is not, then face the right direction
                if(transform.localScale.x != 1){
                    transform.localScale = new Vector3(1, 1);
                }
                if(coll.IsTouchingLayers(ground)){
                    //Run
                    rb.velocity = new Vector2(-speed, 0);
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
                    if(coll.IsTouchingLayers(ground)){
                    rb.velocity = new Vector2(speed, 0);
                    }
                } else {
                facingLeft = true;
                }
        }
    }
}
