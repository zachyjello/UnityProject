using System.Collections;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

namespace Assets.Scripts
{
    public class SpellFireBall : Spell
    {
        private float speed;
        private FireBallSO spellData;

        private void Awake()
        {
            
        }
        public override void Initialize(Vector3 PlayerPos, Quaternion PlayerRotation)
        {
            transform.position = PlayerPos;
            transform.rotation = PlayerRotation;
            Debug.Log("Fireball created");
        }

        public void SetSpellData(FireBallSO spell)
        {
            this.spellData = spell;
            this.speed = spellData.speed;
            myCollider = gameObject.AddComponent<SphereCollider>();
            myCollider.isTrigger = true;
            myCollider.radius = spellData.radius;
            myCollider.enabled = false;
            Invoke("OnColliderActivation", 1.0f);

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.isKinematic = true;
        

            Invoke("OnExpiration", spellData.lifetime);
        }


        void Update()
        {
            Debug.Log("Fireball Moveing");
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("fireball entered collision");
            //Destroy(this.gameObject);
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