using ColorRush.Color;
using ColorRush.Core;
using ColorRush.Data;
using ColorRush.Runner;
using UnityEngine;

namespace ColorRush.Gates
{
    public class GateController : MonoBehaviour
    {
        [SerializeField] private float resolveDepth = 0f;

        private GatePattern _pattern;
        private RunnerController _runner;
        private ColorStateController _colorState;
        private bool _resolved;

        public void Initialize(GatePattern pattern, RunnerController runner, ColorStateController colorState)
        {
            _pattern = pattern;
            _runner = runner;
            _colorState = colorState;
            _resolved = false;
        }

        private void Update()
        {
            if (_resolved || _runner == null || _colorState == null) return;

            if (_runner.transform.position.z >= transform.position.z + resolveDepth)
            {
                Resolve();
            }

            if (_runner.transform.position.z - transform.position.z > 20f)
            {
                gameObject.SetActive(false);
            }
        }

        private void Resolve()
        {
            _resolved = true;

            ColorId active = _colorState.ActiveColor;
            LaneId requiredLane = _pattern.GetLaneForColor(active);
            LaneId runnerLane = (LaneId)_runner.CurrentLaneIndex;

            bool blockedFail = _pattern.hasBlockedLane && _pattern.blockedLane == runnerLane;
            bool success = !blockedFail && runnerLane == requiredLane;

            GameSignals.OnGateResolved?.Invoke(success);
        }
    }
}
