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
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter grass");
            anim.SetTrigger(touchHash);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit grass");
            anim.SetTrigger(touchHash);
        }
    }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.CompareTag("Player"))
    //     {
    //         anim.SetBool("isTouched", false);
    //     }
    // }
}
