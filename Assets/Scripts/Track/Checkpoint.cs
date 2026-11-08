using UnityEngine;

namespace NousRacers
{
    /// <summary>One gate. Laps only count when every gate is hit in order.</summary>
    public class Checkpoint : MonoBehaviour
    {
        public int index;
        public float width = 14f;

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 1f, 0.6f, 0.35f);
            Gizmos.DrawCube(transform.position, new Vector3(width, 3f, 0.4f));
        }
    }
}
