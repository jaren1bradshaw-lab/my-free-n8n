using ColorRush.Core;
using ColorRush.Data;
using ColorRush.Runner;
using UnityEngine;

namespace ColorRush.Difficulty
{
    public class DifficultyDirector : MonoBehaviour
    {
        [SerializeField] private DifficultyConfigSO config;
        [SerializeField] private RunnerController runner;

        public DifficultyPhaseConfig CurrentPhase { get; private set; }
        public float RunTime { get; private set; }

        public void ResetRun()
        {
            RunTime = 0f;
            CurrentPhase = config.phases[0];
            ApplyPhase(CurrentPhase);
        }

        private void Update()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsGameplayActive()) return;

            RunTime += Time.deltaTime;
            var resolved = config.Resolve(RunTime);
            if (!resolved.Equals(CurrentPhase))
            {
                CurrentPhase = resolved;
                ApplyPhase(CurrentPhase);
            }
        }

        private void ApplyPhase(DifficultyPhaseConfig phase)
        {
            runner.SetBaseSpeed(phase.runnerSpeed);
        }
    }
}
