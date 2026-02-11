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
        public int maxHealth = 100;
        enum enemyState
        {
            passive,
            alert,
            agro
        }

        [SerializeField] private FloatingHealthBar healthBar;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            healthBar = GetComponentInChildren<FloatingHealthBar>();
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
            healthBar.UpdateHealthBar(health, maxHealth);
            if (health <= 0) OnDeath();
        }

        public void OnDeath()
        {
            Debug.Log("Enemy has been killed");
            Destroy(this.gameObject);
        }
    }
}