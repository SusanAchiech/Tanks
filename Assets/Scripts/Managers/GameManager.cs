using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] m_PlayerTanks; // Array of player tanks
    public GameObject[] m_EnemyTanks;  // Array of enemy tanks

    private float m_gameTime = 0;
    public float GameTime { get { return m_gameTime; } }

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    public HighScores m_HighScores;
    // Reference to the overlay Text to display winning text, etc
    public Text m_MessageText;
    public Text m_TimerText;

    public GameObject m_HighScorePanel;
    public Text m_HighScoresText;

    public Button m_NewGameButton;
    public Button m_HighScoresButton;

    private GameState m_GameState;
    public GameState State { get { return m_GameState; } }

    private void Awake()
    {
        m_GameState = GameState.Start;
    }

    private void Start()
    {
        // Deactivate player tanks
        for (int i = 0; i < m_PlayerTanks.Length; i++)
        {
            m_PlayerTanks[i].SetActive(false);
        }

        m_TimerText.gameObject.SetActive(false);
        m_MessageText.text = "Get Ready";

        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);

        // Set the initial game state
        //m_GameState = GameState.Start;
    }

    void Update()
    {
        switch (m_GameState)
        {
            case GameState.Start:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_TimerText.gameObject.SetActive(true);
                    m_MessageText.text = "";
                    m_GameState = GameState.Playing;

                    // Activate player tanks
                    for (int i = 0; i < m_PlayerTanks.Length; i++)
                    {
                        m_PlayerTanks[i].SetActive(true);
                    }

                    // Activate enemy tanks
                    for (int i = 0; i < m_EnemyTanks.Length; i++)
                    {
                        m_EnemyTanks[i].SetActive(true);
                    }
                }
                break;
            case GameState.Playing:
                bool isGameOver = false;

                m_TimerText.gameObject.SetActive(true);

                m_gameTime += Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_gameTime);
                m_TimerText.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));


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
                    m_HighScoresButton.gameObject.SetActive(true);

                    if (IsPlayerDead() == true)
                    {
                        m_MessageText.text = "TRY AGAIN";
                    }
                    else
                    {
                        m_MessageText.text = "WINNER!";
                        // save the score
                        m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime));
                        m_HighScores.SaveScoresToFile();
                    }
                }
                break;
            case GameState.GameOver:
                if (Input.GetKeyUp(KeyCode.Return) == true)
                {
                    m_gameTime = 0;
                    m_GameState = GameState.Playing;
                    m_MessageText.text = "";
                    m_TimerText.gameObject.SetActive(true);

                    // Activate player tanks
                    for (int i = 0; i < m_PlayerTanks.Length; i++)
                    {
                        m_PlayerTanks[i].SetActive(true);
                    }

                    // Activate enemy tanks
                    for (int i = 0; i < m_EnemyTanks.Length; i++)
                    {
                        m_EnemyTanks[i].SetActive(true);
                    }
                }
                break;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private bool OneTankLeft()
    {
        int numPlayerTanksLeft = 0;

        for (int i = 0; i < m_PlayerTanks.Length; i++)
        {
            if (m_PlayerTanks[i].activeSelf == true)
            {
                numPlayerTanksLeft++;
            }
        }

        int numEnemyTanksLeft = 0;
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            if (m_EnemyTanks[i].activeSelf == true)
            {
                numEnemyTanksLeft++;
            }
        }

        return numPlayerTanksLeft <= 1 || numEnemyTanksLeft <= 1;
    }

    private bool IsPlayerDead()
    {
        for (int i = 0; i < m_PlayerTanks.Length; i++)
        {
            if (m_PlayerTanks[i].activeSelf == false)
            {
                if (m_PlayerTanks[i].tag == "Player")
                return true;
            }
        }
        return false;
    }


    public void OnNewGame()
    {
        m_HighScorePanel.SetActive(false);
        m_gameTime = 0;
        m_GameState = GameState.Playing;
        m_TimerText.gameObject.SetActive(true);
        m_MessageText.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);

        // Activate player tanks
        for (int i = 0; i < m_PlayerTanks.Length; i++)
        {
            m_PlayerTanks[i].SetActive(true);
        }

        // Activate enemy tanks
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            m_EnemyTanks[i].SetActive(true);
        }
    }

    public void OnHighScores()
    {
        m_MessageText.text = "";
        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);
        string text = "";
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n", (seconds / 60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }
}
