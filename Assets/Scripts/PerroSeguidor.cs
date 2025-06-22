using UnityEngine;
using System.Collections;

public class PerroSeguidor : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 3f;
    public float distanciaMaxima = 10f;
    public float tiempoDePersecucion = 5f;
    public float fadeDuration = 1f;

    private bool seguirJugador = false;
    private float tiempoRestante = 0f;

    private AudioSource audioSource;
    private bool fadingOut = false;

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
                if (tiempoRestante <= fadeDuration && !fadingOut)
                {
                    fadingOut = true;
                    StartCoroutine(FadeOutSound());
                }

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
                audioSource.volume = 1f;
                audioSource.Play();
                fadingOut = false;
            }
        }
    }

    private IEnumerator FadeOutSound()
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        fadingOut = false;
    }
}
