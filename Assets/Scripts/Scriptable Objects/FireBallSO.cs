using Assets.Scripts.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "FireBall", menuName = "ScriptableObjects/Spells/FireBall", order = 1)]
    public class FireBallSO : ScriptableObject, ISpellSO
    {
        [Header("General")]
        public string spellName;
        public float cooldown;
        public GameObject fireBallObject;

        [Header("Ressources")]
        public int damage;
        public int manaCost;

        [Header("Physical Constraints")]
        public float speed;
        public float lifetime;
        public float radius;
        public float castTime;

        public void Cast(Vector3 PlayerPos, Quaternion PlayerRotation)
        {
            GameObject fireballObject = Instantiate(fireBallObject, PlayerPos, PlayerRotation);
            SpellFireBall fireballScript = fireballObject.AddComponent(typeof(SpellFireBall)) as SpellFireBall;
            if(fireballScript != null)
            {
                fireballScript.Initialize(PlayerPos, PlayerRotation, this);
            }

        }
    }
}