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

    [Header("Leveling Config")]
    [SerializeField] private int startingExpToLevelUp = 100;

    private int currentLevel = 0;
    private int currentExp = 0;
    private int expToNextLevel;
    private float maxHP;

    private void Start()
    {
        currentLevel = 0;
        currentExp = 0;
        expToNextLevel = startingExpToLevelUp;
        UpdateUI();
    }

    public void InitializeHUD(Sprite icon, float currentHP, float maxHPValue)
    {
        iconImage.sprite = icon;

        maxHP = maxHPValue;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        hpText.text = $"{(int)currentHP} / {(int)maxHP}";

        expSlider.maxValue = expToNextLevel;
        expSlider.value = currentExp;

        levelText.text = $"LV: {currentLevel:D2}";
    }

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

    public void UpdateHP(float currentHP)
    {
        hpSlider.value = currentHP;
        hpText.text = $"{(int)currentHP} / {(int)maxHP}";
    }

    private void UpdateUI()
    {
        expSlider.maxValue = expToNextLevel;
        expSlider.value = currentExp;
        levelText.text = $"LV: {currentLevel:D2}";
    }
}
