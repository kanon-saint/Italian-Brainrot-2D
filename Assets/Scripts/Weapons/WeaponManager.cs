using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Weapons")]
    [SerializeField] private WeaponData[] allWeapons;

    [Header("UI")]
    [SerializeField] private GameObject powerUpPanel;
    [SerializeField] private WeaponButtonUI[] weaponButtons;

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

        List<WeaponData> choices = GetRandomWeapons(3);

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
                Destroy(oldWeapon);
        }

        // Instantiate and attach new weapon
        GameObject newWeapon = Instantiate(weaponData.prefab, player.transform);
        newWeapon.transform.localPosition = Vector3.zero;

        // Replace the old one in dictionary
        attachedWeapons[weaponData.weaponName] = newWeapon;

        Debug.Log($"Equipped weapon: {weaponData.weaponName} at level {weaponData.level}");

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

    public void ResetAllWeaponLevels()
    {
        foreach (var weapon in allWeapons)
        {
            if (weapon != null)
            {
                weapon.level = 0;
            }
        }

        Debug.Log("All weapon levels have been reset to 0.");
    }

}
