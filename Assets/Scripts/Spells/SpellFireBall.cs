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

            _rigidbody.mass = 0.01f;
            _rigidbody.useGravity = false;

            transform.position = PlayerPos + PlayerRotation*Vector3.forward*spawnDistance;
            transform.rotation = PlayerRotation;

            Invoke(nameof(OnExpiration), _spellData.lifetime);
            Debug.Log("Fireball created");
        }

        void Update()
        {
            transform.Translate(Vector3.forward * _spellData.speed * Time.deltaTime);
            _rigidbody.MovePosition(Vector3.forward * _spellData.speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            IDamageable damageableTarget = collision.gameObject.GetComponent<IDamageable>();
            switch (collision.gameObject.tag)
            {
                case "Damageable":
                    damageableTarget.ReceiveDamage(_spellData.damage);
                    Destroy(this.gameObject);
                    break;
                default:
                    Destroy(this.gameObject);
                    break;
            }
        }

        public void OnExpiration()
        {
            Destroy(this.gameObject);
        }
    }
}