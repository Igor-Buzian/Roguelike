using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RoomsPlacer roomsPlacer;
    public EnemyManager enemyManager;

    private void Start()
    {
        enemyManager.InitializeEnemies(roomsPlacer);
    }
}