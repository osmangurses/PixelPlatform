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
        moveDirection = Input.GetAxis("Horizontal");
        if (moveDirection==0)
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
            isGrounded = false;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Ground")
        {
            isGrounded = true;
        }
    }
}
