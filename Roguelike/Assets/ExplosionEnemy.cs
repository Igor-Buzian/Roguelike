using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    [SerializeField] GameObject Particles;
    [SerializeField] GameObject Enemy;
    MAttackTrigger AttackTrigger;
    private void Start()
    {
        AttackTrigger = GetComponent<MAttackTrigger>();
        AttackTrigger.enabled = true;
    }
    private void OntriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Exploision();
        }
        else if (other.CompareTag("Player"))
        {
            Exploision();
        }
    }

    void Exploision()
    {
        Particles.SetActive(true);
        Enemy.SetActive(false);
    }
}
