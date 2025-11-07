using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();

        button.onClick.AddListener(() =>
        {
            audioSource.PlayOneShot(clickSound);
        });
    }
}
