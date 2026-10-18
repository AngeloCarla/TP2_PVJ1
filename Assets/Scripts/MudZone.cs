using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MudZone : MonoBehaviour
{
    private PlayerMovement player;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            player.SetInMud(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player = other.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            player.SetInMud(false);
        }
    }
}
