using System.Collections;
using ColorRush.Chaser;
using ColorRush.Core;
using ColorRush.Data;
using ColorRush.Runner;
using ColorRush.Scoring;
using UnityEngine;

namespace ColorRush.PowerUps
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField] private RunnerController runner;
        [SerializeField] private ScoreManager score;
        [SerializeField] private ChaserController chaser;
        [SerializeField] private PowerUpConfigSO config;

        private bool _surgeActive;
        private bool _shieldActive;
        private int _mistakeProtectionCharges;
        private Coroutine _surgeRoutine;

        public void ResetRun()
        {
            _surgeActive = false;
            _shieldActive = false;
            _mistakeProtectionCharges = 0;

            if (_surgeRoutine != null)
            {
                StopCoroutine(_surgeRoutine);
                _surgeRoutine = null;
            }

            runner.SetSpeedMultiplier(1f);
        }

        public void TriggerSurgeFuel()
        {
            if (_surgeRoutine != null) StopCoroutine(_surgeRoutine);
            _surgeRoutine = StartCoroutine(SurgeRoutine());
        }

        public void TriggerShieldPulse()
        {
            _shieldActive = true;
            _mistakeProtectionCharges += 1;
            GameSignals.OnPowerUpStateChanged?.Invoke(PowerUpType.ShieldPulse, true);
        }

        public bool TryConsumeMistakeProtection()
        {
            if (_mistakeProtectionCharges <= 0) return false;

            _mistakeProtectionCharges--;
            if (_mistakeProtectionCharges == 0)
            {
                _shieldActive = false;
                GameSignals.OnPowerUpStateChanged?.Invoke(PowerUpType.ShieldPulse, false);
            }

            return true;
        }

        private IEnumerator SurgeRoutine()
        {
            _surgeActive = true;
            _mistakeProtectionCharges += 1;
            runner.SetSpeedMultiplier(config.surgeSpeedMultiplier);
            chaser.PushBack(4f);
            GameSignals.OnPowerUpStateChanged?.Invoke(PowerUpType.SurgeFuel, true);

            yield return new WaitForSeconds(config.surgeDuration);

            _surgeActive = false;
            runner.SetSpeedMultiplier(1f);
            GameSignals.OnPowerUpStateChanged?.Invoke(PowerUpType.SurgeFuel, false);
        }
    }
}
