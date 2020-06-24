using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Reddy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            PlatformerCharacter2D player = other.gameObject.GetComponent<PlatformerCharacter2D>();

            if(player.state == State.falling)
            {
                other.gameObject.GetComponent<PlatformerCharacter2D>().Jump();

                //TODO animate the death 
                
                Destroy(gameObject);
            }
            else
            {
                player.Damage(gameObject);
            }
        }
    }
}
