using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private string NivelEstrellas= "NivelEstrellas"; // Escena que muestra las estrellas

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener CargaManager del jugador
            CargaManager cargaManager = other.GetComponent<PlayerBike>()?.cargaManager;
            if (cargaManager != null)
            {
                int cantidad = cargaManager.CantidadPaquetes();
                int estrellas = CalcularEstrellas(cantidad);

                PlayerPrefs.SetInt("EstrellasNivel", estrellas);
                PlayerPrefs.Save();
            }

            SceneManager.LoadScene(NivelEstrellas);
        }
    }

    private int CalcularEstrellas(int cantidad)
    {
        if (cantidad >= 5) return 3;
        if (cantidad >= 3) return 2;
        if (cantidad >= 1) return 1;
        return 0;
    }
}
