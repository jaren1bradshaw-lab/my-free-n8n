using System.Collections;
using ColorRush.Core;
using UnityEngine;

namespace ColorRush.Runner
{
    public class RunnerController : MonoBehaviour
    {
        [SerializeField] private SwipeInputController input;
        [SerializeField] private float[] laneX = { -2.2f, 0f, 2.2f };
        [SerializeField] private float laneLerpSpeed = 16f;
        [SerializeField] private float baseForwardSpeed = 9f;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private float maxLeanAngle = 12f;

        private int _currentLane = 1;
        private int _targetLane = 1;
        private int _bufferedDirection;
        private float _speedMultiplier = 1f;

        public int CurrentLaneIndex => _currentLane;
        public float ForwardSpeed => baseForwardSpeed * _speedMultiplier;

        private void OnEnable()
        {
            if (input != null) input.OnSwipeHorizontal += HandleSwipe;
        }

        private void OnDisable()
        {
            if (input != null) input.OnSwipeHorizontal -= HandleSwipe;
        }

        public void ResetRun()
        {
            _currentLane = 1;
            _targetLane = 1;
            _bufferedDirection = 0;
            _speedMultiplier = 1f;

            Vector3 pos = transform.position;
            pos.x = laneX[_currentLane];
            transform.position = pos;
        }

        private void Update()
        {
            if (!GameManager.Instance || !GameManager.Instance.IsGameplayActive()) return;

            MoveForward();
            MoveLateral();
            UpdateLean();
        }

        private void MoveForward()
        {
            transform.position += Vector3.forward * (ForwardSpeed * Time.deltaTime);
        }

        private void MoveLateral()
        {
            Vector3 pos = transform.position;
            float targetX = laneX[_targetLane];
            pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * laneLerpSpeed);
            transform.position = pos;

            if (Mathf.Abs(pos.x - targetX) < 0.05f)
            {
                pos.x = targetX;
                transform.position = pos;
                _currentLane = _targetLane;

                if (_bufferedDirection != 0)
                {
                    int dir = _bufferedDirection;
                    _bufferedDirection = 0;
                    HandleSwipe(dir);
                }
            }
        }

        private void UpdateLean()
        {
            if (visualRoot == null) return;
            float laneDelta = laneX[_targetLane] - transform.position.x;
            float t = Mathf.Clamp(laneDelta / 2.2f, -1f, 1f);
            float angle = -t * maxLeanAngle;
            Quaternion target = Quaternion.Euler(0f, 0f, angle);
            visualRoot.localRotation = Quaternion.Slerp(visualRoot.localRotation, target, Time.deltaTime * 10f);
        }

        private void HandleSwipe(int direction)
        {
            if (_currentLane != _targetLane)
            {
                _bufferedDirection = direction;
                return;
            }

            int next = Mathf.Clamp(_targetLane + direction, 0, 2);
            _targetLane = next;
        }

        public void SetSpeedMultiplier(float value)
        {
            _speedMultiplier = Mathf.Max(0.2f, value);
        }

        public void SetBaseSpeed(float newBaseSpeed)
        {
            baseForwardSpeed = Mathf.Max(2f, newBaseSpeed);
        }

        public void OnMistake(int strike)
        {
            if (strike == 2)
            {
                StartCoroutine(TemporarySpeedPenalty());
            }
        }

        private IEnumerator TemporarySpeedPenalty()
        {
            float original = _speedMultiplier;
            _speedMultiplier *= 0.75f;
            yield return new WaitForSeconds(0.5f);
            _speedMultiplier = original;
        }
    }
}
