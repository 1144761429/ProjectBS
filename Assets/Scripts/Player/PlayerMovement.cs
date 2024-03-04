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

            InitializeSpeedHandler();
            
            _rb = GetComponent<Rigidbody>();

            _inputHandler = GetComponent<PlayerInputHandler>();
            //_inputHandler.
        }

        private void Update()
        {
            // if (PlayerInputHandler.MoveDirection != Vector2.zero)
            // {
            //     _speedHandler.AdditiveBonus.IncreaseStack(EPlayerSpeedElement.WalkSpeed, 1);
            // }
            // else
            // {
            //     
            // }
            
            //Move();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(Move(), ForceMode.VelocityChange);
        }

        // TODO: add comment
        private Vector3 Move()
        {
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");
            
            Vector3 moveDirection = new Vector3(xInput, 0f, zInput).normalized;
            Debug.Log(moveDirection);
            //_rb.velocity = moveDirection * 10;// * _speedHandler.GetSpeed();

            return moveDirection;
        }
        
        // TODO: add comment
        private void InitializeSpeedHandler()
        {
            SpeedElement walkElement = new SpeedElement(WalkSpeed, 0, 0, 1, true);
            SpeedElement runElement = new SpeedElement(RunSpeed, 0, 0, 1, true);
            
            _speedHandler.AdditiveBonus.Add(EPlayerSpeedElement.WalkSpeed, walkElement);
            _speedHandler.AdditiveBonus.Add(EPlayerSpeedElement.RunSpeed, runElement);
        }
    }
}