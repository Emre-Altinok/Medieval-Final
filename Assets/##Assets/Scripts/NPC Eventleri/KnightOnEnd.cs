using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class KnightOnEnd : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Material mat;


    [Header("Efektler")]
    public ParticleSystem mistParticles;
    public Light flashLight;
    //public AudioSource vanishSound;


    void Start()
    {
        mat = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = mat;
    }

    public void OnDialogueEndEvent()
    {
        Debug.Log("KnightOnEnd: OnDialogueEndEvent called, starting fade and destroy.");
        StartCoroutine(FadeAndDestroy());
        Debug.Log("AFTERLINEKnightOnEnd: OnDialogueEndEvent called, starting fade and destroy.");
    }

    private System.Collections.IEnumerator FadeAndDestroy()
    {

        if (mistParticles != null) mistParticles.Play();
        if (flashLight != null) flashLight.enabled = true;
        //if (vanishSound != null) vanishSound.Play();

        float fadeTime = 0f;
        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            float amount = Mathf.Clamp01(fadeTime / fadeDuration);
            mat.SetFloat("_Fade", amount);
            yield return null;
        }
        mat.SetFloat("_Fade", 1f);
        Destroy(gameObject);
    }
}