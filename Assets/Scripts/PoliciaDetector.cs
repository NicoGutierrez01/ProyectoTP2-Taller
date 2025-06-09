using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliciaDetector : MonoBehaviour
{
    public float velocidadDeteccionMinima = 4f;
    public float velocidadPersecucion = 3f;
    public float tiempoDePersecucion = 5f;

    private Transform jugador;
    private bool seguirJugador = false;
    private float tiempoRestante = 0f;

    void Update()
    {
        if (seguirJugador && jugador != null)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante > 0)
            {
                Vector2 direccion = (jugador.position - transform.position).normalized;
                transform.position += (Vector3)direccion * velocidadPersecucion * Time.deltaTime;
            }
            else
            {
                seguirJugador = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float velocidadX = Mathf.Abs(rb.velocity.x);

                if (velocidadX > velocidadDeteccionMinima)
                {
                    jugador = other.transform;
                    seguirJugador = true;
                    tiempoRestante = tiempoDePersecucion;

                    Debug.Log("¡Policía empieza a perseguirte por ir rápido!");
                }
                else
                {
                    Debug.Log("Ibas lento, el policía no te sigue.");
                }
            }
        }
    }

        void OnCollisionEnter2D(Collision2D collision)
    {
        if (seguirJugador && collision.collider.CompareTag("Player"))
        {
            Debug.Log("¡La policía te atrapó!");
            // Acá terminás el nivel o cargás escena
            SceneManager.LoadScene("Menu");
        }
    }

}
