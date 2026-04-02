using System.Collections;
using System.Collections.Generic;
using ColorRush.Core;
using ColorRush.Data;
using ColorRush.Difficulty;
using UnityEngine;

namespace ColorRush.Color
{
    public class ColorStateController : MonoBehaviour
    {
        [SerializeField] private ColorId startingColor = ColorId.Green;
        [SerializeField] private float fallbackWarningDuration = 1.25f;
        [SerializeField] private string warningText = "SWITCH INCOMING";

        private Coroutine _colorRoutine;
        private DifficultyDirector _difficultyDirector;

        public ColorId ActiveColor { get; private set; }
        public ColorId NextColor { get; private set; }

        public string WarningText => warningText;

        private void Awake()
        {
            _difficultyDirector = FindObjectOfType<DifficultyDirector>();
        }

        public void ResetRun()
        {
            if (_colorRoutine != null) StopCoroutine(_colorRoutine);

            ActiveColor = startingColor;
            NextColor = startingColor;
            GameSignals.OnColorShiftApplied?.Invoke(ActiveColor);
            _colorRoutine = StartCoroutine(ColorShiftLoop());
        }

        private IEnumerator ColorShiftLoop()
        {
            while (true)
            {
                var phase = _difficultyDirector != null ? _difficultyDirector.CurrentPhase : default;
                float interval = Mathf.Max(2f, phase.colorSwitchInterval <= 0 ? 4f : phase.colorSwitchInterval);
                float warningDuration = Mathf.Clamp(phase.warningDuration > 0 ? phase.warningDuration : fallbackWarningDuration, 0.75f, 1.5f);

                yield return new WaitForSeconds(interval - warningDuration);

                NextColor = GetNextColor(phase.useThreeColors);
                GameSignals.OnColorShiftWarningStarted?.Invoke(ActiveColor, NextColor, warningDuration);

                float t = warningDuration;
                while (t > 0f)
                {
                    t -= Time.deltaTime;
                    GameSignals.OnColorShiftWarningTick?.Invoke(Mathf.Clamp01(t / warningDuration));
                    yield return null;
                }

                ActiveColor = NextColor;
                GameSignals.OnColorShiftApplied?.Invoke(ActiveColor);
            }
        }

        private ColorId GetNextColor(bool useThreeColors)
        {
            List<ColorId> pool = new() { ColorId.Green, ColorId.Purple };
            if (useThreeColors) pool.Add(ColorId.Orange);

            ColorId selected = ActiveColor;
            int guard = 0;
            while (selected == ActiveColor && guard < 8)
            {
                selected = pool[Random.Range(0, pool.Count)];
                guard++;
            }
            return selected;
        }
    }
}
