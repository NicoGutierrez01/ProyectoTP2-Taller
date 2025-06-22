using UnityEngine;
using UnityEngine.UI;

public class MostrarEstrellas : MonoBehaviour
{
    [Header("Estrellas llenas que se activan")]
    public Image[] estrellasLlenas; // Las negras que aparecen encima

    [Header("Estrellas vacías que siempre están visibles")]
    public Image[] estrellasVacias; // Las de fondo, siempre activas

    void Start()
    {
        int cantidadEstrellas = PlayerPrefs.GetInt("EstrellasNivel", 0);

        for (int i = 0; i < estrellasVacias.Length; i++)
        {
            estrellasVacias[i].enabled = true; // Siempre visibles
        }

        for (int i = 0; i < estrellasLlenas.Length; i++)
        {
            estrellasLlenas[i].enabled = i < cantidadEstrellas; // Mostrar solo las ganadas
        }
    }
}
