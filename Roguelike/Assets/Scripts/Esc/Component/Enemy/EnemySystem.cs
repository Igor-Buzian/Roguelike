using Leopotam.Ecs;
using UnityEngine;

public struct EnemyComponent
{
    public float moveSpeed;
    public Transform transform; // ��������� ���� ��� ������ �� Transform
}
public struct TargetComponent
{
    public Transform target; // ���� ��� �������������
}

public class EnemySystem : IEcsRunSystem
{
    private EcsFilter<EnemyComponent, TargetComponent> _filter;

    public void Run()
    {
        foreach (var i in _filter)
        {
            ref var enemy = ref _filter.Get1(i);
            ref var target = ref _filter.Get2(i);

            // ������� ����� � ����
            MoveTowardsTarget(ref enemy, target.target);
        }
    }

    private void MoveTowardsTarget(ref EnemyComponent enemy, Transform target)
    {
        Vector3 direction = (target.position - enemy.transform.position); // ���������� enemy.transform
        direction.y = 0; // ���������� ��������� �� Y
        direction.Normalize();

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 3);

        enemy.transform.Translate(direction * enemy.moveSpeed * Time.deltaTime);
    }
}