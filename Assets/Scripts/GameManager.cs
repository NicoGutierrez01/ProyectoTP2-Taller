// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int estrellasNivel = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para que sobreviva entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarEstrella()
    {
        estrellasNivel++;
        Debug.Log("Estrellas recolectadas: " + estrellasNivel);
    }

    public void ReiniciarEstrellas()
    {
        estrellasNivel = 0;
    }
}
