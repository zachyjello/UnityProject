using UnityEngine;

namespace Entities.Player
{
    public class PlayerInput : MonoBehaviour
    {
        // 1. Movement Properties
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public Vector2 MoveDirection => new Vector2(Horizontal, Vertical);

        // 2. Action Triggers (Booleans)
        public bool JumpPressed { get; private set; }
        public bool InteractPressed { get; private set; }
        public bool SprintHeld { get; private set; }

        private void Update()
        {
            // Gather Raw Input
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");

            // We use a "Buffer" or "Trigger" pattern for one-shot actions
            if (Input.GetKeyDown(KeyCode.Space)) JumpPressed = true;
            if (Input.GetKeyDown(KeyCode.E)) InteractPressed = true;
            
            // Continuous state
            SprintHeld = Input.GetKey(KeyCode.LeftShift);
        }

        /// <summary>
        /// Call this at the end of FixedUpdate in your movement/interaction scripts
        /// to ensure triggers aren't processed twice.
        /// </summary>
        public void ClearInputTriggers()
        {
            JumpPressed = false;
            InteractPressed = false;
        }
    }
}