using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject playerDeploymentZone;
    [SerializeField] GameObject enemyDeploymentZone;

    [SerializeField] float spawnInterval; // Assign in Editor
    int currentWaveNumber;

    public static WaveManager waveManager;
    [SerializeField] bool isWaveOnGoing;
    [SerializeField] GameObject[] enemiesLeft = new GameObject[0];
    [SerializeField] List<GameObject> playerNPCsLeft = new List<GameObject>();
    [SerializeField] GameObject enemyMeleeNPC; // Assign in Editor
    [SerializeField] GameObject enemyRangedNPC; // Assign in Editor
    [SerializeField] GameObject enemyCasterNPC; // Assign in Editor

    [SerializeField] GameObject[] spawnPointsInOrderOfShortestDistance; // Assign in Editor

    void Start()
    {
        waveManager = this; // Always remember to declare static variables!
    }
    void Update()
    {
        playerNPCsLeft = playerNPCsLeft.Where(item => item != null).ToList(); // Removing null transform from playerNPCsLeft list.

        StartCoroutine(CheckIfWaveOver());
        CheckForWinOrLoseCondition();

        if (isWaveOnGoing) UI_ManagerBattleScene.uiManagerBattleScene.MoveBottomUI_GraduallyDown();
        else UI_ManagerBattleScene.uiManagerBattleScene.MoveBottomUI_GraduallyUp();
    }
    public void SpawnNumberedWave(int waveNumber)
    {
        if (isWaveOnGoing == true) return; // If a wave is already ongoing, don't start a new one.
        isWaveOnGoing = true;
        currentWaveNumber = waveNumber;
        UI_ManagerBattleScene.uiManagerBattleScene.IncreaseWaveNumber();
        Coroutine spawningCoroutine;

        if (waveNumber == 0)
        {
            spawningCoroutine = StartCoroutine(SpawnWave(3, 2, 0));
            UI_ManagerBattleScene.uiManagerBattleScene.DisableHowToPlayElements();
        }
        else if (waveNumber == 1) spawningCoroutine = StartCoroutine(SpawnWave(5, 3, 0));
        else if (waveNumber == 2) spawningCoroutine = StartCoroutine(SpawnWave(8, 2, 0));
        else if (waveNumber == 3) spawningCoroutine = StartCoroutine(SpawnWave(8, 5, 0));
        else if (waveNumber == 4) // Changing BGM to BattleBGM and spawning wave.
        {
            spawningCoroutine = StartCoroutine(SpawnWave(10, 3, 2));
            BattleSceneAudioManager.battleSceneAudioManager.ChangeToBattleBGM();
        }
        else if (waveNumber == 5) spawningCoroutine = StartCoroutine(SpawnWave(10, 7, 3));
        else if (waveNumber == 6) spawningCoroutine = StartCoroutine(SpawnWave(12, 10, 3));
        else if (waveNumber == 7) spawningCoroutine = StartCoroutine(SpawnWave(10, 10, 5));
        else if (waveNumber == 8) spawningCoroutine = StartCoroutine(SpawnWave(15, 5, 5));
        else if (waveNumber == 9) spawningCoroutine = StartCoroutine(SpawnWave(15, 5, 5));
    }

    IEnumerator SpawnWave(int meleeNPC_Amount, int rangedNPC_Amount, int casterAmount_NPC)
    {
        Debug.Log("Wave has started!");
        playerDeploymentZone.SetActive(false);
        enemyDeploymentZone.SetActive(false);

        enemiesLeft = new GameObject[meleeNPC_Amount + rangedNPC_Amount + casterAmount_NPC]; // Create new array for the amount of enemies in the wave.

        int spawnPosition = 0;
        // Spawning enemies, adding them into enemiesLeft array, and setting their tag to "Red" to identify them as enemies.
        for (int i = 0; i < meleeNPC_Amount; i++)
        {
            GameObject enemy = Instantiate(enemyMeleeNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            OrganizeInLineFormation organizeLineFormation = enemy.AddComponent<OrganizeInLineFormation>();
            enemy.tag = "Red";
            enemy.transform.position = new Vector3(enemy.transform.position.x + 9, enemy.transform.position.y, enemy.transform.position.z);
            enemy.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.SuspendStateMachine);
            organizeLineFormation.TakePlaceInLineFormation(spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position); 
            enemiesLeft[i] = enemy;
            spawnPosition++;
            yield return new WaitForSeconds(spawnInterval);
        }
        for (int j = 0; j < rangedNPC_Amount; j++)
        {
            GameObject enemy = Instantiate(enemyRangedNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            OrganizeInLineFormation organizeLineFormation = enemy.AddComponent<OrganizeInLineFormation>();
            enemy.tag = "Red";
            enemy.transform.position = new Vector3(enemy.transform.position.x + 9, enemy.transform.position.y, enemy.transform.position.z);
            enemy.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.SuspendStateMachine);
            organizeLineFormation.TakePlaceInLineFormation(spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position);
            enemiesLeft[j + meleeNPC_Amount] = enemy;
            spawnPosition++;
            yield return new WaitForSeconds(spawnInterval);
        }
        for (int k = 0; k < casterAmount_NPC; k++)
        {
            GameObject enemy = Instantiate(enemyCasterNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            OrganizeInLineFormation organizeLineFormation = enemy.AddComponent<OrganizeInLineFormation>();
            enemy.tag = "Red";
            enemy.transform.position = new Vector3(enemy.transform.position.x + 9, enemy.transform.position.y, enemy.transform.position.z);
            enemy.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.SuspendStateMachine);
            organizeLineFormation.TakePlaceInLineFormation(spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position);
            enemiesLeft[k + meleeNPC_Amount + rangedNPC_Amount] = enemy;
            spawnPosition++;
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(3);

        foreach (GameObject enemy in enemiesLeft)
        {
            enemy.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.Idle);
        }
        foreach (GameObject playerNPC in playerNPCsLeft)
        {
            playerNPC.GetComponent<NPC2D>().SetState(NPC2D.CurrentState.Idle);
        }
        Debug.Log("GameObjects have become hostile!");
    }
    IEnumerator CheckIfWaveOver()
    {
        if (enemiesLeft.All(enemy => enemy == null) && isWaveOnGoing == true)
        {
            int amountOfMoneyGained = 0;
            if (currentWaveNumber >= 0 || currentWaveNumber <= 3) { amountOfMoneyGained = 1000; }
            else if (currentWaveNumber >= 4 || currentWaveNumber <= 7) { amountOfMoneyGained = 1500; }
            else if (currentWaveNumber >= 8 || currentWaveNumber <= 9) { amountOfMoneyGained = 2000; }

            isWaveOnGoing = false;
            UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(amountOfMoneyGained);
            UI_ManagerBattleScene.uiManagerBattleScene.DeclareWaveOver();
            playerDeploymentZone.SetActive(true);
            enemyDeploymentZone.SetActive(true);

            yield return new WaitForSeconds(1f);
            foreach (GameObject playerNPC in playerNPCsLeft)
            {
                NPC2D playerNPC_Script = playerNPC.GetComponent<NPC2D>();
                playerNPC_Script.SetState(NPC2D.CurrentState.SuspendStateMachine);
                playerNPC_Script.HealCharacterToFullHealth();
            }
        }
    }
    public bool CheckForWaveStatus()
    {
        return isWaveOnGoing;
    }
    public void AddPlayerToList(GameObject playerNPC)
    {
        playerNPCsLeft.Add(playerNPC);
    }

    void CheckForWinOrLoseCondition()
    {
        if (!isWaveOnGoing && currentWaveNumber > 9)
        {
            UI_ManagerBattleScene.uiManagerBattleScene.PlayerWins();
        }
        else if (isWaveOnGoing && playerNPCsLeft.Count == 0)
        {
            UI_ManagerBattleScene.uiManagerBattleScene.PlayerLoses();
        }
    }
}
