using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform hand; // Desde donde apunta el raycast (La mano)
    [SerializeField] private float rayDistance; // Largo del rayo
    [SerializeField] private LayerMask layer; // Capa que detecta
    [SerializeField] private Vector3 offset; 
    private Vector3 origin; // Punto de inicio
    private Vector3 dir; // Direccion a donde mira
    private RaycastHit hit; // Guarda informacion del impacto
    private bool isInteractable; // Interactuable


    private void Update()
    {
        if (hand == null)
            return;

        if (Input.GetKeyDown(KeyCode.E)) // Al presionar E
        {
            isInteractable = false;

            origin = hand.TransformPoint(offset); 
            dir = hand.forward;

            // Lanza el rayo
            if (Physics.Raycast(origin, dir, out hit, rayDistance, layer))
            {
                InformationData data = hit.collider.GetComponent<InformationData>();
                if (data != null)
                {
                    data.Collect(); // Recolecta
                    return;
                }

                DoorConsole console = hit.collider.GetComponent<DoorConsole>();
                if (console != null)
                {
                    console.DoorActivate();// Activa la puerta
                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja el Raycast (Test)
        if (hand == null) return;

        Vector3 rayOrigin = hand.TransformPoint(offset);
        Vector3 rayDir = hand.forward;

        Gizmos.color = isInteractable ? Color.green : Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDir * rayDistance);
    }
}
