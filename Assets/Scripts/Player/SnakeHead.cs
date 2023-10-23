using System;
using System.Collections.Generic;
using Garvity;
using Infrastructure;
using Serivces;
using TMPro;
using UnityEngine;

namespace Player
{
    public class SnakeHead : MonoBehaviour, IArtificiallyAttracted
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _steerSpeed;
        [SerializeField] private float _bodySpeed;
        [SerializeField] private int _insert;
        [SerializeField] private int _speedBodyRotation = 5;

        private List<SnakeBody> _bodies = new();
        private List<Vector3> _positionHead = new ();
        private IInputService _inputService;
        private IGameFactory _gameFactory;
        public Transform Transform { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        
        
        private void Awake()
        {
            Transform = GetComponent<Transform>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void Construct(IGameFactory gameFactory, IInputService inputService)
        {
            _gameFactory = gameFactory;
            _inputService = inputService;
        }

        void FixedUpdate() => 
            Move();

        private void Move()
        {
            Rigidbody.MovePosition(Rigidbody.position + transform.forward * _moveSpeed * Time.fixedDeltaTime);

            float steerDirection = _inputService.AxisX;
            transform.Rotate(Vector3.up * steerDirection * _steerSpeed * Time.fixedDeltaTime);

            _positionHead.Insert(0, transform.position);
            int i = 0;
            foreach (var body in _bodies)
            {
                Vector3 position = _positionHead[Mathf.Min(i * _insert, _positionHead.Count - 1)];
                Vector3 positionForward = position - body.transform.position;
                body.transform.position += positionForward * _bodySpeed * Time.fixedDeltaTime;
                body.transform.rotation = Quaternion.Lerp(body.transform.rotation, transform.rotation, Time.fixedDeltaTime * _speedBodyRotation);
                i++;
            }
        }

        private void OnTriggerEnter(Collider other) => 
            AddBody(other);

        private void AddBody(Collider other)
        {
            if (other.TryGetComponent<Eat>(out Eat eat))
            {
                var spawnPosition = transform.position - transform.forward * 2;
                if(_bodies.Count > 0)
                    spawnPosition = _bodies[_bodies.Count - 1].transform.position;
                var newBody = _gameFactory
                    .Instantiate(AssetsPath.SnakeBodyPath,spawnPosition , transform)
                    .GetComponent<SnakeBody>();
                eat.Collect();
                _bodies.Add(newBody);
            }

        }
    }
}