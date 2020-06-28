using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextLevelDoor : MonoBehaviour
{
    public string goToLevel;

    private void OnTriggerEnter2D(Collider2D other) {
        GameStatus.GetInstance().LoadScene(goToLevel);
    }
}
