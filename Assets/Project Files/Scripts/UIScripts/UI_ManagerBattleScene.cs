using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManagerBattleScene : MonoBehaviour
{
    public static UI_ManagerBattleScene uiManagerBattleScene; //Always singular, might as well be static.
    [SerializeField] WaveManager waveManager; // Assign in Editor

    [SerializeField] GameObject menuUI; // Assign in Editor

    [SerializeField] Button startWaveButton; // Assign in Editor
    [SerializeField] Button menuButton; // Assign in Editor
    [SerializeField] Button quitButton; // Assign in Editor
    [SerializeField] Button restartButton; // Assign in Editor
    [SerializeField] Button resumeButton; // Assign in Editor

    [SerializeField] TMP_Text waveCounterText; // Assign in Editor
    [SerializeField] TMP_Text moneyLeftText; // Assign in Editor
    [SerializeField] TMP_Text playerNPCTotalValueText; // Assign in Editor
    [SerializeField] TMP_Text enemyNPCTotalValueText; // Assign in Editor

    int waveNumber = 0;
    [SerializeField] int moneyLeft = 0; // Assign starting money in Editor
    int playerNPCTotal = 0;
    int enemyNPCTotal = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiManagerBattleScene = this;
        moneyLeftText.text = moneyLeft.ToString();

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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_ANDROID || UNITY_STANDALONE
        Application.Quit();
#endif
    }
    void SpawnWaveAndUpdateWaveCounter()
    {
        waveManager.SpawnNumberedWave(waveNumber);
    }
    public bool CheckIfEnoughMoneyToDeployNPC(int npcValue)
    {
        if (moneyLeft - npcValue >= 0)
        {
            Debug.Log("We have enough money to deploy NPC");
            return true;
        }
        else
        {
            Debug.Log("We do not have enough money to deploy NPC");
            return false;
        }
    }
    public void UpdateMoneyLeftText(int money)
    {
        moneyLeft += money;
        moneyLeftText.text = moneyLeft.ToString();
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
    public void DeclareWaveOver()
    {
        Debug.Log("Wave is over!");
    }
    public void IncreaseWaveNumber()
    {
        waveNumber++;
        waveCounterText.text = "Wave: " + waveNumber.ToString();
    }
}
