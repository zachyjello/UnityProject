using System.Collections;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

namespace Assets.Scripts
{
    public abstract class Spell : MonoBehaviour
    {
        private readonly int _coolDownDuration;
        private readonly int _duration;
        protected SphereCollider myCollider;
        protected Rigidbody myRigidbody;

        public abstract void Initialize(Vector3 PlayerPos, Quaternion PlayerRotation);

        //public abstract void SetSpellData(SpellSO spell);
        public abstract void OnExpiration();
    }
}