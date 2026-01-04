using System.Collections;
using UnityEngine;

public class CorruptedLegend : MonoBehaviour
{
    [Header("Legend Settings")]
    public int hitsToDefeat = 5;

    [Header("UI Data")]
    public string legendName;
    public Sprite portrait;

    [Header("Vanish Effect (Optional)")]
    public GameObject vanishEffectPrefab;

    private Animator anim;
    private int hitCount;
    private bool defeated;

     private SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (defeated) return;

        if (other.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (defeated) return;

        if (other.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", false);
        }
    }

    public void TakeHit()
    {
        if (defeated) return;

        hitCount++;

        // Optional: flash color
        StartCoroutine(FlashRed());
        StartCoroutine(Knockback(0.1f, 0.2f)); // duration, distance

        if (hitCount >= hitsToDefeat)
        {
        Defeat();
        }
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = sr.color;
        sr.color = Color.red;       // flash red
        yield return new WaitForSeconds(0.1f); // duration of flash
        sr.color = originalColor;   // return to normal color
    }

    void Defeat()
    {
        defeated = true;

        // Stop animation
        anim.SetBool("isAttacking", false);

        // Spawn vanish effect (optional)
        if (vanishEffectPrefab != null)
        {
            Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
        }

        // Hide legend visually
        GetComponent<SpriteRenderer>().enabled = false;

        // Disable collider
        GetComponent<Collider2D>().enabled = false;

    }

    private IEnumerator Knockback(float duration, float distance)
    {
        Vector3 originalPos = transform.position;

        // Move slightly to the RIGHT
        Vector3 targetPos = originalPos + Vector3.right * distance;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Smoothly return back
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(targetPos, originalPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }

}
