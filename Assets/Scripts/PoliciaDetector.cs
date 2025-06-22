using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PoliciaDetector : MonoBehaviour
{
    public float velocidadDeteccionMinima = 4f;
    public float velocidadPersecucion = 3f;
    public float tiempoDePersecucion = 5f;
    public float fadeDuration = 1f;

    private Transform jugador;
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

                    if (audioSource != null && !audioSource.isPlaying)
                    {
                        audioSource.volume = 1f;
                        audioSource.Play();
                        fadingOut = false;
                    }

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
            SceneManager.LoadScene("Menu");
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
