using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// A handler that handles the input value of player.
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Fields and Properties
        
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static PlayerInputHandler Instance { get; private set; }
        
        /// <summary>
        /// The Unity <see cref="PlayerInput"/> that contains a player <see cref="InputActionMap"/>.
        /// </summary>
        private PlayerInput _playerInput;

        #region Movement
        
        /// <summary>
        /// <see cref="InputAction"/> of Walk, which is AWSD.
        /// </summary>
        public InputAction WalkAction { get; private set; }
        
        /// <summary>
        /// <see cref="InputAction"/> of run, which is <see cref="KeyCode.LeftShift"/>
        /// </summary>
        public InputAction RunAction { get; private set; }

        /// <summary>
        /// The direction of <see cref="InputAction"/> Walk.
        /// </summary>
        public Vector2 MoveDirection { get; private set; }
        
        /// <summary>
        /// If the player is holding down <see cref="KeyCode.LeftShift"/>.
        /// </summary>
        public bool IsHoldingRunButton { get; private set; }
        
        #endregion
        
        #endregion

        #region MonoBehavior Functions
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            Debug.Log(_playerInput == null);
            WalkAction = _playerInput.actions["Walk"];
            RunAction = _playerInput.actions["Run"];

            InitSingleton();
        }

        private void Update()
        {
            MoveDirection = WalkAction.ReadValue<Vector2>();
            IsHoldingRunButton = RunAction.IsPressed();
        }
        
        #endregion

        #region Private Functions
        
        /// <summary>
        /// Initialize the Singleton for this <see cref="PlayerInputHandler"/>.
        /// </summary>
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
        
        #endregion
    }
}