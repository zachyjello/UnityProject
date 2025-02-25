using System.Collections;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Interfaces;
using Unity.VisualScripting;
using UnityEditor.Build;

namespace Assets.Scripts
{
    public class SpellFireBall : MonoBehaviour, ISpell
    {
        private SphereCollider _collider;
        private Rigidbody _rigidbody;
        private FireBallSO _spellData;
        private int spawnDistance = 1;

        private void Awake()
        {
            
        }
        public void Initialize(Vector3 PlayerPos, Quaternion PlayerRotation, ISpellSO spell)
        {
            _spellData = spell as FireBallSO;
            _collider = gameObject.AddComponent<SphereCollider>();
            _rigidbody = gameObject.AddComponent<Rigidbody>();

            transform.position = PlayerPos + PlayerRotation*Vector3.forward*spawnDistance;
            transform.rotation = PlayerRotation;

            Invoke(nameof(OnExpiration), _spellData.lifetime);
            Debug.Log("Fireball created");
        }

        void Update()
        {
            Debug.Log("Fireball Moving");
            transform.Translate(Vector3.forward * _spellData.speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            IDamageable damageableTarget = collision.gameObject.GetComponent<IDamageable>();
            if (collision.gameObject.tag != "Player")
            {
                damageableTarget.ReceiveDamage(_spellData.damage);
                Debug.Log(collision.gameObject.tag);
                Debug.Log("fireball exploded");
                Destroy(this.gameObject);
            }
        }

        public void OnExpiration()
        {
            Destroy(this.gameObject);
        }
    }
}