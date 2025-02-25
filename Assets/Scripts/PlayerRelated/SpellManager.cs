using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Interfaces; 



namespace Assets.Scripts
{
    public class SpellHandler : MonoBehaviour
    {
        public PlayerDataSO playerData;
        public SpellBookSO spellBook;
        private int spellAmount;
        private List<ISpellSO> spells;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            spellAmount = spellBook.getSpellAmount();
            spells = spellBook.getSpells();
            HandleCasting();
        }
        void HandleCasting()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (spells[0] == null)
                    Debug.Log("No spell in slot");
                else {
                    spells[0].Cast(playerData.position, playerData.rotation);
                    Debug.Log("Fireball launched");
                }
            }
            else
            {
                Debug.Log("No spell launched");
            }
        }
    }
}