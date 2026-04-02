using System;
using ColorRush.Data;

namespace ColorRush.Core
{
    public static class GameSignals
    {
        public static Action<GameState> OnGameStateChanged;
        public static Action<ColorId, ColorId, float> OnColorShiftWarningStarted;
        public static Action<float> OnColorShiftWarningTick;
        public static Action<ColorId> OnColorShiftApplied;
        public static Action<bool> OnGateResolved;
        public static Action<int, float> OnScoreChanged;
        public static Action<int, float> OnStreakChanged;
        public static Action<int> OnStrikeChanged;
        public static Action OnRunStarted;
        public static Action OnRunEnded;
        public static Action<PowerUpType, bool> OnPowerUpStateChanged;
    }
}
