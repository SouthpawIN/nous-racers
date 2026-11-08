using UnityEngine;
using UnityEngine.InputSystem;

namespace NRCU.Kart
{
    /// <summary>
    /// Arcade kart: grip-based turning, hop-drift with three mini-turbo tiers.
    /// Feel first — content comes after the drift feels right.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class KartController : MonoBehaviour
    {
        [Header("Driving")]
        public float motorForce = 2200f;
        public float maxSpeed = 42f;
        public float steerAngle = 28f;
        public float normalGrip = 22f;
        public float driftGrip = 8f;

        [Header("Drift")]
        public float hopImpulse = 320f;
        public float blueAt = 0.8f;
        public float orangeAt = 1.6f;
        public float purpleAt = 2.6f;
        public float[] boostForce = { 800f, 1200f, 1700f };

        [Header("Wheels")]
        public Transform[] wheelVisuals;
        public float wheelRadius = 0.35f;
        public LayerMask groundMask = ~0;

        Rigidbody body;
        KartInputFrame input;
        bool drifting;
        bool grounded;
        float driftCharge;
        float boostTimer;
        float boostPower;

        void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.mass = 180f;
            body.linearDamping = 0.2f;
            body.angularDamping = 4f;
        }

        public void SetInput(in KartInputFrame frame) => input = frame;

        void FixedUpdate()
        {
            grounded = Physics.Raycast(transform.position, -transform.up, wheelRadius + 0.25f, groundMask);
            float speed = Vector3.Dot(body.linearVelocity, transform.forward);

            if (grounded)
            {
                Vector3 force = transform.forward * input.throttle * motorForce;
                if (boostTimer > 0f)
                {
                    force += transform.forward * boostPower;
                    boostTimer -= Time.fixedDeltaTime;
                }
                body.AddForce(force);

                if (speed > maxSpeed)
                {
                    body.linearVelocity = Vector3.ClampMagnitude(body.linearVelocity, maxSpeed);
                }

                float grip = drifting ? driftGrip : normalGrip;
                float lateral = Vector3.Dot(body.linearVelocity, transform.right);
                body.AddForce(-transform.right * lateral * grip);

                float steer = input.steer * steerAngle * Mathf.Clamp01(Mathf.Abs(speed) / 8f);
                transform.Rotate(0f, steer * Time.fixedDeltaTime * (drifting ? 1.6f : 1f), 0f);
            }

            UpdateDrift(grounded);
            foreach (var wheel in wheelVisuals)
            {
                if (wheel != null) wheel.Rotate(Vector3.right, speed * Time.fixedDeltaTime / wheelRadius * Mathf.Rad2Deg);
            }
        }

        void UpdateDrift(bool onGround)
        {
            if (onGround && input.driftPressed && !drifting)
            {
                drifting = true;
                driftCharge = 0f;
                body.AddForce(transform.up * hopImpulse, ForceMode.Impulse);
            }
            if (drifting && !input.driftHeld)
            {
                ReleaseDrift();
            }
            if (drifting)
            {
                driftCharge += Time.fixedDeltaTime;
            }
        }

        void ReleaseDrift()
        {
            drifting = false;
            if (driftCharge >= purpleAt) { boostTimer = 1.4f; boostPower = boostForce[2]; }
            else if (driftCharge >= orangeAt) { boostTimer = 1.1f; boostPower = boostForce[1]; }
            else if (driftCharge >= blueAt) { boostTimer = 0.8f; boostPower = boostForce[0]; }
            driftCharge = 0f;
        }
    }

    public struct KartInputFrame
    {
        public float throttle;
        public float steer;
        public bool driftPressed;
        public bool driftHeld;
        public bool firePressed;
    }
}
