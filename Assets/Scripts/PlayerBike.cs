using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerBike : MonoBehaviour
{
    public CargaManager cargaManager;

    [Header("Velocidad")]
    public float acceleration = 2f;
    public float maxSpeed = 12f;
    public float decelerationTime = 1.5f;

    [Header("Willy")]
    public float willyRotateSpeed = 100f;

    [Header("Botones")]
    public Button brakeButton;
    public Button willyButton;

    private Rigidbody2D rb;

    private bool isGrounded = true;
    private bool isBraking = false;
    private bool isDoingWilly = false;

    private float currentVelocityX = 0f;
    private float velocidadCaidaAnterior = 0f;
    private bool haCaido = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.inertia = 1f;
        rb.angularDrag = 5f;

        EventTriggerButton(brakeButton, () => isBraking = true, () => isBraking = false);
        EventTriggerButton(willyButton, () => isDoingWilly = true, () => isDoingWilly = false);
    }

    void Update()
    {
        if (haCaido) return; // No hacer nada si ya se cayó

        float targetVelocityX = isBraking ? 0f : maxSpeed;

        if (isBraking)
        {
            float speedChangePerSecond = maxSpeed / decelerationTime;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, 0f, speedChangePerSecond * Time.deltaTime);
        }
        else
        {
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, maxSpeed, acceleration * Time.deltaTime);
        }

        rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);

        // Willy
        if (isGrounded && isDoingWilly)
        {
            transform.Rotate(0, 0, willyRotateSpeed * Time.deltaTime);
        }

        // Guardar velocidad vertical mientras cae
        if (rb.velocity.y < velocidadCaidaAnterior)
        {
            velocidadCaidaAnterior = rb.velocity.y;
        }

        // Detectar caída por dar vuelta
        float rotZ = transform.eulerAngles.z;
        if (!haCaido && (rotZ > 110f && rotZ < 250f))
        {
            haCaido = true;

            // Detener movimiento y rotación
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;

            StartCoroutine(ReiniciarNivelTrasCaida());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }

        // Soltar carga solo si la caída fue fuerte
        if (velocidadCaidaAnterior < -7f)
        {
            cargaManager.SoltarTodos();
        }

        velocidadCaidaAnterior = 0f;

        if (!isDoingWilly)
        {
            StartCoroutine(ResetRotation());
        }
    }

    IEnumerator ResetRotation()
    {
        float elapsed = 0f;
        float duration = 0.3f;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
    }

    void EventTriggerButton(Button btn, System.Action onPress, System.Action onRelease)
    {
        var trigger = btn.gameObject.AddComponent<EventTrigger>();

        var entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entryDown.callback.AddListener((e) => onPress?.Invoke());
        trigger.triggers.Add(entryDown);

        if (onRelease != null)
        {
            var entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            entryUp.callback.AddListener((e) => onRelease?.Invoke());
            trigger.triggers.Add(entryUp);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            cargaManager.AgregarPaquete();
            Destroy(other.gameObject);
        }
    }

    IEnumerator ReiniciarNivelTrasCaida()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
}
