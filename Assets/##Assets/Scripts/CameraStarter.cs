using UnityEngine;

public class CameraStarter : MonoBehaviour
{
    public Cinemachine.CinemachineDollyCart dollyCart;
    public float speed = 0.2f; // Ýstediðin hýz
    void Start()
    {
        dollyCart.m_Speed = speed;  
    }
}
