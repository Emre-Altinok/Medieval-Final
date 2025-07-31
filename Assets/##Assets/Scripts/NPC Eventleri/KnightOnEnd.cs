using UnityEngine;

public class KnightOnEnd : MonoBehaviour
{
    public float walkSpeed = 2f;         // Yürüme hýzý (m/sn)
    public float walkDistance = 5f;      // Yürüyeceði mesafe (metre)

    private bool isWalking = false;
    private Vector3 walkDirection;
    private Vector3 startPosition;

    public void OnDialogueEnd()
    {
        // Arkasýný dön (180 derece döndür)
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180f, 0);

        // Rotasyon güncellendikten sonra yönü al!
        walkDirection = transform.forward;
        startPosition = transform.position;
        isWalking = true;

        StartCoroutine(WalkAndDestroy());
    }

    private System.Collections.IEnumerator WalkAndDestroy()
    {
        float walked = 0f;
        while (walked < walkDistance)
        {
            float step = walkSpeed * Time.deltaTime;
            transform.position += walkDirection * step;
            walked += step;
            yield return null;
        }
        Destroy(gameObject);
    }
}