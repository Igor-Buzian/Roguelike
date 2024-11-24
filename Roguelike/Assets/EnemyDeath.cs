using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] ParticleSystem[] EnemyParticleSystem;
    [SerializeField]MAttackTrigger AttackTrigger;
    BoxCollider EnemyCollider;
    private void Start()
    {

      EnemyCollider = GetComponent<BoxCollider>();

    }
    private void Update()
    {
        if (transform.position.y < -1)
        {
            ExploisionDeath();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ExploisionDeath();
        }
    }
    public void ExploisionDeath()
    {
        Enemy.SetActive(false);
        for (int i = 0; i < EnemyParticleSystem.Length; i++)
        {
            EnemyParticleSystem[i].Play();
        }
        AttackTrigger.enabled = false;
    }
}
