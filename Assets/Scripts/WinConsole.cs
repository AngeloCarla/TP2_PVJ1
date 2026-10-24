using UnityEngine;
using System.Collections;

public class WinConsole : MonoBehaviour
{

    [SerializeField] private GameObject pantalla;
    [SerializeField] private GameObject[] explosions;

    private bool missionCompleted;
    private Renderer rend;

    private void Awake()
    {
        if (pantalla != null) rend = pantalla.GetComponent<Renderer>();
    }

    public void WinPlayer()
    {
        if (missionCompleted) return;

        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.HasRequiredData())
        {
            return;
        }

        missionCompleted = true;

        // La pantalla se vuelve verde
        if (rend != null) rend.material.color = Color.green;

        foreach (GameObject explosion in explosions)
        {
            if (explosion != null)
                explosion.SetActive(true);
        }
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.Win();
    }
}
