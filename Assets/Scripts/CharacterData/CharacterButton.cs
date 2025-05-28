using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private Image characterImageUI; // Image component on the button to show character sprite
    [SerializeField] private AudioClip clickSound;   // Sound to play when the button is clicked

    private CharacterData characterData;
    private CharacterInfoPanel infoPanel;
    private CharacterSelectionManager selectionManager;
    private AudioSource audioSource; // Reference to the AudioSource component

    void Awake()
    {
        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false; // Don't play sound automatically
    }

    public void Initialize(CharacterData data, CharacterInfoPanel panel, CharacterSelectionManager manager)
    {
        characterData = data;
        infoPanel = panel;
        selectionManager = manager;

        // Update button UI with character info
        if (characterImageUI != null)
            characterImageUI.sprite = characterData.characterImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // This method is still needed because IPointerEnterHandler is implemented,
        // but no sound logic is added here.
        if (infoPanel != null && characterData != null)
        {
            infoPanel.SetInfo(characterData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play click sound if assigned
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        selectionManager.SelectCharacter(characterData);
    }
}