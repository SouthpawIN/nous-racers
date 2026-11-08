using UnityEngine;

namespace NousRacers
{
    /// <summary>Returns a kart that fell off to the last checkpoint it cleared.</summary>
    public class Respawn : MonoBehaviour
    {
        public float killPlaneY = -20f;
        Checkpoint last;
        Rigidbody body;

        void Awake() => body = GetComponent<Rigidbody>();

        public void Mark(Checkpoint checkpoint) => last = checkpoint;

        void Update()
        {
            if (transform.position.y < killPlaneY && last != null)
            {
                transform.SetPositionAndRotation(last.transform.position + Vector3.up * 1.5f,
                                                 last.transform.rotation);
                body.linearVelocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }
    }
}
