using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int speed;
    public int jumpForce;


    Rigidbody2D body;
    float moveDirection = 0;
    bool isGrounded;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        GroundCheck();
        moveDirection = Input.GetAxis("Horizontal");
        if (isGrounded==false)
        {
            if (body.velocity.y>0)
            {
                GetComponent<Animator>().Play("JumpUp");
            }
            else
            {
                GetComponent<Animator>().Play("JumpDown");
            }
        }
        else if (moveDirection==0)
        {
            GetComponent<Animator>().Play("Idle");
        }
        else
        {
            GetComponent<Animator>().Play("Run");
        }
        if (Input.GetButtonDown("Jump") && isGrounded==true)
        {
            body.AddForce(Vector3.up*jumpForce);
        }
        
    }
    private void FixedUpdate()
    {
        body.velocity = new Vector3(moveDirection*speed, body.velocity.y,0);

        if (moveDirection>0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveDirection<0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public float rayLength;
    public Vector3 rayPosition;
    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position+rayPosition,Vector2.down,rayLength);
        Debug.DrawRay(transform.position + rayPosition, Vector3.down * rayLength, Color.red);
        if (hit.collider!=null && hit.collider.gameObject.tag=="Ground")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
