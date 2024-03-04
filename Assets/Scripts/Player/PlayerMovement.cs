using UnityEngine;
using Utilities.StackableElement;
using Utilities.StackableElement.SpeedHandler;

namespace Player
{
    /// <summary>
    /// A class that controls the player's movement.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Fields and Properties
        
        /// <summary>
        /// The basic move speed of player.
        /// </summary>
        [field: SerializeField] public float WalkSpeed { get; private set; }
        
        /// <summary>
        /// The run speed of player.
        /// </summary>
        ///
        /// <remarks>
        /// If the player is running, this will be added upon the <see cref="WalkSpeed"/>.
        /// </remarks>
        [field: SerializeField] public float RunSpeed { get; private set; }

        /// <summary>
        /// The <see cref="SpeedHandler{TID}"/> of player, which uses <see cref="EPlayerSpeedElement"/> as the ID for
        /// each <see cref="SpeedElement"/>.
        /// </summary>
        private SpeedHandler<EPlayerSpeedElement> _speedHandler;
        
        /// <summary>
        /// The <see cref="Rigidbody"/> of player.
        /// </summary>
        private Rigidbody _rb;

        /// <summary>
        /// The <see cref="PlayerInputHandler"/>.
        /// </summary>
        private PlayerInputHandler _inputHandler;

        #endregion
        
        #region MonoBehavior Functions
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            _inputHandler = PlayerInputHandler.Instance;
            
            InitializeSpeedHandler();
            RegisterInputEvents();
        }
        
        private void FixedUpdate()
        {
            Move();
        }
        
        #endregion

        #region Private Functions
        
        /// <summary>
        /// Using <see cref="Rigidbody"/> to move the player, disregarding the player's mass.
        /// The inputs are gained from <see cref="PlayerInputHandler"/>.
        /// </summary>
        private void Move()
        {
            Vector3 velocity = new Vector3(_inputHandler.MoveDirection.x, 0, _inputHandler.MoveDirection.y) *
                               _speedHandler.GetSpeed();
            
            _rb.AddForce(velocity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        
        /// <summary>
        /// <para>
        /// Initialize the <see cref="SpeedHandler{TID}"/> for player.
        /// </para>
        ///
        /// <para>
        /// <see cref="SpeedElement"/>s of <see cref="EPlayerSpeedElement.WalkSpeed"/> and <see cref="EPlayerSpeedElement.RunSpeed"/>
        /// will be added to the <see cref="SpeedHandler{TID}"/> by default. Both elements falls into additive bonus category.
        /// </para>
        /// </summary>
        private void InitializeSpeedHandler()
        {
            _speedHandler = new SpeedHandler<EPlayerSpeedElement>();
            
            SpeedElement walkElement = new SpeedElement(WalkSpeed, 0, 0, 1, true);
            SpeedElement runElement = new SpeedElement(RunSpeed, 0, 0, 1, true);
            
            _speedHandler.AdditiveBonus.Add(EPlayerSpeedElement.WalkSpeed, walkElement);
            _speedHandler.AdditiveBonus.Add(EPlayerSpeedElement.RunSpeed, runElement);
        }

        /// <summary>
        /// Register events for the player's inputs.
        /// </summary>
        private void RegisterInputEvents()
        {
            // When start to move(WASD).
            _inputHandler.WalkAction.started += context =>
            {
                _speedHandler.AdditiveBonus.IncreaseStack(EPlayerSpeedElement.WalkSpeed, 1);
            };
            
            // When stop moving.
            _inputHandler.WalkAction.canceled += context =>
            {
                _speedHandler.AdditiveBonus.DecreaseStack(EPlayerSpeedElement.WalkSpeed, 1);
            };
            
            // When press run button.
            _inputHandler.RunAction.started += context =>
            {
                _speedHandler.AdditiveBonus.IncreaseStack(EPlayerSpeedElement.RunSpeed, 1);
            };
            
            // When release run button.
            _inputHandler.RunAction.canceled += context =>
            {
                _speedHandler.AdditiveBonus.DecreaseStack(EPlayerSpeedElement.RunSpeed, 1);
            };
        }
        
        #endregion
    }
}