using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed; // Velocidad
    private float h, v; // Horizontal, Vertical
    private Vector3 dir; // Direccion

    [Header("Salto")]
    [SerializeField] private float jumpForce; // Fuerza de Salto
    private Rigidbody rb; // Rigidbody

    [Header("Raycast")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayDistance; // Cuanto mide el rayo
    [SerializeField] private LayerMask groundLayer; // Capa que detecta

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        dir = new Vector3(h, 0, v);

        // Salto
        CheckGround();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Vector3 movement = dir.normalized * speed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + transform.TransformDirection(movement));
            //transform.Translate(movement); // Movimiento
        }
    }

    private void CheckGround()
    {
        // Raycast
        Vector3 origin = transform.position; // Origen del Rayo

        // Lanza el rayo
        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);

        // Dibujar el raycast (Test)
        Color ColorRay = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector3.down * rayDistance, ColorRay);
    }
}
