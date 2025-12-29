using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    [Header("animator")]
    public Animator animator;
    
    public GameObject attackHitbox;
    public float attackDuration = 0.3f;

    private bool isAttacking;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);
        isAttacking = false;
    }
}