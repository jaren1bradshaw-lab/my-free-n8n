using UnityEngine;

namespace ColorRush.PowerUps
{
    public class EnergyRushPowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpController controller;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            controller.TriggerSurgeFuel();
            gameObject.SetActive(false);
        }
    }
}
