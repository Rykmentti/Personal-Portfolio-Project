using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject enemyMeleeNPC; // Assign in Editor
    [SerializeField] GameObject enemyRangedNPC; // Assign in Editor
    [SerializeField] GameObject enemyCasterNPC; // Assign in Editor

    [SerializeField] GameObject[] spawnPointsInOrderOfShortestDistance; // Assign in Editor

    [SerializeField] Button startWaveButton; // Assign in Editor
    [SerializeField] TMP_Text waveCounterText; // Assign in Editor;
    int waveNumber;

    // Start is called before the first frame update
    void Start()
    {
        startWaveButton.onClick.AddListener(() => { SpawnNumberedWave(); });
        waveCounterText.text = "Wave: " + waveNumber.ToString();
    }

    void SpawnNumberedWave()
    {
        if (waveNumber == 0) SpawnWave(3, 2, 0);
        else if (waveNumber == 1) SpawnWave(5, 3, 0);
        else if (waveNumber == 2) SpawnWave(8, 2, 0);
        else if (waveNumber == 3) SpawnWave(8, 5, 0);
        else if (waveNumber == 4) SpawnWave(10, 3, 2);
        else if (waveNumber == 5) SpawnWave(10, 7, 3);
        else if (waveNumber == 6) SpawnWave(12, 10, 3);
        else if (waveNumber == 7) SpawnWave(10, 10, 5);
        else if (waveNumber == 8) SpawnWave(15, 5, 5);
        else if (waveNumber == 9) SpawnWave(15, 5, 5);
    }

    void SpawnWave(int meleeNPC_Amount, int rangedNPC_Amount, int casterAmount_NPC)
    {
        int spawnPosition = 0;
        waveNumber++;
        waveCounterText.text = "Wave: " + waveNumber.ToString();
        for (int i = 0; i < meleeNPC_Amount; i++)
        {
            GameObject enemy = Instantiate(enemyMeleeNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            enemy.tag = "Red";
            spawnPosition++;
        }
        for (int j = 0; j < rangedNPC_Amount; j++)
        {
            GameObject enemy = Instantiate(enemyRangedNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            enemy.tag = "Red";
            spawnPosition++;
        }
        for (int k = 0; k < casterAmount_NPC; k++)
        {
            GameObject enemy = Instantiate(enemyCasterNPC, spawnPointsInOrderOfShortestDistance[spawnPosition].transform.position, Quaternion.identity);
            enemy.tag = "Red";
            spawnPosition++;
        }
    }
}
