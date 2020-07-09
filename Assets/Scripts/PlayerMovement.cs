using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed = 3.0f;
    public bool canMove;
    Vector2 movement;
    public Animator LegAnimator;

    public Animator HandAnimator;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Awake(){
        canMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
    }

    void FixedUpdate(){
        if(canMove){
            if(movement.x > 0){
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if(movement.x < 0){
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if(Input.GetKey(KeyCode.LeftShift))
                rb2d.velocity = movement * speed * 1.5f;
            else
                rb2d.velocity = movement * speed;
        }
        else{
            //Doing action, stop character movement
            rb2d.velocity = Vector2.zero;
        }
        if(rb2d.velocity.x != 0 || rb2d.velocity.y != 0){
            LegAnimator.SetBool("Running", true);
            HandAnimator.SetBool("Running", true);
        }
        else{
            LegAnimator.SetBool("Running", false);
            HandAnimator.SetBool("Running", false);
        }
        
    }
    
}
