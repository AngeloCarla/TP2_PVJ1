using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform hand; // Desde donde apunta el raycast (La mano)
    [SerializeField] private float rayDistance; // Largo del rayo
    [SerializeField] private Vector3 offset;
    private Vector3 origin; // Punto de inicio
    private Vector3 dir; // Direccion a donde mira
    private RaycastHit hit; // Guarda informacion del impacto
    private bool isDetectable; // Detectable


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Al presionar E
        {
            isDetectable = false;

            origin = hand.TransformPoint(offset); 
            dir = hand.forward;

            // Lanza el rayo
            if (Physics.Raycast(origin, dir, out hit, rayDistance))
            {
                isDetectable = hit.collider.CompareTag("Detectable"); // Comprueba si el objeto alcanzado tiene el tag Detectable
                Debug.Log("Detectado");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja el Raycast (Test)
        if (hand == null) return;

        Vector3 rayOrigin = hand.TransformPoint(offset);
        Vector3 rayDir = hand.forward;

        Gizmos.color = isDetectable ? Color.green : Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDir * rayDistance);
    }
}
