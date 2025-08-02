using UnityEngine;
using UnityEngine.UI;

public class AnaKarakterCanSistemi : MonoBehaviour
{
    [SerializeField] private float can = 100;
    public float Can => can;
    public float maksimumCan = 100;
    public Image canBar;

    [Header("Regen Ayarlar�")]
    public float regenRate = 5f;      // Saniyede ka� can yenilenecek
    public float regenDelay = 3f;     // Hasar ald�ktan sonra ka� saniye sonra regen ba�lar

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
            regenTimer = regenDelay; // Hasar al�nca regen gecikmesi ba�lat
            canRegen = false;
        }
        //Debug.Log("Can azald�: " + can);
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
        Debug.Log("Can artt�: " + can);
    }

    private void Olum()
    {
        isDead = true;
        Debug.Log("Karakter �ld�!");
        if (playerController != null)
            playerController.Die();
        //animat�r� kapat
    }

    private void Update()
    {
        if (canBar != null)
        {
            canBar.fillAmount = can / maksimumCan;
        }

        // Regen mant���
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