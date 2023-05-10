using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public HighScores m_HighScores;

    //ref to the overlay to display winning text etc
    public Text m_MainText;
    public Text m_TimerText;

    public GameObject m_HighScorePanel;
    public Text m_HighScoresText;

    public Button m_NewGameButton;
    public Button m_HighScoreButton;

    

    public GameObject[] m_Tanks;

    private float m_gameTime = 0;
    public float GameTime { get { return m_gameTime; } }

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    private void Awake()
    {
        m_GameState = GameState.Start;
    }

    private void Start()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
        m_MainText.text = "Get Ready";

        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoreButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (m_GameState)
        {
            case GameState.Start:
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    m_TimerText.gameObject.SetActive(true);
                    m_MainText.text = "";
                    m_GameState = GameState.Playing;

                    for (int i = 0; i < m_Tanks.Length; i++)
                    {
                        m_Tanks[i].SetActive(true);
                    }
                }
                break;
            case GameState.Playing:
                bool isGameOver = false;

                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}",
                                   (seconds / 60), (seconds % 60));
                if (OneTankLeft() == true)
                {
                    isGameOver = true;
                }
                else if (IsPlayerDead() == true)
                {
                    isGameOver = true;
                }
                if (isGameOver == true)
                {
                    m_GameState = GameState.GameOver;
                    m_TimerText.gameObject.SetActive(false);

                    m_NewGameButton.gameObject.SetActive(true);
                    m_HighScoreButton.gameObject.SetActive(true);

                    if (IsPlayerDead() == true)
                    {
                        m_MainText.text = "TRY AGAIN";

                    }
                    else
                    {
                        m_MainText.text = "WINNER!!";

                        //save the score
                        m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime));
                        m_HighScores.SaveScoresToFile();
                      
                        break;
                    }
                }
                break;

            case GameState.GameOver:
                break;

        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == true)
            {
                numTanksLeft++;
            }
        }

        return numTanksLeft <= 1;
    }

   public bool IsPlayerDead()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].activeSelf == false)
            {
                if (m_Tanks[i].tag == "Player")
                    return true;
            }
        }
        return false;
    }

    public void OnNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnHighScores()
    {
        m_MainText.text = "";

        m_HighScoreButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);


        string text = "";
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2} \n", (seconds / 60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }
}