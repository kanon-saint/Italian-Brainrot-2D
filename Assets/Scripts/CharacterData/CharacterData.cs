using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public string attackName;
    [TextArea] public string attackDescription;
    [TextArea] public string characterDescription;
    public int maxHP;
    public int currentHP;
    public float speed;
    public float attackPower;

    public GameObject characterPrefab;
    public Sprite characterImage;
    public Sprite characterPng;
    
    public void ResetHP()
    {
        currentHP = maxHP;
    }
}
