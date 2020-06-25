using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Reddy : MonoBehaviour
{
    private float reddyWorldHeight = 0.25f;
    private Animator anim;
    [SerializeField] private AnimationClip deathAnim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
