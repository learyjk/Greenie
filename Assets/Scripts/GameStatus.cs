using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    [SerializeField] private int lives = 3;
    static GameStatus instance;
    public static GameStatus GetInstance() { return instance; }

    // Start is called before the first frame update
    void Start()
    {
        //Singleton - There should only ever be ONE GameStatus!
        if (instance != null)
        {
            //instance already set!
            Destroy(this.gameObject);
            return; 
        }

        instance = this;
        GameStatus.DontDestroyOnLoad(this.gameObject);


    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetLives()
    {
        return lives;
    }

    public void ResetLevel() {
        lives -= 1;
        if (lives == 0)
        {
            LoadScene("GameOver");
            lives = 3;
        }
        else
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }
        
        
    }

    private void OnDestroy() {
        Debug.Log("GameStatus was destroyed!");
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Load it up!");
        SceneManager.LoadScene(sceneName);
    }
}
