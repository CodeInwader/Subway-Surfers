using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIAndScoreManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] TextMeshProUGUI scoreText;
    public static int score;
    public static bool gameHasStarted = false;

    [SerializeField] GameObject deadPanel;
    [SerializeField] TextMeshPro endScore;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameHasStarted) Score();
        

        scoreText.text = score.ToString() + "m";

    }

    void Score()
    {
        score++;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameHasStarted = false;
    }

   public void Resume()
    {
        Time.timeScale = 1;
        gameHasStarted = true;
    }

    public void Dead()
    {
        deadPanel.SetActive(true);

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloaScene()
    {
        SceneManager.LoadScene("Main");
    }
}
