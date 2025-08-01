using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class KnightOnEnd : MonoBehaviour
{
    public float walkSpeed = 2f;         // Yürüme hýzý (m/sn)
    public float walkDistance = 50f;     // Yürüyeceði mesafe (metre)

    private bool isWalking = false;
    private Vector3 walkDirection;
    private Vector3 startPosition;
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void OnDialogueEnd()
    {
        if (isWalking) return; // Tekrar baþlatma engeli

        // Arkasýný dön (180 derece döndür)
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);

        // Rotasyon güncellendikten sonra yönü al!
        walkDirection = transform.forward;
        startPosition = transform.position;
        isWalking = true;

        // Animatörde yürüme animasyonunu baþlat
        if (animator != null)
            animator.SetFloat("Speed", walkSpeed);

        StartCoroutine(WalkAndDestroy());
    }

    private System.Collections.IEnumerator WalkAndDestroy()
    {
        while (Vector3.Distance(startPosition, transform.position) < walkDistance)
        {
            // Rigidbody ile hareket
            rb.MovePosition(transform.position + walkDirection * walkSpeed * Time.deltaTime);

            // Animatörde yürüme animasyonu devam etsin
            if (animator != null)
                animator.SetFloat("Speed", walkSpeed);

            yield return null;
        }

        // Animasyonu durdur
        if (animator != null)
            animator.SetFloat("Speed", 0f);

        Destroy(gameObject);
    }
}