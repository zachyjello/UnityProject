using System.Collections;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

namespace Assets.Scripts
{
    public interface ISpell
    {
        public abstract void Initialize(Vector3 PlayerPos, Quaternion PlayerRotation, ISpellSO spell);

        public void OnExpiration();
    }
}