using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractiveAudioObject : MonoBehaviour
{
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip hoverClip;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color pressedColor = Color.green;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SetColor(normalColor);
    }

    private void OnMouseEnter()
    {
        SetColor(hoverColor);

        if (audioSource != null && hoverClip != null)
            audioSource.PlayOneShot(hoverClip);
    }

    private void OnMouseExit()
    {
        SetColor(normalColor);
    }

    private void OnMouseDown()
    {
        SetColor(pressedColor);

        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }

    private void OnMouseUp()
    {
        SetColor(hoverColor);
    }

    private void SetColor(Color color)
    {
        if (targetRenderer != null && targetRenderer.material.HasProperty("_Color"))
            targetRenderer.material.color = color;
    }
}
