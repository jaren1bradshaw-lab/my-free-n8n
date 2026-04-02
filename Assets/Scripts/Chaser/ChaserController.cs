using ColorRush.Core;
using UnityEngine;

namespace ColorRush.Chaser
{
    public class ChaserController : MonoBehaviour
    {
        [SerializeField] private Transform runner;
        [SerializeField] private float defaultDistance = 18f;
        [SerializeField] private float minDistance = 2.8f;
        [SerializeField] private float smoothSpeed = 4f;

        private float _distanceBehind;

        public void ResetRun()
        {
            _distanceBehind = defaultDistance;
            SnapToTarget();
        }

        private void Update()
        {
            if (runner == null) return;
            Vector3 target = runner.position - Vector3.forward * _distanceBehind;
            target.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothSpeed);
        }

        public void OnMistake(int strike)
        {
            _distanceBehind -= strike == 1 ? 3.5f : 5f;
            _distanceBehind = Mathf.Max(minDistance, _distanceBehind);

            if (_distanceBehind <= minDistance + 0.05f)
            {
                GameManager.Instance.EndRun();
            }
        }

        public void OnPerfectGate(int streak)
        {
            if (streak > 0 && streak % 6 == 0)
            {
                _distanceBehind = Mathf.Min(defaultDistance, _distanceBehind + 1.25f);
            }
        }

        public void PushBack(float amount)
        {
            _distanceBehind = Mathf.Min(defaultDistance + 4f, _distanceBehind + amount);
        }

        private void SnapToTarget()
        {
            if (runner == null) return;
            transform.position = runner.position - Vector3.forward * _distanceBehind;
        }
    }
}
