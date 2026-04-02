namespace ColorRush.Data
{
    public enum GameState
    {
        Boot,
        MainMenu,
        TapToPlay,
        Playing,
        Paused,
        GameOver
    }

    public enum LaneId
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public enum ColorId
    {
        Green = 0,
        Purple = 1,
        Orange = 2
    }

    public enum SymbolId
    {
        Triangle,
        Circle,
        Square
    }

    public enum DifficultyPhaseId
    {
        WarmUp,
        LockIn,
        Pressure,
        ChaosMastery
    }

    public enum PowerUpType
    {
        SurgeFuel,
        ShieldPulse
    }
}
