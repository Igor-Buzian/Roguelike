using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] ParticleSystem[] EnemyParticleSystem;
    MAttackTrigger AttackTrigger;
    BoxCollider EnemyCollider;
    private void Start()
    {
        AttackTrigger = GetComponent<MAttackTrigger>();
        AttackTrigger.enabled = true;
    }

    public void Exploision()
    {
        Enemy.SetActive(false);
        for(int i = 0; i < EnemyParticleSystem.Length; i++)
        {
            EnemyParticleSystem[i].Play();
        }
    }
}
