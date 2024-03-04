using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;

        #region Movement

        private InputAction _walkAction;
        private InputAction _runAction;

        public static Vector2 MoveDirection { get; private set; }
        public static bool IsHoldingRunButton { get; private set; }
        
        #endregion
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();

            _walkAction = _playerInput.actions["Walk"];
            _runAction = _playerInput.actions["Run"];
        }

        private void Update()
        {
            MoveDirection = _walkAction.ReadValue<Vector2>();
            IsHoldingRunButton = _runAction.ReadValue<bool>();
        }
    }
}