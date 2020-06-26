using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Reddy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;

    public Transform detector;
    public LayerMask layerMask;

    private float reddyWorldHeight = 0.25f;
    private Animator anim;
    private Camera cam;
    [SerializeField] private AnimationClip deathAnim;
    private Rigidbody2D rb;

    private bool faceLeft = true; //first frame flips if due to bad coding.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {       
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            // Reddy is in camera view.
            Move();
            AnimationUpdate();
        }  
    }

    private void Move()
    {
        
        // Ground detector Raycast
        RaycastHit2D groundInfo = Physics2D.Raycast(detector.position, Vector2.down, 1f, layerMask);

        // Wall detector Raycast
        float direction = (detector.transform.position.x - transform.position.x);
        RaycastHit2D wallInfo;
        if (direction < 0)
        {
            wallInfo = Physics2D.Raycast(detector.position, Vector2.left, 0.01f, layerMask);
        }
        else
        {
            wallInfo = Physics2D.Raycast(detector.position, Vector2.right, 0.01f, layerMask);
        }

        // Hit map terrain or reached an edge so turn Reddy around!
        if (wallInfo.collider == true || groundInfo.collider == false)
        {
            transform.Rotate(0f, 180f, 0f);
            faceLeft = !faceLeft;
        }
        
        if (faceLeft)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }


    }

    private void AnimationUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1)
        {
            anim.SetFloat("hSpeed", Mathf.Abs(rb.velocity.x));
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            PlatformerCharacter2D player = other.gameObject.GetComponent<PlatformerCharacter2D>();

            // Player lands on top of Reddy.
            if(player.state == State.falling && other.gameObject.transform.position.y > transform.position.y + reddyWorldHeight)
            {
                other.gameObject.GetComponent<PlatformerCharacter2D>().Jump();
                anim.Play(deathAnim.name);
                Destroy(gameObject, deathAnim.length);
            }
            else
            {
                player.Damage(gameObject);
            }
        }
    }
}
