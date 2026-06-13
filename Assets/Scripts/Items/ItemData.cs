using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu (fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Description")]
    public string itemName;
    public int price;

    [Header("Visuals")]
    public GameObject modelPrefab;
    public Sprite icon;

    [Header("Modifiers")]
    public float damageMultiplier;
    public float speedMultiplier;
    public float firerateMultiplier;

    public int healthBonus;
    public int damageBonus;
    public int speedBonus;

    public enum uniqueEffects
    { 
        None,BulletCircle,BulletSize,BulletSpeed,DamageShield,EnemySlowDown,PassiveRegen,RandomBuffs,ShipSpike,StationaryFireRate
    }
    public uniqueEffects ItemEffect;

}
