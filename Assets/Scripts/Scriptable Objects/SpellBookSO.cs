using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "SpellBook", menuName = "ScriptableObjects/Spells/Spellbook", order = 2)]
    public class SpellBookSO : ScriptableObject
    {
        public int spellAmount;
        public List<GenreicSpellSO> spells;
        public List<GenreicSpellSO> unlockedSpells;

        public void unlockSpell(GenreicSpellSO newSpell)
        {
            if (!unlockedSpells.Contains(newSpell))
                unlockedSpells.Add(newSpell);
        }
        public void addSpell(GenreicSpellSO spellToAdd)
        {
            if (!(spells.Count > spellAmount))
                spells.Add(spellToAdd);
        }
        public List<GenreicSpellSO> getSpells()
        {
            return spells;
        }
        public int getSpellAmount()
        {
            return spellAmount;
        }
    }
}