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

    private IEnumerator Start()
    {
        if (RoomPrefabs == null || RoomPrefabs.Length == 0)
        {
            Debug.LogError("RoomPrefabs не настроены!");
            yield break;
        }

        if (StartingRoom == null)
        {
            Debug.LogError("StartingRoom не настроена!");
            yield break;
        }

        spawnedRooms = new Room[11, 11];
        spawnedRooms[5, 5] = StartingRoom;

        for (int i = 0; i < 12; i++)
        {
            yield return new WaitForSecondsRealtime(0.5f); // Уберите это для мгновенной генерации

            PlaceOneRoom();
        }
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
        GameObject newEnemy = Instantiate(spawnedEnemy[Random.Range(0, spawnedEnemy.Length)]);

        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 14;
                newWeapon.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 14;
                newEnemy.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 14;

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
