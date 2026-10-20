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

    [Header("Resaltado")]
    [SerializeField] private Color highlightColor = new Color(0.6f, 0.2f, 1f, 0.4f);
    private Renderer highlightedRenderer;
    private Color originalColor;


    private void Update()
    {
        if (hand == null)
            return;

        // Actualiza el origen y la direccion del Raycast
        origin = hand.TransformPoint(offset);
        dir = hand.forward;

        // Resalta el objeto al que estamos apuntando
        UpdateHighlight();

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

                EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();

                if (enemy != null)
                {
                    StunGun stunGun = GetComponent<StunGun>();

                    if (stunGun != null) stunGun.TryStun(enemy); // Aturde al enemigo
                    return;
                }
            }
        }
    }

    private void UpdateHighlight()
    {
        if (Physics.Raycast(origin, dir, out RaycastHit targetHit, rayDistance, layer)) // Comprueba el Raycast
        {
            Renderer targetRenderer = targetHit.collider.GetComponent<Renderer>();

            if (targetRenderer != highlightedRenderer)
            {
                if (highlightedRenderer != null) highlightedRenderer.material.color = originalColor; // Si no apunta vuelve al color original

                highlightedRenderer = targetRenderer;

                if (highlightedRenderer != null)
                {
                    originalColor = highlightedRenderer.material.color; // Guarda el color original
                    highlightedRenderer.material.color = highlightColor; // Resaltado
                }
            }
        }
        else
        {
            if (highlightedRenderer != null) highlightedRenderer.material.color = originalColor; // Color original
            highlightedRenderer = null;
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
