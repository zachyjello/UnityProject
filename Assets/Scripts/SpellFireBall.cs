using System.Collections;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Interfaces;
using Unity.VisualScripting;

namespace Assets.Scripts
{
    public class SpellFireBall : Spell
    {
        private float speed;
        private GenreicSpellSO spellData;
        private int spawnDistance = 1;

        private void Awake()
        {
            
        }
        public override void Initialize(Vector3 PlayerPos, Quaternion PlayerRotation, GenreicSpellSO spell)
        {
            SetSpellData(spell);
            transform.position = PlayerPos + PlayerRotation*Vector3.forward*spawnDistance;
            transform.rotation = PlayerRotation;
            Debug.Log("Fireball created");
        }

        public void SetSpellData(GenreicSpellSO spell)
        {
            this.spellData = spell;
            this.speed = spellData.speed;
            myCollider = gameObject.AddComponent<SphereCollider>();
            //myCollider.isTrigger = true;
            myCollider.radius = spellData.radius;
            //myCollider.enabled = false;
            //Invoke("OnColliderActivation", 0.2f);

            //myRigidbody = gameObject.AddComponent<Rigidbody>();
            //myRigidbody.isKinematic = true;


            Invoke("OnExpiration", spellData.lifetime);
        }


        void Update()
        {
            Debug.Log("Fireball Moving");
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            IDamageable damageableTarget = collision.gameObject.GetComponent<IDamageable>();
            if (collision.gameObject.tag != "Player")
            {
                damageableTarget.ReceiveDamage(spellData.health);
                Debug.Log(collision.gameObject.tag);
                Debug.Log("fireball exploded");
                Destroy(this.gameObject);
            }
            
        }

        public void OnColliderActivation()
        {
            myCollider.enabled = true;
        }

        public override void OnExpiration()
        {
            Destroy(this.gameObject);
        }
    }
}