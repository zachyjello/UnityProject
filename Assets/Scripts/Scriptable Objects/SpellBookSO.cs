using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Mono.Reflection;
using System.Linq;
using System;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "SpellBook", menuName = "ScriptableObjects/Spells/Spellbook", order = 2)]
    public class SpellBookSO : ScriptableObject
    {
        public int spellAmount;

        [SerializeReference]
        public List<ScriptableObject> spells;
        [SerializeReference]
        public List<ISpellSO> unlockedSpells;

        public void unlockSpell(ISpellSO newSpell)
        {
            if (!unlockedSpells.Contains(newSpell))
                unlockedSpells.Add(newSpell);
        }
        public void addSpell(ISpellSO spellToAdd)
        {
            if (spells.Count < spellAmount && spellToAdd is ScriptableObject component)
            {
                spells.Add(component);
            }
            else
                Console.WriteLine("SpellBook is full!");
        }
        public List<ISpellSO> getSpells()
        {
            return spells.OfType<ISpellSO>().ToList();
        }
        public int getSpellAmount()
        {
            return spellAmount;
        }
    }
}