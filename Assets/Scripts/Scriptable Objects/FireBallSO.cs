using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "FireBall", menuName = "ScriptableObjects/Spells/FireBall", order = 1)]
    public class FireBallSO : GenreicSpellSO
    {
        public int health;
        public int maxHealth;

        public int mana;
        public int maxMana;

        public float speed;

        public override void Cast(Vector3 PlayerPos, Quaternion PlayerRotation)
        {
            GameObject fireballObject = Instantiate(spellObject, PlayerPos, PlayerRotation);
            SpellFireBall fireballScript = fireballObject.AddComponent(typeof(SpellFireBall)) as SpellFireBall;
            if(fireballScript != null)
            {
                fireballScript.Initialize(PlayerPos, PlayerRotation);
                fireballScript.SetSpellData(this);
            }

        }
    }
}