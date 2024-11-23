using UnityEngine;
using Leopotam.Ecs;

public class EnemyCollisionSystem : IEcsRunSystem
{
    private EcsFilter<EnemyComponent, TargetComponent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var enemy = ref _filter.Get1(i);
            ref var target = ref _filter.Get2(i);

            // Проверка на столкновение с пулями или игроком
            CheckCollisions(enemy, target.target);
        }
    }

    private void CheckCollisions(EnemyComponent enemy, Transform target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, 0.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Bullet"))
            {
                Object.Destroy(hitCollider.gameObject); // Используйте Object.Destroy
                enemy.health--;

                if (enemy.health <= 0)
                {
                    HandleCollision(enemy);
                }
            }

            if (hitCollider.CompareTag("Player"))
            {
                ShowLosePanel();
            }
        }
    }

    private void HandleCollision(EnemyComponent enemy)
    {
        // Логика для обработки столкновения с пулей
        if (enemy.health <= 0)
        {
            enemy.killCount++;
            PlayerPrefs.SetInt("killCount", enemy.killCount);
            // Уничтожить врага и создать эффект взрыва
            // Можно вызвать метод для уничтожения врага
        }
    }

    private void ShowLosePanel()
    {
        // Получаем доступ к панели проигрыша и активируем её
        var losePanel = GameObject.Find("LosedCanvas");
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }
}