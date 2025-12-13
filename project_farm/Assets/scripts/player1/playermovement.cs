using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float dashSpeed = 50f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.5f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 15f;
    public float dashStaminaCost = 20f;


    private Rigidbody rb;
    private Vector3 moveInput;

    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        // รับ input เดิม
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(x, 0, z).normalized;

        // การฟื้น Stamina
        if (!isDashing && currentStamina < maxStamina)
        {
             currentStamina += staminaRegenRate * Time.deltaTime;
             currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        // Dash จะทำงาน **เฉพาะตอนที่ผู้เล่นกดทิศทาง**
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
        if (currentStamina < dashStaminaCost) return;  // ❌ Stamina ไม่พอ

    currentStamina -= dashStaminaCost;  // ✔ หัก Stamina
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector3 direction = moveInput;
        if (direction == Vector3.zero)
            direction = transform.forward; // Dash ไปด้านหน้าถ้าไม่กดทิศทาง

        rb.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector3.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
