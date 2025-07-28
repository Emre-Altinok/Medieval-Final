using UnityEngine;

public class Point : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public float speed = 2f;

    private Transform target;
    private Animator animator;

    void Start()
    {
        if (PointA == null || PointB == null)
        {
            Debug.LogError("Nokta eksik! PointA ve PointB atanmal�.");
            enabled = false;
            return;
        }

        target = PointB;

        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsWalking", true); // y�r�y�� animasyonu aktif
        }
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, targetPosition);
        Debug.Log("Hedefe uzakl�k: " + dist);  // Bu sat�r� ekle

        if (dist < 0.5f)
        {
            target = (target == PointA) ? PointB : PointA;
            Debug.Log("Hedef de�i�ti! Yeni hedef: " + target.name); // Bu sat�r� ekle
            RotateTowards(target.position);
        }
    }


    void RotateTowards(Vector3 lookAtTarget)
    {
        Vector3 direction = (lookAtTarget - transform.position).normalized;
        direction.y = 0; // sadece yatay eksende d�nd�r

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
