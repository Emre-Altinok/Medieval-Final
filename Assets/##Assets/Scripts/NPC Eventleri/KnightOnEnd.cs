using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class KnightOnEnd : MonoBehaviour
{
    public float walkSpeed = 2f;         // Y�r�me h�z� (m/sn)
    public float walkDistance = 50f;     // Y�r�yece�i mesafe (metre)

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
        if (isWalking) return; // Tekrar ba�latma engeli

        // Arkas�n� d�n (180 derece d�nd�r)
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);

        // Rotasyon g�ncellendikten sonra y�n� al!
        walkDirection = transform.forward;
        startPosition = transform.position;
        isWalking = true;

        // Animat�rde y�r�me animasyonunu ba�lat
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

            // Animat�rde y�r�me animasyonu devam etsin
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