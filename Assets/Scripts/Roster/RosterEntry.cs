using System;
using UnityEngine;

namespace NRCU.Kart
{
    [Serializable]
    public class RosterEntry
    {
        public string slug;
        public string display;
        public string portrait;
        public string weight;          // light | medium | heavy
        public Stats stats;
        public string credit;
        public string source_note;
        public bool locked;
        public string bio_hook;

        public float Mass => weight switch
        {
            "light" => 150f,
            "heavy" => 220f,
            _ => 180f,
        };
    }

    [Serializable]
    public class Stats
    {
        public int speed;
        public int accel;
        public int handling;
        public int weight;

        public float MotorForce => 1600f + speed * 90f;
        public float SteerAngle => 20f + handling * 1.6f;
    }

    [CreateAssetMenu(menuName = "NRCU/Roster")]
    public class RosterAsset : ScriptableObject
    {
        public RosterEntry[] characters;
    }
}
