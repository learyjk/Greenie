using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPos, endPos;
    private Vector3 nextPos;

    [SerializeField] private float speed = 5.0f;

    void Start()
    {
        transform.position = startPos.position;
        nextPos = endPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed*Time.deltaTime);

        if (transform.position == endPos.position)
        {
            nextPos = startPos.position;
        }
        if (transform.position == startPos.position)
        {
            nextPos = endPos.position;
        }

    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.CompareTag("Player"))
        {
            other.collider.transform.SetParent(transform); 
        }
          
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.collider.CompareTag("Player"))
        {
            other.collider.transform.SetParent(null); 
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(startPos.position, endPos.position);
    }
}
