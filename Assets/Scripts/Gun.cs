using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private float speed; // Velocidad
    [SerializeField] private Transform point;

    [Header("Bala")]
    [SerializeField] private GameObject bullet; // Bala Enemigo
    [SerializeField] private Vector3 offset;

    public void Shoot()
    {
        if (bullet == null) return;

        // Instantiate (gameObject, posicion, rotacion)
        GameObject newBullet = Instantiate(bullet, point.position + offset, point.rotation); // Crea bala
        Destroy(newBullet, 4f); // Destruye la bala despues de un tiempo
    }
}
