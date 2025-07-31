using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Door : MonoBehaviour
{
    [Header("Kapý Ayarlarý")]
    public float openAngle = 90f;
    public float openSpeed = 120f;
    public bool isOpen = false;

    private float targetYRotation;
    private float initialYRotation;
    private bool isRotating = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Kontrol bizde
        initialYRotation = transform.eulerAngles.y;
        targetYRotation = initialYRotation;
    }

    void FixedUpdate() // Artýk FixedUpdate içinde fiziksel dönüþ
    {
        if (isRotating)
        {
            float currentY = transform.eulerAngles.y;
            float newY = Mathf.MoveTowardsAngle(currentY, targetYRotation, openSpeed * Time.fixedDeltaTime);
            Quaternion newRotation = Quaternion.Euler(transform.eulerAngles.x, newY, transform.eulerAngles.z);
            rb.MoveRotation(newRotation);

            if (Mathf.Abs(Mathf.DeltaAngle(newY, targetYRotation)) < 0.1f)
            {
                isRotating = false;
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Kapý etkileþime geçildi");
        if (isRotating) return;

        if (!isOpen)
        {
            targetYRotation = initialYRotation + openAngle;
            isOpen = true;
        }
        else
        {
            targetYRotation = initialYRotation;
            isOpen = false;
        }
        isRotating = true;
    }
}
