using UnityEngine;

public class ItemPick : MonoBehaviour
{

    public GameObject sword;
    public GameObject shield;
    public GameObject helm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            sword.SetActive(true);
            shield.SetActive(true);
            helm.SetActive(true);

            Destroy(gameObject);
        }
    }



}
