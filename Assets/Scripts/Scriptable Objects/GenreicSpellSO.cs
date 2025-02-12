using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    public abstract class GenreicSpellSO : ScriptableObject
    {
        public int manaCost;
        public bool continousCast;
        public int castTime;
        public string spellName;
        public int coolDownDuration;
        public int spellDuration;
        public float radius;
        public int lifetime;
        public GameObject spellObject;

        public abstract void Cast(Vector3 PlayerPos, Quaternion PlayerAngles);
    }
}