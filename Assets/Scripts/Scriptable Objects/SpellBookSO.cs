using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "SpellBook", menuName = "ScriptableObjects/Spells/Spellbook", order = 2)]
    public class SpellBookSO : ScriptableObject
    {
        public int spellAmount;
        public List<ISpellSO> spells;
        public List<ISpellSO> unlockedSpells;

        public void unlockSpell(ISpellSO newSpell)
        {
            if (!unlockedSpells.Contains(newSpell))
                unlockedSpells.Add(newSpell);
        }
        public void addSpell(ISpellSO spellToAdd)
        {
            if (!(spells.Count > spellAmount))
                spells.Add(spellToAdd);
        }
        public List<ISpellSO> getSpells()
        {
            return spells;
        }
        public int getSpellAmount()
        {
            return spellAmount;
        }
    }
}