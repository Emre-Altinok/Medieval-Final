using UnityEngine;
using UnityEngine.UI;

public class AnaKarakterCanSistemi : MonoBehaviour
{
    [SerializeField] private float can = 100;
    public float Can => can;
    public float maksimumCan = 100;
    public Image canBar;

    [Header("Regen Ayarlarý")]
    public float regenRate = 5f;      // Saniyede kaç can yenilenecek
    public float regenDelay = 3f;     // Hasar aldýktan sonra kaç saniye sonra regen baþlar

    private float regenTimer = 0f;
    private bool canRegen = true;

    private PlayerController playerController;
    private InGameUIManager inGameUIManager;

    private bool isDead = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inGameUIManager = FindAnyObjectByType<InGameUIManager>();
    }

    public void CanAzalt(float miktar)
    {
        can -= miktar;
        if (can <= 0)
        {
            can = 0;
            Olum();
        }
        else
        {
            regenTimer = regenDelay; // Hasar alýnca regen gecikmesi baþlat
            canRegen = false;
        }
        //Debug.Log("Can azaldý: " + can);
        if (inGameUIManager != null)
            inGameUIManager.ShowDamagePanel();
    }

    public void CanArttir(float miktar)
    {
        can += miktar;
        if (can > maksimumCan)
        {
            can = maksimumCan;
        }
        Debug.Log("Can arttý: " + can);
    }

    private void Olum()
    {
        isDead = true;
        Debug.Log("Karakter öldü!");
        if (playerController != null)
            playerController.Die();
        //animatörü kapat
    }

    private void Update()
    {
        if (canBar != null)
        {
            canBar.fillAmount = can / maksimumCan;
        }

        // Regen mantýðý
        if (!isDead && can < maksimumCan)
        {
            if (!canRegen)
            {
                regenTimer -= Time.deltaTime;
                if (regenTimer <= 0f)
                {
                    canRegen = true;
                }
            }
            else
            {
                can += regenRate * Time.deltaTime;
                if (can > maksimumCan)
                    can = maksimumCan;
            }
        }
    }
}