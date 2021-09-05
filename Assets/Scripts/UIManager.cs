using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject resumeButton;
    [SerializeField]
    GameObject overPausePanel;
    [SerializeField]
    GameObject playerUI;
    [SerializeField]
    Text waveCount;

    SpawnManager spawnManager;

    bool gamePaused = false;

    public bool IsPaused
    {
        get
        {
            return gamePaused;
        }
    }

    void Awake()
    {
        Resume();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        waveCount.text = "Wave " + (spawnManager.WaveCount - 1);
    }

    public void Resume()
    {
        gamePaused = false;
        OpenUI();
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        CloseUI();
        resumeButton.SetActive(gamePaused);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Prototype 4");
    }

    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        CloseUI();
    }

    void OpenUI()
    {
        overPausePanel.SetActive(false);
        playerUI.SetActive(true);
    }

    void CloseUI()
    {
        overPausePanel.SetActive(true);
        playerUI.SetActive(false);
    }
}