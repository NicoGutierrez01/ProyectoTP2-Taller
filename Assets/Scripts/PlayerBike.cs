using UnityEngine;
using UnityEngine.UI;

public class PlayerBike : MonoBehaviour
{
    [Header("Velocidad")]
    public float maxSpeed = 7f;
    public float acceleration = 15f;
    public float deceleration = 20f;

    [Header("Salto")]
    public float jumpForce = 8f;
    public float jumpTime = 0.25f; // tiempo máximo de salto mantenido

    [Header("Pirueta")]
    public float trickRotateSpeed = 300f;

    [Header("Botones")]
    public Button forwardButton;
    public Button backButton;
    public Button jumpButton;
    public Button trickButton;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isMovingForward = false;
    private bool isMovingBack = false;
    private bool isDoingTrick = false;
    private bool didTrick = false;

    private float currentVelocityX = 0f;

    // Salto sensible
    private bool isJumping = false;
    private float jumpTimeCounter = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Eventos de botones táctiles
        EventTriggerButton(forwardButton, () => isMovingForward = true, () => isMovingForward = false);
        EventTriggerButton(backButton,   () => isMovingBack = true,    () => isMovingBack = false);
        EventTriggerButton(jumpButton,   JumpStart, JumpEnd);
        EventTriggerButton(trickButton,  () => isDoingTrick = true, () => isDoingTrick = false);
    }

    void Update()
    {
        // Movimiento con aceleración
        float targetVelocityX = 0f;
        if (isMovingForward) targetVelocityX = maxSpeed;
        else if (isMovingBack) targetVelocityX = -maxSpeed;

        currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX,
            (targetVelocityX == 0 ? deceleration : acceleration) * Time.deltaTime);

        rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);

        // Piruetas en el aire
        if (!isGrounded && isDoingTrick)
        {
            transform.Rotate(0, 0, trickRotateSpeed * Time.deltaTime);
            didTrick = true;
        }

        // Salto mantenido
        JumpHold();
    }

    // ---------------------
    // SALTO PRESIONADO
    // ---------------------
    void JumpStart()
    {
        if (isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void JumpHold()
    {
        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    void JumpEnd()
    {
        isJumping = false;
    }

    // ---------------------
    // PIRUETAS
    // ---------------------
    void TrickSuccess()
    {
        Debug.Log("¡Pirueta exitosa!");
        // Acá podés sumar puntos, partículas, etc.
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // Evaluar pirueta
            if (didTrick)
            {
                float rotZ = transform.eulerAngles.z % 360;
                if (rotZ < 10f || rotZ > 350f)
                    TrickSuccess();
                else
                    Debug.Log("Cayó mal la pirueta...");

                didTrick = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    // ---------------------
    // BOTONES TÁCTILES
    // ---------------------
    void EventTriggerButton(Button btn, System.Action onPress, System.Action onRelease)
    {
        var trigger = btn.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        var entryDown = new UnityEngine.EventSystems.EventTrigger.Entry();
        entryDown.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
        entryDown.callback.AddListener((e) => onPress?.Invoke());
        trigger.triggers.Add(entryDown);

        if (onRelease != null)
        {
            var entryUp = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryUp.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
            entryUp.callback.AddListener((e) => onRelease?.Invoke());
            trigger.triggers.Add(entryUp);
        }
    }
}
