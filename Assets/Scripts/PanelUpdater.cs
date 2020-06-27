using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUpdater : MonoBehaviour
{
    void Update()
    {
        // GameObject go = GameObject.Find("GameStatus");

        // if (go == null)
        // {
        //     Debug.LogError("Failed to find object named GameStatus");
        //     this.enabled = false;
        //     return;
        // }

        // GameStatus gs = go.GetComponent<GameStatus>();

        GetComponent<Text>().text = "Lives: " + GameStatus.GetInstance().GetLives();

    }
}
