using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAllEnemies : MonoBehaviour
{
    [SerializeField] GameObject VictoryCanvas;
    int EnemyCount;

    private void OnDisable()
    {
        EnemyCount = PlayerPrefs.GetInt("EnemyCount", 0);
        EnemyCount--;
        if (EnemyCount <= 0)
        {
            VictoryCanvas.SetActive(true);
            PlayerPrefs.SetInt("EnemyCount", 0);
            Time.timeScale = 0;
        }
        else
        {
            PlayerPrefs.SetInt("EnemyCount", EnemyCount);
        }

    }
}
