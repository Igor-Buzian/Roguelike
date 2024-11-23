using Leopotam.Ecs;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _systems;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // ������ �����
    public int enemyCount = 5; // ���������� ������ ��� ������

    void Start()
    {
        // ������������� ECS
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems.Add(new EnemySystem());
        //_systems.Add(new EnemyCollisionSystem());
        _systems.Init();

        SpawnEnemies(); // ������� ������
    }

    void Update()
    {
        _systems.Run(); // ��������� ������� ECS
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // ���������� ��������� ������� ��� ������
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // �������������� ����� � ECS
            InitializeEnemy(enemyObject);
        }
    }

    private void InitializeEnemy(GameObject enemyObject)
    {
        var enemyEntity = _world.NewEntity();

        // �������� ���������� �����
        ref var enemy = ref enemyEntity.Get<EnemyComponent>();
        enemy.health = 4; // ��������� ��������
        enemy.moveSpeed = 3f; // �������� �������� �����
        enemy.transform = enemyObject.transform; // ������ �� Transform �����

        ref var target = ref enemyEntity.Get<TargetComponent>();
        target.target = GameObject.FindGameObjectWithTag("Player").transform; // ���� ��� �������������
    }

    private void OnDestroy()
    {
        _systems.Destroy(); // ����������� ������� ������
        _world.Destroy(); // ����������� ������� ����
    }
}