using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    public abstract class GenericEnemySO : ScriptableObject
    {
        public float maxHealth;
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}