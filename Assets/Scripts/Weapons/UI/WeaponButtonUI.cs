using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponButtonUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private WeaponData weaponData;
    private WeaponManager weaponManager;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnSelect);
        }
        else
        {
            Debug.LogWarning($"No Button component found on {gameObject.name}");
        }
    }

    public void Setup(WeaponData data, WeaponManager manager)
    {
        weaponData = data;
        weaponManager = manager;

        if (data == null)
        {
            Debug.LogError("WeaponButtonUI.Setup received null WeaponData!");
            gameObject.SetActive(false);
            return;
        }
        if (data.prefab == null)
        {
            Debug.LogError($"WeaponData '{data.weaponName}' has null prefab assigned!");
            gameObject.SetActive(false);
            return;
        }

        if (iconImage != null) iconImage.sprite = data.icon;
        if (nameText != null) nameText.text = data.weaponName;
        if (descriptionText != null) descriptionText.text = data.description;

        gameObject.SetActive(true);
        button.interactable = true;
    }


    // WeaponButtonUI.cs
    public void OnSelect()
    {
        Debug.Log($"Button clicked for weapon: {weaponData.weaponName}");
        if (weaponManager != null && weaponData != null)
        {
            weaponManager.EquipWeapon(weaponData);
        }
        else
        {
            Debug.LogWarning("WeaponManager or WeaponData is null on button select.");
        }
    }

}
