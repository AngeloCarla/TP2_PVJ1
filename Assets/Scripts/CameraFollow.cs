using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camara")]
    [SerializeField] private GameObject target_p; // Jugador
    [SerializeField] private float distance = 5.0f; // Distancia del jugador a la camara
    [SerializeField] private Vector3 Offset = new Vector3(0, 1.5f, 0);

    [Header("Sensibilidad y limites")]
    [SerializeField] private float sensitivityX = 4.0f; // Sensibilidad de giro horizontal
    [SerializeField] private float sensitivityY = 2.0f; // Sensibilidad de giro vertical
    [SerializeField] private float minYAngle = -20.0f; // Limite inferior 
    [SerializeField] private float maxYAngle = 60.0f; // Limite superior

    private float currentX = 0.0f; // Giro horizontal
    private float currentY = 20.0f; // Giro vertical

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
        // Cursor.visible = false; // Oculta el cursor

        if (target_p != null)
        {
            currentX = target_p.transform.eulerAngles.y; // Prientacion inicial de la camara
        }
    }

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle); // Limite de la camara
    }

    void LateUpdate()
    {
        if (target_p == null) return;

        Quaternion camRotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 positionOffset = camRotation * new Vector3(0, 0, -distance); // Rota el desplazamiento para quede detras del jugador

        transform.position = target_p.transform.position + Offset + positionOffset; // Sigue al jugador y cambia el angulo del mouse
        transform.LookAt(target_p.transform.position + Offset); // Orienta la camara a la posicion correcta

        target_p.GetComponent<PlayerMovement>().RotateCharacter(currentX); // Gira al personaje
    }
}
