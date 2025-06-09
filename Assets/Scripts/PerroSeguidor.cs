using UnityEngine;

public class PerroSeguidor : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 3f;
    public float distanciaMaxima = 10f;
    public float tiempoDePersecucion = 5f;

    private bool seguirJugador = false;
    private float tiempoRestante = 0f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (seguirJugador && jugador != null)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante > 0)
            {
                float distancia = Vector2.Distance(transform.position, jugador.position);

                if (distancia < distanciaMaxima)
                {
                    Vector2 direccion = (jugador.position - transform.position).normalized;
                    transform.position += (Vector3)direccion * velocidad * Time.deltaTime;
                }
            }
            else
            {
                seguirJugador = false;

                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !seguirJugador)
        {
            jugador = other.transform;
            seguirJugador = true;
            tiempoRestante = tiempoDePersecucion;

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // reproducir ladrido
            }
        }
    }
}
