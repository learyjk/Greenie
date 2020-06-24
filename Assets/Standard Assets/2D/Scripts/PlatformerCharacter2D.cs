using System;
using UnityEngine;

public enum State {idle, walking, jumping, falling, hurt}

#pragma warning disable 649
namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private bool spawnDust;
        public GameObject DustParticles;
        private Animator camAnim;
        private AudioSource source;
        public AudioClip landingSound;
        public AudioClip jumpingSound;
        public State state = State.idle;
        
        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            camAnim = Camera.main.GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            source = GetComponent<AudioSource>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetInteger("state", (int)state);
            velocityState();
            
        }

        private void Update() {
            if (m_Grounded)
            {
                if (spawnDust)
                {
                    // Player lands on the ground.
                    var pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                    source.pitch = pitch;
                    source.clip = landingSound;
                    source.Play();
                    camAnim.SetTrigger("shake");
                    Instantiate(DustParticles, m_GroundCheck.position, Quaternion.identity);
                    spawnDust = false;
                }
            }
            else // Player leaves ground
            {
                spawnDust = true;
                // source.clip = jumpingSound;
                // source.pitch = 1f;
                // source.Play();
            }
            
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            // if (!crouch && m_Anim.GetBool("Crouch"))
            // {
            //     // If the character has a ceiling preventing them from standing up, keep them crouching
            //     if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            //     {
            //         crouch = true;
            //     }
            // }

            // Set whether or not the character is crouching in the animator
            //m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                Jump();
            }
        }

        public void Jump()
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            state = State.jumping;
        }

        public void Damage(GameObject other)
        {
            state = State.hurt;
            Debug.Log("Hurt greenie!");
            if (transform.position.x < other.transform.position.x)
            {
                //Reddy is to the right so damage Greenie left
                Input.ResetInputAxes();
                m_Rigidbody2D.AddForce(new Vector2(-2000.0f, m_Rigidbody2D.velocity.y));
                //m_Rigidbody2D.velocity = new Vector2(-1f, m_Rigidbody2D.velocity.y);
            }
            else
            {
                //Reddy is to the left so damage Greenie right
                Input.ResetInputAxes();
                m_Rigidbody2D.AddForce(new Vector2(2000.0f, m_Rigidbody2D.velocity.y));
                //m_Rigidbody2D.velocity = new Vector2(1f, m_Rigidbody2D.velocity.y);
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            transform.Rotate(0f, 180f, 0f);
        }

        private void velocityState()
        {
            if (m_Rigidbody2D.velocity.y > 0.1f)
            {
                state = State.jumping;
            }
            else if (m_Rigidbody2D.velocity.y < -0.1f)
            {
                state = State.falling;
            }
            else if (state == State.falling)
            {
                if (m_Grounded)
                {
                    state = State.idle;
                }
            }
            else if (state == State.hurt)
            {
                // velocity is near zero.
                if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 0.1f)
                {
                    state = State.idle;
                }
            }
            else if (Mathf.Abs(m_Rigidbody2D.velocity.x) > Mathf.Epsilon)
            {
                //Moving
                state = State.walking;
            }
            else
            {
                state = State.idle;
            }

        }
    }
}
