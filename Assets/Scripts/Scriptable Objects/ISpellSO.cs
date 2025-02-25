using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    public interface ISpellSO
    {
        public void Cast(Vector3 PlayerPos, Quaternion PlayerAngles);
    }
}