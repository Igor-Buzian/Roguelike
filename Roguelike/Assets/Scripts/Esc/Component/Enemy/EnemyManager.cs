using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _systems;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    void Start()
    {
        // Инициализация ECS
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems.Add(new EnemySystem());
        //_systems.Add(new EnemyCollisionSystem());
        _systems.Init();
    }
    /// <summary>
    /// Initialize Enemies in new room
    /// </summary>
    /// <param name="roomsPlacer"></param>
    public void InitializeEnemies(RoomsPlacer roomsPlacer)
     {
         SpawnEnemies(roomsPlacer.ECSEnemySpawnPositions);
     }
    /// <summary>
    /// Spawn Enemies in new room
    /// </summary>
    /// <param
    private void SpawnEnemies(List<Vector3> enemyPositions)
    {
        foreach (var position in enemyPositions)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, position, Quaternion.identity);
            InitializeEnemy(enemyObject);
        }
    }

    private void InitializeEnemy(GameObject enemyObject)
    {
        var enemyEntity = _world.NewEntity();

        ref var enemy = ref enemyEntity.Get<EnemyComponent>();
        enemy.moveSpeed = 1.5f;
        enemy.transform = enemyObject.transform;

        ref var target = ref enemyEntity.Get<TargetComponent>();
        target.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _systems.Run();
    }

    private void OnDestroy()
    {
        _systems.Destroy();
        _world.Destroy();
    }
}