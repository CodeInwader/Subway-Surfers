using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIAndScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public static int score;
    public static bool GameHasStarted = false;

    [SerializeField] GameObject deadPanel;
    [SerializeField] TextMeshProUGUI endScore;

    public GlobalLeadboard GlobalLeadboard;

    public GameObject GlobalLeadboardManager;

    private void Awake()
    {
        Time.timeScale = 0;
    }


    void Start()
    {
        GlobalLeadboardManager = GameObject.Find("GlobalLeadboardManager");
    }

   
    void FixedUpdate()
    {
        if (GameHasStarted) IncrementScore();
        

        scoreText.text = score.ToString() + "m";

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameHasStarted = false;
    }

   public void Resume()
    {
        Time.timeScale = 1;
        GameHasStarted = true;
    }

    public void Dead()
    {
        deadPanel.SetActive(true);
        endScore.text = score.ToString() + "m";
        GlobalLeadboardManager.GetComponent<GlobalLeadboard>().IsScoreBigger(score);
        Debug.Log(score);
       
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloaScene()
    {
        SceneManager.LoadScene("Main");
        score = 0;
    }

    public static void IncrementScore()
    {
        score++;
    }
}
