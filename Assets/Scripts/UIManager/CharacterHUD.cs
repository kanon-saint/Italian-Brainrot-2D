using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Leveling Configuration")]
    [SerializeField] private int startingExpToLevelUp = 100;

    public static CharacterHUD Instance { get; private set; }

    private int currentLevel;
    private int currentExp;
    private int expToNextLevel;
    private float maxHP;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ResetExperience();
    }

    /// <summary>
    /// Initializes the HUD with icon and HP values.
    /// </summary>
    public void InitializeHUD(Sprite icon, float currentHP, float maxHPValue)
    {
        if (iconImage == null || hpSlider == null || hpText == null || expSlider == null || levelText == null)
        {
            Debug.LogError("CharacterHUD: UI references are missing!");
            return;
        }

        iconImage.sprite = icon;

        maxHP = maxHPValue;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        hpText.text = $"{(int)currentHP} / {(int)maxHP}";

        UpdateUI();
    }

    /// <summary>
    /// Adds experience and handles level-up logic.
    /// </summary>
    public void AddExperience(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;
            expToNextLevel *= 2;
        }

        UpdateUI();
    }

    /// <summary>
    /// Updates the HP display.
    /// </summary>
    public void UpdateHP(float currentHP)
    {
        hpSlider.value = currentHP;
        hpText.text = $"{(int)currentHP} / {(int)maxHP}";
    }

    /// <summary>
    /// Resets the experience and level to starting values.
    /// </summary>
    public void ResetExperience()
    {
        currentLevel = 0;
        currentExp = 0;
        expToNextLevel = startingExpToLevelUp;
        UpdateUI();
    }

    /// <summary>
    /// Updates experience slider and level text.
    /// </summary>
    private void UpdateUI()
    {
        expSlider.maxValue = expToNextLevel;
        expSlider.value = currentExp;
        levelText.text = $"LV: {currentLevel:D2}";
    }

    // Public getters (read-only)
    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;
}
