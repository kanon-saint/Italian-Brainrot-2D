using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Weapons")]
    [SerializeField] private WeaponData[] allWeapons;

    [Header("UI")]
    [SerializeField] private GameObject powerUpPanel;
    [SerializeField] private WeaponButtonUI[] weaponButtons;

    [Header("Equipped Weapon Display")]
    [SerializeField] private Transform equippedWeaponsPanel; // Parent with Grid Layout Group
    [SerializeField] private GameObject equippedWeaponItemPrefab; // UI prefab with Image + Text

    private Dictionary<string, GameObject> attachedWeapons = new();


    // Helper property to find player dynamically when needed
    private GameObject Player
    {
        get
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
                Debug.LogError("Player GameObject with tag 'Player' not found!");
            return playerObj;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        if (powerUpPanel != null)
            powerUpPanel.SetActive(false);
        else
            Debug.LogError("PowerUpPanel not assigned in Inspector!");
    }

    public void ShowWeaponChoices()
    {
        if (powerUpPanel == null)
        {
            Debug.LogError("PowerUpPanel not assigned in Inspector!");
            return;
        }

        Time.timeScale = 0f;
        powerUpPanel.SetActive(true);

        CharacterData selectedData = GameManager.Instance.selectedCharacter;
        Debug.Log($"Selected character: {selectedData.characterName}");
        string playerName = selectedData.characterName.ToLower();

        // Build weapon pool
        List<WeaponData> filteredWeapons = new();
        foreach (var weapon in allWeapons)
        {
            if (weapon == null) continue;

            string wName = weapon.weaponName.ToLower();

            // Always include core weapons
            if (wName == "axe" || wName == "fireball" || wName == "laser" || wName == "death zone")
            {
                filteredWeapons.Add(weapon);
            }
            // Conditionally include
            else if (wName == "club" && playerName.Contains("tung tung tung sahur"))
            {
                filteredWeapons.Add(weapon);
            }
            else if (wName == "bite" && playerName.Contains("tralalero tralala"))
            {
                filteredWeapons.Add(weapon);
            }
            else if (wName == "thorn" && playerName.Contains("br br patapim"))
            {
                filteredWeapons.Add(weapon);
            }
        }

        List<WeaponData> choices = GetRandomWeaponsFromPool(filteredWeapons, 3);

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                weaponButtons[i].gameObject.SetActive(true);
                weaponButtons[i].Setup(choices[i], this);
            }
            else
            {
                weaponButtons[i].gameObject.SetActive(false);
            }
        }
    }


    public void EquipWeapon(WeaponData weaponData)
    {
        Debug.Log($"EquipWeapon called with: {weaponData?.weaponName}");

        GameObject player = Player;

        if (weaponData == null || weaponData.prefab == null || player == null)
        {
            Debug.LogWarning("EquipWeapon failed: missing data or player");
            return;
        }

        // Destroy old weapon instance if it exists
        if (attachedWeapons.TryGetValue(weaponData.weaponName, out GameObject oldWeapon))
        {
            if (oldWeapon != null)
            {
                // Destroy any child axes that may have been created
                AxeOrbit oldAxeOrbit = oldWeapon.GetComponent<AxeOrbit>();
                if (oldAxeOrbit != null && oldAxeOrbit.orbitingAxes != null)
                {
                    for (int i = 1; i < oldAxeOrbit.orbitingAxes.Length; i++)
                    {
                        if (oldAxeOrbit.orbitingAxes[i] != null)
                        {
                            Destroy(oldAxeOrbit.orbitingAxes[i].gameObject);
                        }
                    }
                }
                Destroy(oldWeapon);
            }

        }


        // Instantiate and attach new weapon
        GameObject newWeapon = Instantiate(weaponData.prefab, player.transform);
        newWeapon.transform.localPosition = Vector3.zero;

        // Replace the old one in dictionary
        attachedWeapons[weaponData.weaponName] = newWeapon;

        Debug.Log($"Equipped weapon: {weaponData.weaponName} at level {weaponData.level}");

        DisplayEquippedWeapons();

        // Hide panel and resume game
        if (powerUpPanel != null)
            powerUpPanel.SetActive(false);

        Time.timeScale = 1f;
    }



    private List<WeaponData> GetRandomWeapons(int count)
    {
        List<WeaponData> pool = new(allWeapons);
        List<WeaponData> result = new();

        int actualCount = Mathf.Min(count, pool.Count);

        for (int i = 0; i < actualCount; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    private List<WeaponData> GetRandomWeaponsFromPool(List<WeaponData> pool, int count)
    {
        List<WeaponData> result = new();
        List<WeaponData> tempPool = new(pool);

        int actualCount = Mathf.Min(count, tempPool.Count);

        for (int i = 0; i < actualCount; i++)
        {
            int index = Random.Range(0, tempPool.Count);
            result.Add(tempPool[index]);
            tempPool.RemoveAt(index);
        }

        return result;
    }


    public void ResetAllWeaponLevels()
    {
        foreach (var weapon in allWeapons)
        {
            if (weapon == null) continue;

            string wName = weapon.weaponName.ToLower();

            // Core weapons always reset to level 0
            if (wName == "axe" || wName == "fireball" || wName == "laser" || wName == "death zone")
            {
                weapon.level = 0;
            }
            // Conditional weapons reset to level 1 based on player
            else if (wName == "club" || wName == "bite" || wName == "thorn")
            {
                weapon.level = 1;
            }
            else
            {
                weapon.level = 0; // All other weapons default to level 0
            }
        }

        Debug.Log("Weapon levels have been reset based on player character.");
    }

 public void DisplayEquippedWeapons()
{
    foreach (Transform child in equippedWeaponsPanel)
    {
        Destroy(child.gameObject);
    }

    foreach (var weapon in allWeapons)
    {
        if (weapon == null || weapon.level < 1)
            continue;

        string wName = weapon.weaponName.ToLower();
        CharacterData selectedData = GameManager.Instance.selectedCharacter;
        string playerName = selectedData.characterName.ToLower();

        bool isClub = wName == "club";
        bool isBite = wName == "bite";
        bool isThorn = wName == "thorn";

        bool isValidClub = isClub && playerName.Contains("tung tung tung sahur");
        bool isValidBite = isBite && playerName.Contains("tralalero tralala");
        bool isValidThorn = isThorn && playerName.Contains("br br patapim");

        if ((isClub && !isValidClub) || (isBite && !isValidBite) || (isThorn && !isValidThorn))
            continue;

        GameObject item = Instantiate(equippedWeaponItemPrefab, equippedWeaponsPanel);

        Image iconImage = item.GetComponent<Image>();
        if (iconImage == null)
            iconImage = item.transform.Find("Icon")?.GetComponent<Image>();

        TextMeshProUGUI levelText = item.GetComponentInChildren<TextMeshProUGUI>();

        if (iconImage != null) iconImage.sprite = weapon.icon;
        if (levelText != null) levelText.text = $"Lv. {weapon.level}";
    }
}


}
