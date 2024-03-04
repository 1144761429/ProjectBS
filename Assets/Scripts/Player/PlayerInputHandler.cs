using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DefaultNamespace.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;

        #region Movement

        public InputAction WalkAction { get; private set; }
        public InputAction RunAction { get; private set; }

        public static Vector2 MoveDirection { get; private set; }
        public static bool IsHoldingRunButton { get; private set; }
        
        #endregion
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();

            WalkAction = _playerInput.actions["Walk"];
            RunAction = _playerInput.actions["Run"];
        }

        private void Update()
        {
            MoveDirection = WalkAction.ReadValue<Vector2>();
            IsHoldingRunButton = RunAction.ReadValue<bool>();
        }
    }
}