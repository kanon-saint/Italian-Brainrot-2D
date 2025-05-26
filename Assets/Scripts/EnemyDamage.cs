using UnityEngine;
using System.Collections.Generic;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageCooldown = 0.5f; // 0.5 seconds invincibility per enemy

    // Store last damage time *per player* (if multiplayer) or just once for player here
    private float lastDamageTime = -Mathf.Infinity;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                var character = GameManager.Instance?.selectedCharacter;
                if (character == null) return;

                character.currentHP -= (int)damage;
                if (character.currentHP < 0) character.currentHP = 0;

                CharacterHUD.Instance.UpdateHP(character.currentHP);

                Debug.Log($"Player damaged by {gameObject.name}. Current HP: {character.currentHP}");

                if (character.currentHP <= 0)
                {
                    GameOverManager.Instance.TriggerGameOver();
                }

                lastDamageTime = Time.time;
            }
        }
    }
}
