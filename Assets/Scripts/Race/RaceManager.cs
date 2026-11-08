using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NRCU.Kart
{
    /// <summary>Countdown, laps, finish order. Cup points 15/12/10/8/6/4/2/1.</summary>
    public class RaceManager : MonoBehaviour
    {
        public int laps = 3;
        public float countdown = 3f;
        public Checkpoint[] trackOrder;

        readonly List<PositionTracker> racers = new();
        bool started;
        float timer;

        static readonly int[] CupPoints = { 15, 12, 10, 8, 6, 4, 2, 1 };

        public void Register(PositionTracker tracker) => racers.Add(tracker);

        void Update()
        {
            if (!started)
            {
                timer += Time.deltaTime;
                if (timer >= countdown) started = true;
            }
        }

        public bool Racing => started;

        public List<PositionTracker> Standings() =>
            racers.OrderByDescending(r => r.Progress).ToList();

        public int PointsFor(int place) => place < CupPoints.Length ? CupPoints[place] : 0;

        public bool TryCompleteLap(PositionTracker tracker, Checkpoint gate)
        {
            if (gate.index != tracker.nextCheckpoint) return false;
            tracker.nextCheckpoint++;
            if (tracker.nextCheckpoint >= trackOrder.Length)
            {
                tracker.nextCheckpoint = 0;
                tracker.lap++;
                return tracker.lap > laps;
            }
            return false;
        }
    }
}
