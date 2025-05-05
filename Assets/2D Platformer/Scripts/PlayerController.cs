using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false, door;

        public bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        private bool isClimbing = false;
        
    public float climbSpeed = 3f;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        private float inputVertical;
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded&&!isClimbing)
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!b&&chest)
                {
                    b = true;
                    a = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<Rigidbody2D>().gravityScale = 0;
                    GetComponent<SpriteRenderer>().enabled = false;

                }
                else if(b)
                {
                    a = true;
                    b = false;

                    GetComponent<BoxCollider2D>().enabled = true;
                    GetComponent<Rigidbody2D>().gravityScale = 1;

                    GetComponent<SpriteRenderer>().enabled = true;
                }

                
            }
        }
       public bool a = true, b;
        void Update()
        {
            if (a)
            {
                if (Input.GetButton("Horizontal"))
                {
                    moveInput = Input.GetAxis("Horizontal");
                    Vector3 direction = transform.right * moveInput;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                    animator.SetInteger("playerState", 1); // Turn on run animation
                }
                else
                {
                    // Turn on idle animation
                }

                // Turn on jump animation

                if (facingRight == false && moveInput > 0)
                {
                    Flip();
                }
                else if (facingRight == true && moveInput < 0)
                {
                    Flip();
                }
                if (isClimbing)
                {
                    inputVertical = Input.GetAxisRaw("Vertical");
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, inputVertical * climbSpeed);

                }
                else
                {

                }
            }
        }
        public void Jump()
        {

            isGrounded = false;
            Debug.Log("a");
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
            if (other.gameObject.tag == "door")
            {
                door = true;
            }
            if (other.gameObject.tag == "chest")
            {
                chest = true;
            }

            if (other.CompareTag("ladder"))
                {
                    isClimbing = true;
                }
            
        }
        public bool chest;
        private void OnTriggerExit2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "door")
            {
                door = false;
            }
            if (collision.gameObject.tag == "chest")
            {
                chest = false;
            }
            if (collision.CompareTag("ladder"))
            {
                isClimbing = false;
            }
        }
    }
}
