using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public NavMeshAgent agent;
        Rigidbody rb;
        enemyState state = enemyState.passive;
        public int health = 100;
        enum enemyState
        {
            passive,
            alert,
            agro
        }

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case enemyState.passive: PassiveHandler(); break;
                case enemyState.alert: break;
                case enemyState.agro: break;
            }
        }

        public void PassiveHandler()
        {
           
        }

        public void ReceiveDamage(int Damage)
        {
            health -= Damage;
            Debug.Log("Enemy health = " + health);
            if (health <= 0) OnDeath();
        }

        public void OnDeath()
        {
            Debug.Log("Enemy has been killed");
            Destroy(this.gameObject);
        }
    }
}