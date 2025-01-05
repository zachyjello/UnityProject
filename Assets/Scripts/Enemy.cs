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

        public void ReceiveDamage(float Damage)
        {

        }

        public void OnDeath()
        {

        }
    }
}