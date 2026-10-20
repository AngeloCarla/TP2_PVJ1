using UnityEngine;

public class TipMessage : MonoBehaviour
{
    // Script para dar consejos por consola
    [SerializeField] private string title; // Titulo
    [SerializeField] private string message; // Mensaje

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"<color=magenta>{title}</color><color=white>{message}</color>");
        }
        Destroy(gameObject,10f);
    }
}
