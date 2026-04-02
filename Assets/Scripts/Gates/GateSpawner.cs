using System.Collections.Generic;
using ColorRush.Color;
using ColorRush.Data;
using ColorRush.Difficulty;
using ColorRush.Runner;
using UnityEngine;

namespace ColorRush.Gates
{
    public class GateSpawner : MonoBehaviour
    {
        [SerializeField] private RunnerController runner;
        [SerializeField] private ColorStateController colorState;
        [SerializeField] private DifficultyDirector difficulty;
        [SerializeField] private GateConfigSO gateConfig;
        [SerializeField] private GateController gatePrefab;
        [SerializeField] private int prewarmCount = 16;
        [SerializeField] private float initialSpawnOffset = 25f;

        private readonly Queue<GateController> _pool = new();
        private readonly List<GateController> _active = new();
        private float _nextSpawnZ;

        private void Start()
        {
            for (int i = 0; i < prewarmCount; i++)
            {
                GateController g = Instantiate(gatePrefab, transform);
                g.gameObject.SetActive(false);
                _pool.Enqueue(g);
            }

            ResetRun();
        }

        private void Update()
        {
            if (ColorRush.Core.GameManager.Instance == null || !ColorRush.Core.GameManager.Instance.IsGameplayActive()) return;

            while (_nextSpawnZ < runner.transform.position.z + 90f)
            {
                SpawnOne();
            }

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                if (!_active[i].gameObject.activeSelf)
                {
                    _pool.Enqueue(_active[i]);
                    _active.RemoveAt(i);
                }
            }
        }

        public void ResetRun()
        {
            for (int i = 0; i < _active.Count; i++)
            {
                _active[i].gameObject.SetActive(false);
                _pool.Enqueue(_active[i]);
            }
            _active.Clear();

            _nextSpawnZ = runner.transform.position.z + initialSpawnOffset;
        }

        private void SpawnOne()
        {
            if (gateConfig.patterns.Count == 0) return;

            var gate = _pool.Count > 0 ? _pool.Dequeue() : Instantiate(gatePrefab, transform);
            var pattern = gateConfig.patterns[Random.Range(0, gateConfig.patterns.Count)];

            gate.transform.position = new Vector3(0f, 0f, _nextSpawnZ);
            gate.Initialize(pattern, runner, colorState);
            gate.gameObject.SetActive(true);
            _active.Add(gate);

            var phase = difficulty.CurrentPhase;
            float spacing = Random.Range(phase.minGateSpacing, phase.maxGateSpacing);
            _nextSpawnZ += Mathf.Max(6f, spacing);
        }
    }
}
