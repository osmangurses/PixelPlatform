using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public int speed;
    public int jumpForce;
    public bool isKeyboard;
    public AudioSource stepAudio, jumpAudio;

    Rigidbody2D body;
    float moveDirection = 0;
    bool isGrounded;
    bool isDeath;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!isDeath)
        {
            GroundCheck();
            if (moveDirection!=0 && !stepAudio.isPlaying && isGrounded)
            {
                stepAudio.Play();
            }
            if (moveDirection==0 && stepAudio.isPlaying)
            {
                stepAudio.Stop();
            }
            if (isKeyboard)
            {
                moveDirection = Input.GetAxis("Horizontal");
            }
            if (isGrounded == false)
            {
                if (body.velocity.y > 0)
                {
                    GetComponent<Animator>().Play("JumpUp");
                }
                else
                {
                    GetComponent<Animator>().Play("JumpDown");
                }
            }
            else if (moveDirection == 0)
            {
                GetComponent<Animator>().Play("Idle");
            }
            else
            {
                GetComponent<Animator>().Play("Run");
            }
            if (isKeyboard && Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        } 
    }
    public void Jump()
    {
        if (isGrounded == true)
        {
            body.AddForce(Vector3.up * jumpForce);
            jumpAudio.Play();
           
        }
    }
    public void ChangeMoveDir(int newDir)
    {
        moveDirection = newDir;
    }
    private void FixedUpdate()
    {
        if (!isDeath)
        {
            body.velocity = new Vector3(moveDirection * speed, body.velocity.y, 0);

            if (moveDirection > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (moveDirection < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isDeath = true;
            Destroy(GetComponent<Rigidbody2D>());
            GetComponent<Animator>().Play("Death");
            Invoke("ReloadScene", 1f);
        }
        if (collision.gameObject.tag=="Finish")
        {
            PlayerPrefs.SetInt("Level",SceneManager.GetActiveScene().buildIndex+1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            stepAudio.Stop();
        }
    }
}
