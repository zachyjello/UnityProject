using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player/PlayerSO", order = 1)]
    public class PlayerDataSO : ScriptableObject
    {
        public int health;
        public int maxHealth;

        public int mana;
        public int maxMana;

        public Vector3 position;
        public Quaternion rotation;
    }
}