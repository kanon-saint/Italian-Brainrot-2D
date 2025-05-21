using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private Image characterImageUI;      // Image component on the button to show character sprite

    private CharacterData characterData;
    private CharacterInfoPanel infoPanel;
    private CharacterSelectionManager selectionManager;

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
        if (infoPanel != null && characterData != null)
        {
            infoPanel.SetInfo(characterData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectionManager.SelectCharacter(characterData);
    }
}
