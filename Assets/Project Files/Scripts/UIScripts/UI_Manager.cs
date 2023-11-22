using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager uiManager; //Always singular, might as well be static.
    [SerializeField] WaveManager waveManager; // Assign in Editor

    [SerializeField] GameObject menuUI; // Assign in Editor

    [SerializeField] Button startWaveButton; // Assign in Editor
    [SerializeField] Button menuButton; // Assign in Editor
    [SerializeField] Button quitButton; // Assign in Editor
    [SerializeField] Button restartButton; // Assign in Editor
    [SerializeField] Button resumeButton; // Assign in Editor

    [SerializeField] TMP_Text waveCounterText; // Assign in Editor;
    [SerializeField] TMP_Text moneyLeftText; // Assign in Editor;
    [SerializeField] TMP_Text playerNPCTotalValueText; // Assign in Editor;
    [SerializeField] TMP_Text enemyNPCTotalValueText; // Assign in Editor;

    int waveNumber = 0;
    int moneyLeft = 0;
    int playerNPCTotal = 0;
    int enemyNPCTotal = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = this;
        waveCounterText.text = "Wave: " + waveNumber.ToString();
        startWaveButton.onClick.AddListener(() => { SpawnWaveAndUpdateWaveCounter(); });
        menuButton.onClick.AddListener(() => { OpenMenuAndPauseGame(); });
        resumeButton.onClick.AddListener(() => { CloseMenuAndPauseGame(); });
        restartButton.onClick.AddListener(() => { RestartGame(); });
        quitButton.onClick.AddListener(() => { QuitGame(); });
    }
    void OpenMenuAndPauseGame()
    {
        menuUI.SetActive(true); 
        Time.timeScale = 0f;
    }
    void CloseMenuAndPauseGame()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void QuitGame()
    {
        Application.Quit();
    }
    void SpawnWaveAndUpdateWaveCounter()
    {
        waveManager.SpawnNumberedWave(waveNumber);
        waveNumber++;
        waveCounterText.text = "Wave: " + waveNumber.ToString();
    }
    public void UpdateMoneyLeftText(int money)
    {
        moneyLeft += money;
        moneyLeftText.text = "Money Left: " + moneyLeft.ToString();
    }
    public void UpdatePlayerNPCTotalValueText(int playerNPCValue)
    {
        playerNPCTotal += playerNPCValue;
        playerNPCTotalValueText.text = playerNPCTotal.ToString();
    }
    public void UpdateEnemyNPCTotalValueText(int enemyNPCValue)
    {
        enemyNPCTotal += enemyNPCValue;
        enemyNPCTotalValueText.text = enemyNPCTotal.ToString();
    }
}
