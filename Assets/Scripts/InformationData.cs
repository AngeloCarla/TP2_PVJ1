using UnityEngine;

public class InformationData : MonoBehaviour
{
    [SerializeField] private int points; // Puntos
    public void Collect()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null) return;

        if (gameManager.HasRequiredData()) return; // Todos los datos requeridos esta recogidos

        gameManager.CollectData(points); // Se recolecta y suma puntos 
        Destroy(gameObject); // Eliminamos el fragmento del escenario
    }
}
