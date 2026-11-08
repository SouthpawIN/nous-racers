using UnityEngine;

namespace NousRacers
{
    /// <summary>Rank by laps cleared, then by progress through the current lap.</summary>
    public class PositionTracker : MonoBehaviour
    {
        public int lap = 1;
        public int nextCheckpoint;
        public float distanceAlongTrack;

        public float Progress => lap * 10000f + nextCheckpoint * 100f + distanceAlongTrack;
    }
}
