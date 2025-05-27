using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI attackNameText;
    [SerializeField] private TextMeshProUGUI attackDescText;
    [SerializeField] private TextMeshProUGUI characterDescText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI attackPowerText;

    [Header("Image Elements")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private Image SpriteImageLeft;
    [SerializeField] private Image SpriteImageRight;

    [Header("Audio Elements")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] characterSelectSound;

    public void SetInfo(CharacterData data)
    {
        nameText.text = data.characterName;
        attackNameText.text = data.attackName;
        attackDescText.text = data.attackDescription;
        characterDescText.text = data.characterDescription;
        hpText.text = "HP: " + data.maxHP;
        speedText.text = "Speed: " + data.speed;
        attackPowerText.text = "Attack: " + data.attackPower;

        portraitImage.sprite = data.characterImage;
        SpriteImageLeft.sprite = data.characterPng;
        SpriteImageRight.sprite = data.characterPng;

        if (data.characterName.ToLower() == "br br patapim" && characterSelectSound.Length > 0)
        {
            audioSource.clip = characterSelectSound[0];
            audioSource.Play();
        }
        else if (data.characterName.ToLower() == "tralalero tralala" && characterSelectSound.Length > 0)
        {
            audioSource.clip = characterSelectSound[1];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = characterSelectSound[2];
            audioSource.Play();
        }
    }
}
