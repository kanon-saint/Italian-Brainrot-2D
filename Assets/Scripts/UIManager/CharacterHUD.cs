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
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private WeaponManager weaponManager; // Assign in Inspector
    [SerializeField] private int expIncreasePerLevel = 100;



    [Header("Leveling Configuration")]
    [SerializeField] private int startingExpToLevelUp = 100;

    public static CharacterHUD Instance { get; private set; }

    private int currentLevel;
    private int currentExp;
    private int expToNextLevel;
    private float maxHP;
    private float timer = 0f;


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
        UpdateUI();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void InitializeHUD(Sprite icon, int currentHP, int maxHPValue)
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
        hpText.text = $"{currentHP} / {maxHP}";

        UpdateUI();
    }


    public void AddExperience(int amount)
    {
        currentExp += amount;

        bool leveledUp = false;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;
            expToNextLevel += expIncreasePerLevel;

            leveledUp = true;
        }

        UpdateUI();

        if (leveledUp && weaponManager != null)
        {
            weaponManager.ShowWeaponChoices();
        }
    }


    public void UpdateHP(float currentHP)
    {
        hpSlider.value = currentHP;
        hpText.text = $"{(int)currentHP} / {(int)maxHP}";
    }

    public void ResetExperience()
    {
        currentLevel = 0;
        currentExp = 0;
        expToNextLevel = startingExpToLevelUp;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = currentExp;
        }

        if (levelText != null)
            levelText.text = $"LV: {currentLevel:D2}";
    }


    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;
}
