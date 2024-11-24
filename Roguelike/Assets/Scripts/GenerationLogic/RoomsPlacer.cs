using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsPlacer : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public GameObject[] spawnedWeapon;
    public GameObject[] spawnedEnemy;
    public Room StartingRoom;

    private Room[,] spawnedRooms;
    bool EcsEnemy;
    public List<Vector3> ECSEnemySpawnPositions { get; private set; } = new List<Vector3>(); // Позиции врагов
    private void Awake()
    {

        Time.timeScale = 1; // Снова запускаем время
        if (RoomPrefabs == null || RoomPrefabs.Length == 0)
        {
            Debug.LogError("RoomPrefabs не настроены!");
            return;
        }

        if (StartingRoom == null)
        {
            Debug.LogError("StartingRoom не настроена!");
             return;
        }

        spawnedRooms = new Room[11, 11];
        spawnedRooms[5, 5] = StartingRoom;

        for (int i = 0; i < 12; i++)
        {

            PlaceOneRoom();
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        PlayerPrefs.SetInt("EnemyCount", enemies.Length);
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]); 
        GameObject newWeapon = Instantiate(spawnedWeapon[Random.Range(0, spawnedWeapon.Length)]);
        GameObject newEnemy;
        if (Random.Range(0,100) < 70)
        {
            newEnemy = Instantiate(spawnedEnemy[0]);
            EcsEnemy = false;
        }
        else
        {
            newEnemy = null;
            EcsEnemy = true;
        }


        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 14;
                newWeapon.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 14;
                if (EcsEnemy)
                {
                    ECSEnemySpawnPositions.Add(new Vector3(position.x - 5, .5f, position.y - 5) * 14);
                }
                else
                {
                    newEnemy.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 14;
                }
                spawnedRooms[position.x, position.y] = newRoom; // Исправлено
                Debug.Log($"Размещена комната на позиции {position}");
                return;
            }
        }

        Destroy(newRoom.gameObject);
        Destroy(newRoom.gameObject);
        Destroy(newRoom.gameObject);
    }

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (p.x > 0 && spawnedRooms[p.x - 1, p.y] != null) neighbours.Add(Vector2Int.left);
        if (p.y > 0 && spawnedRooms[p.x, p.y - 1] != null) neighbours.Add(Vector2Int.down);
        if (p.x < maxX && spawnedRooms[p.x + 1, p.y] != null) neighbours.Add(Vector2Int.right);
        if (p.y < maxY && spawnedRooms[p.x, p.y + 1] != null) neighbours.Add(Vector2Int.up);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        Debug.Log($"Комната подключена к {selectedDirection}");
        return true;
    }
}
