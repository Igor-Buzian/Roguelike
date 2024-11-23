using Leopotam.Ecs;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _systems;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // Префаб врага
    public int enemyCount = 5; // Количество врагов для спавна

    void Start()
    {
        // Инициализация ECS
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems.Add(new EnemySystem());
        //_systems.Add(new EnemyCollisionSystem());
        _systems.Init();

        SpawnEnemies(); // Создаем врагов
    }

    void Update()
    {
        _systems.Run(); // Запускаем системы ECS
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Генерируем случайные позиции для спавна
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Инициализируем врага в ECS
            InitializeEnemy(enemyObject);
        }
    }

    private void InitializeEnemy(GameObject enemyObject)
    {
        var enemyEntity = _world.NewEntity();

        // Получаем компоненты врага
        ref var enemy = ref enemyEntity.Get<EnemyComponent>();
        enemy.health = 4; // Начальное здоровье
        enemy.moveSpeed = 3f; // Скорость движения врага
        enemy.transform = enemyObject.transform; // Ссылка на Transform врага

        ref var target = ref enemyEntity.Get<TargetComponent>();
        target.target = GameObject.FindGameObjectWithTag("Player").transform; // Цель для преследования
    }

    private void OnDestroy()
    {
        _systems.Destroy(); // Освобождаем ресурсы систем
        _world.Destroy(); // Освобождаем ресурсы мира
    }
}