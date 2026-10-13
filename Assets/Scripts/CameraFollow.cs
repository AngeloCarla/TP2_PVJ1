using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camara")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        // Seguimiento de la camara
        transform.position = player.position + offset;
    }
}
