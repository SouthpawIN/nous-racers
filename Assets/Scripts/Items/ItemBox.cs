using UnityEngine;

namespace NRCU.Kart
{
    /// <summary>A row of boxes; each respawns a few seconds after being taken.</summary>
    public class ItemBox : MonoBehaviour
    {
        public float respawnSeconds = 4f;
        public ItemDefinition[] table;

        float cooldown;

        void Update()
        {
            if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
                gameObject.SetActive(cooldown <= 0f);
            }
        }

        public ItemDefinition Take()
        {
            cooldown = respawnSeconds;
            gameObject.SetActive(false);
            return table.Length == 0 ? null : table[Random.Range(0, table.Length)];
        }
    }
}
