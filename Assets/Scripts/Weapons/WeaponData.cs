using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data", order = 1)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public GameObject prefab;
    [TextArea]
    public string description;
}
