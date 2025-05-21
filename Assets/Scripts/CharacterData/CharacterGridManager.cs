using UnityEngine;

public class CharacterGridManager : MonoBehaviour
{
    [SerializeField] private CharacterData[] characterList;
    [SerializeField] private GameObject characterButtonPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private CharacterInfoPanel infoPanel; // Reference this in Inspector
    [SerializeField] private CharacterSelectionManager selectionManager;

    void Start()
    {
        PopulateCharacterGrid();
    }

    private void PopulateCharacterGrid()
    {
        foreach (CharacterData character in characterList)
        {
            GameObject btnGO = Instantiate(characterButtonPrefab, gridParent);
            CharacterButton btnScript = btnGO.GetComponent<CharacterButton>();
            btnScript.Initialize(character, infoPanel, selectionManager);
        }
    }
}
