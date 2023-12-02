using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager combatManager;
    [SerializeField] bool isWaveOnGoing;
    [SerializeField] GameObject[] enemiesLeft = new GameObject[0];
    [SerializeField] GameObject enemyMeleeNPC; // Assign in Editor
    [SerializeField] GameObject enemyRangedNPC; // Assign in Editor
    [SerializeField] GameObject enemyCasterNPC; // Assign in Editor

    [SerializeField] GameObject[] spawnPointsInOrderOfShortestDistance; // Assign in Editor

    void Start()
    {
        combatManager = this; // Always remember to declare static variables!
    }
    void Update()
    {
        CheckIfWaveOver();
    }
    public void SpawnNumberedWave(int waveNumber)
    {
        if (waveNumber == 0) { SpawnWave(3, 2, 0); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1000); }
        else if (waveNumber == 1) { SpawnWave(5, 3, 0); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1000); }
        else if (waveNumber == 2) { SpawnWave(8, 2, 0); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1000); }
        else if (waveNumber == 3) { SpawnWave(8, 5, 0); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1000); }
        else if (waveNumber == 4) { SpawnWave(10, 3, 2); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1500); }
        else if (waveNumber == 5) { SpawnWave(10, 7, 3); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1500); }
        else if (waveNumber == 6) { SpawnWave(12, 10, 3); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1500); }
        else if (waveNumber == 7) { SpawnWave(10, 10, 5); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(1500); }
        else if (waveNumber == 8) { SpawnWave(15, 5, 5); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(2000); }
        else if (waveNumber == 9) { SpawnWave(15, 5, 5); UI_ManagerBattleScene.uiManagerBattleScene.UpdateMoneyLeftText(2000); }
    }

    void SpawnWave(int meleeNPC_Amount, int rangedNPC_Amount, int casterAmount_NPC)
    {
        isWaveOnGoing = true;
        Debug.Log("Wave has started!");

        enemiesLeft = new GameObject[meleeNPC_Amount + rangedNPC_Amount + casterAmount_NPC]; // Create new array for the amount of enemies in the wave.

        int spawnPosition = 0;
        // Spawning enemies, adding them into enemiesLeft array, and setting their tag to "Red" to identify them as enemies.
        for (int i = 0; i < meleeNPC_Amount; i++)
        {
            GameObject enemy = Instantiate(enemyMeleeNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            enemiesLeft[i] = enemy;
            enemy.tag = "Red";
            spawnPosition++;
        }
        for (int j = 0; j < rangedNPC_Amount; j++)
        {
            GameObject enemy = Instantiate(enemyRangedNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            enemiesLeft[j + meleeNPC_Amount] = enemy;
            enemy.tag = "Red";
            spawnPosition++;
        }
        for (int k = 0; k < casterAmount_NPC; k++)
        {
            GameObject enemy = Instantiate(enemyCasterNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            enemiesLeft[k + meleeNPC_Amount + rangedNPC_Amount] = enemy;
            enemy.tag = "Red";
            spawnPosition++;
        }
    }
    void CheckIfWaveOver()
    {
        if (enemiesLeft.All(enemy => enemy == null) && isWaveOnGoing == true)
        {
            isWaveOnGoing = false;
            UI_ManagerBattleScene.uiManagerBattleScene.DeclareWaveOver();
        }
    }
    public bool CheckForWaveStatus()
    {
        return isWaveOnGoing;
    }
}
