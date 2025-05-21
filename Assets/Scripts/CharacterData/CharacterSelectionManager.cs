// using UnityEngine;

// public class CharacterSelectionManager : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Transform gridParent; // Your CharacterGridPanel
//     [SerializeField] private CharacterInfoPanel infoPanel;
//     [SerializeField] private GameObject characterButtonPrefab;
//     [SerializeField] private CharacterData[] allCharacters;

//     void Start()
//     {
//         foreach (var data in allCharacters)
//         {
//             GameObject btnObj = Instantiate(characterButtonPrefab, gridParent);
//             CharacterButton charBtn = btnObj.GetComponent<CharacterButton>();
//             charBtn.Setup(data, infoPanel);
//         }
//     }
// }
