using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour
{
    // We will still have a serialized field for the AudioClip for convenience in the Inspector
    // However, the *primary* AudioClip for the AudioSource component will be used if set.
    [SerializeField] private AudioClip fallbackClickSound; // Optional: A sound to use if AudioSource doesn't have one
    [SerializeField] private float volume = 1.0f;

    private Button uiButton;
    private AudioSource audioSource; // Reference to the AudioSource component

    void Awake()
    {
        uiButton = GetComponent<Button>();

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Essential settings for a button sound
        audioSource.playOnAwake = false; // Never play sound automatically
        audioSource.loop = false;       // Don't loop button sounds
        audioSource.volume = volume;    // Set initial volume
    }

    void Start()
    {
        if (uiButton != null)
        {
            uiButton.onClick.AddListener(PlayClickSound);
        }
        else
        {
            Debug.LogError("Button component not found on this GameObject. UIButtonSound requires a Button component.", this);
        }

        // Log which AudioClip will be used if one is set directly on the AudioSource component
        if (audioSource.clip != null)
        {
            Debug.Log($"🔊 AudioSource on '{gameObject.name}' has default clip: '{audioSource.clip.name}'. This will be used.");
        }
        else if (fallbackClickSound != null)
        {
            Debug.Log($"🔊 AudioSource on '{gameObject.name}' has no default clip. Using 'Fallback Click Sound': '{fallbackClickSound.name}'.");
        }
        else
        {
            Debug.LogWarning($"⚠️ No default clip on AudioSource and no Fallback Click Sound assigned for '{gameObject.name}'. Sound may not play.", this);
        }
    }

    private void PlayClickSound()
    {
        Debug.Log("🔘 Button clicked.");

        // Determine which AudioClip to play:
        // 1. Prioritize the AudioClip directly assigned to the AudioSource component's 'AudioClip' field.
        // 2. Fallback to 'fallbackClickSound' if the AudioSource's clip is null.
        AudioClip clipToPlay = audioSource.clip;
        if (clipToPlay == null)
        {
            clipToPlay = fallbackClickSound;
        }

        if (clipToPlay != null)
        {
            // Create a temporary GameObject to play the sound and ensure it finishes
            // This is the robust way to handle sounds needing to finish after the source disappears.
            GameObject soundGameObject = new GameObject("OneShotAudio");
            AudioSource tempAudioSource = soundGameObject.AddComponent<AudioSource>();

            // Copy relevant settings from the button's AudioSource
            tempAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
            tempAudioSource.volume = audioSource.volume; // Use the volume from the component
            tempAudioSource.spatialBlend = audioSource.spatialBlend; // If 3D sound is desired
            // You can copy more properties like pitch, reverb, etc., if needed

            tempAudioSource.PlayOneShot(clipToPlay, volume); // Use PlayOneShot for click sounds

            // Destroy the temporary GameObject after the sound finishes
            Destroy(soundGameObject, clipToPlay.length);

            Debug.Log($"🔊 Playing '{clipToPlay.name}' on temporary GameObject. Volume: {volume}. Will destroy in {clipToPlay.length} seconds.");
        }
        else
        {
            Debug.LogWarning("⚠️ No click sound available to play for this button. Assign a clip to the AudioSource component or the Fallback Click Sound field.", this);
        }
    }

    void OnDestroy()
    {
        // Clean up the listener when the GameObject is destroyed
        if (uiButton != null)
        {
            uiButton.onClick.RemoveListener(PlayClickSound);
        }
    }
}