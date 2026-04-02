using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorRush.Data
{
    [Serializable]
    public struct ColorDefinition
    {
        public ColorId id;
        public string displayName;
        public Color color;
        public SymbolId symbol;
        public Sprite symbolSprite;
    }

    [Serializable]
    public struct DifficultyPhaseConfig
    {
        public DifficultyPhaseId phaseId;
        public float startTime;
        public float runnerSpeed;
        public float minGateSpacing;
        public float maxGateSpacing;
        [Range(0f, 1f)] public float blockedLaneChance;
        public float colorSwitchInterval;
        public float warningDuration;
        public bool useThreeColors;
    }

    [Serializable]
    public class GatePattern
    {
        public LaneId greenLane = LaneId.Left;
        public LaneId purpleLane = LaneId.Center;
        public LaneId orangeLane = LaneId.Right;
        public bool hasBlockedLane;
        public LaneId blockedLane;

        public LaneId GetLaneForColor(ColorId colorId)
        {
            return colorId switch
            {
                ColorId.Green => greenLane,
                ColorId.Purple => purpleLane,
                ColorId.Orange => orangeLane,
                _ => LaneId.Center
            };
        }
    }

    [CreateAssetMenu(menuName = "ColorRush/Color Config", fileName = "ColorConfig")]
    public class ColorConfigSO : ScriptableObject
    {
        public List<ColorDefinition> definitions = new();

        public ColorDefinition Get(ColorId id)
        {
            for (int i = 0; i < definitions.Count; i++)
            {
                if (definitions[i].id == id) return definitions[i];
            }
            return default;
        }
    }

    [CreateAssetMenu(menuName = "ColorRush/Difficulty Config", fileName = "DifficultyConfig")]
    public class DifficultyConfigSO : ScriptableObject
    {
        public List<DifficultyPhaseConfig> phases = new();

        public DifficultyPhaseConfig Resolve(float runTime)
        {
            DifficultyPhaseConfig active = phases[0];
            for (int i = 0; i < phases.Count; i++)
            {
                if (runTime >= phases[i].startTime)
                {
                    active = phases[i];
                }
            }
            return active;
        }
    }

    [CreateAssetMenu(menuName = "ColorRush/Gate Config", fileName = "GateConfig")]
    public class GateConfigSO : ScriptableObject
    {
        public List<GatePattern> patterns = new();
    }

    [CreateAssetMenu(menuName = "ColorRush/PowerUp Config", fileName = "PowerUpConfig")]
    public class PowerUpConfigSO : ScriptableObject
    {
        public float surgeDuration = 6f;
        public float surgeSpeedMultiplier = 1.2f;
        public float surgeScoreBonus = 1.5f;
        public float shieldDuration = 10f;
    }
}
