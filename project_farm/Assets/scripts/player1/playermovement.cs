using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Dash")]
    public float dashSpeed = 50f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.5f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 15f;
    public float dashStaminaCost = 20f;

    [Header("Stamina UI")]
    public Slider staminaSlider;
    public Image staminaFill;

    [Header("Stamina Colors")]
    public Color highStaminaColor = Color.cyan;
    public Color midStaminaColor = Color.yellow;
    public Color lowStaminaColor = Color.red;

    private Rigidbody rb;
    private Vector3 moveInput;

    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ตั้งค่า Stamina เริ่มต้น
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;

        UpdateStaminaUI();
    }

    void Update()
    {
        // รับ input เดิน
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(x, 0, z).normalized;

        // ฟื้น Stamina
        if (!isDashing && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            UpdateStaminaUI();
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && moveInput != Vector3.zero)
        {
            TryDash();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(transform.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void TryDash()
    {
        if (!canDash || isDashing) return;
        if (currentStamina < dashStaminaCost) return;

        // ใช้ Stamina
        currentStamina -= dashStaminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        UpdateStaminaUI();
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector3 direction = moveInput;
        if (direction == Vector3.zero)
            direction = transform.forward;

        rb.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector3.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina;

        float percent = currentStamina / maxStamina;

        if (percent > 0.6f)
        {
            staminaFill.color = highStaminaColor;
        }
        else if (percent > 0.3f)
        {
            staminaFill.color = midStaminaColor;
        }
        else
        {
            staminaFill.color = lowStaminaColor;
        }
    }
}
