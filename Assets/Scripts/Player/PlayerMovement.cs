using System;
using UnityEngine;
using Utilities.StackableElement;
using Utilities.StackableElement.Core;
using Utilities.StackableElement.SpeedHandler;

namespace DefaultNamespace.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [field: SerializeField] public float WalkSpeed { get; private set; }
        [field: SerializeField] public float RunSpeed { get; private set; }

        private SpeedHandler<EPlayerSpeedElement> _speedHandler;
        private Rigidbody _rb;
        private PlayerInputHandler _inputHandler;
        
        private void Awake()
        {
            _speedHandler = new SpeedHandler<EPlayerSpeedElement>();
            _rb = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            
            InitializeSpeedHandler();
            RegisterInputEvents();
        }
        
        private void FixedUpdate()
        {
            Move();
        }

        // TODO: add comment
        private void Move()
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float zInput = Input.GetAxisRaw("Vertical");
            
            Vector3 velocity = new Vector3(xInput, 0f, zInput).normalized * _speedHandler.GetSpeed();
            Debug.Log(velocity);
            _rb.AddForce(velocity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        
        // TODO: add comment
        private void InitializeSpeedHandler()
        {
            SpeedElement walkElement = new SpeedElement(WalkSpeed, 0, 0, 1, true);
            SpeedElement runElement = new SpeedElement(RunSpeed, 0, 0, 1, true);
            
            _speedHandler.AdditiveBonus.Add(EPlayerSpeedElement.WalkSpeed, walkElement);
            _speedHandler.AdditiveBonus.Add(EPlayerSpeedElement.RunSpeed, runElement);
        }

        // TODO: add comment
        private void RegisterInputEvents()
        {
            _inputHandler.WalkAction.started += context =>
            {
                _speedHandler.AdditiveBonus.IncreaseStack(EPlayerSpeedElement.WalkSpeed, 1);
                Debug.Log("adsadasd");
            };
            
            _inputHandler.WalkAction.canceled += context =>
            {
                _speedHandler.AdditiveBonus.DecreaseStack(EPlayerSpeedElement.WalkSpeed, 1);
            };
            
            _inputHandler.RunAction.started += context =>
            {
                _speedHandler.AdditiveBonus.IncreaseStack(EPlayerSpeedElement.RunSpeed, 1);
            };
            
            _inputHandler.RunAction.canceled += context =>
            {
                _speedHandler.AdditiveBonus.DecreaseStack(EPlayerSpeedElement.RunSpeed, 1);
            };
        }
    }
}