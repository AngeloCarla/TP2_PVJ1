using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 50f; // Velocidad
    private float h, v; // Horizontal, Vertical
    private Vector3 dir; // Direccion

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        dir = new Vector3(h, 0, v);

        Vector3 movement = dir.normalized * speed * Time.deltaTime;

        transform.Translate(movement); // Movimiento
    }
}
