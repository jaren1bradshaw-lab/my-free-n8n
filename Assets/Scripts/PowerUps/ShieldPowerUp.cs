using UnityEngine;

namespace ColorRush.PowerUps
{
    public class ShieldPowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpController controller;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            controller.TriggerShieldPulse();
            gameObject.SetActive(false);
        }
    }
}
