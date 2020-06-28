using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    private Animator anim;
    int touchHash = Animator.StringToHash("Touch");

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            //Enter grass
            anim.SetTrigger(touchHash);
            //GameStatus.GetInstance().LoadScene("Level2");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Exit Grass
            anim.SetTrigger(touchHash);
        }
    }
}
