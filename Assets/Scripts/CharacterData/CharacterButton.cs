using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image characterImageUI;      // Image component on the button to show character sprite

    private CharacterData characterData;
    private CharacterInfoPanel infoPanel;

    public void Initialize(CharacterData data, CharacterInfoPanel panel)
    {
        characterData = data;
        infoPanel = panel;

        // Update button UI with character info
        if (characterImageUI != null)
            characterImageUI.sprite = characterData.characterImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null && characterData != null)
        {
            infoPanel.SetInfo(characterData);
        }
    }
}
