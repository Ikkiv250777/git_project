using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider hpSlider;
    public Image hpFill;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        UpdateHPColor();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        hpSlider.value = currentHealth;
        UpdateHPColor();

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHPColor()
    {
        float percent = currentHealth / maxHealth;

        if (percent > 0.6f)
            hpFill.color = Color.green;
        else if (percent > 0.3f)
            hpFill.color = Color.yellow;
        else
            hpFill.color = Color.red;
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player Died");

        GetComponent<PlayerMovement>().enabled = false;
    }
    void Update()
{
    // กด H เพื่อทดสอบโดนดาเมจ
    if (Input.GetKeyDown(KeyCode.H))
    {
        TakeDamage(10f);
    }
}

}
