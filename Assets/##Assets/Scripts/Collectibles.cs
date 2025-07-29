using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public string collectibleType; // Inspector'dan atanabilir

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Görev ilerlemesini bildir
            if (ActiveQuestTracker.instance != null)
                ActiveQuestTracker.instance.RegisterGather(collectibleType);

            Destroy(gameObject);
        }
    }
}