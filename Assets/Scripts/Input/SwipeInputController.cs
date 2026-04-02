using System;
using UnityEngine;

namespace ColorRush.Runner
{
    public class SwipeInputController : MonoBehaviour
    {
        [SerializeField] private float minSwipeDistance = 45f;
        [SerializeField] private float maxSwipeTime = 0.4f;

        public event Action<int> OnSwipeHorizontal; // -1 left, +1 right

        private Vector2 _startPos;
        private float _startTime;
        private bool _inputEnabled;

        public void EnableInput(bool enabled) => _inputEnabled = enabled;

        private void Update()
        {
            if (!_inputEnabled) return;

#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouse();
#else
            HandleTouch();
#endif
        }

        private void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPos = Input.mousePosition;
                _startTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0))
            {
                EvaluateSwipe((Vector2)Input.mousePosition);
            }
        }

        private void HandleTouch()
        {
            if (Input.touchCount == 0) return;
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startPos = touch.position;
                _startTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                EvaluateSwipe(touch.position);
            }
        }

        private void EvaluateSwipe(Vector2 endPos)
        {
            float elapsed = Time.time - _startTime;
            if (elapsed > maxSwipeTime) return;

            Vector2 delta = endPos - _startPos;
            if (Mathf.Abs(delta.x) < minSwipeDistance) return;

            int direction = delta.x > 0 ? 1 : -1;
            OnSwipeHorizontal?.Invoke(direction);
        }
    }
}
