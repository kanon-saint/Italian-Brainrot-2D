using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform character; // Still needed to get the spawn position and rotation
    public float spawnInterval = 2f;
    public float spreadAngle = 10f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnFireball();
            timer = 0f;
        }
    }

    void SpawnFireball()
    {
        if (fireballPrefab == null || character == null) return;

        // Calculate the rotation for the fireball based on the character's rotation
        // and a random spread angle.
        Quaternion fireballRotation = character.rotation *
                                      Quaternion.Euler(0, Random.Range(-spreadAngle, spreadAngle), 0);

        // Instantiate the fireball at the character's position with the calculated rotation.
        GameObject fireball = Instantiate(fireballPrefab, character.position, fireballRotation);

        // The Fireball script no longer needs the 'character' reference for its destruction logic,
        // as it now destroys itself based on a fixed travel distance from its spawn point.
        // Therefore, we no longer need to assign 'fb.character'.
        // Fireball fb = fireball.GetComponent<Fireball>();
        // if (fb != null)
        // {
        //     fb.character = character;
        // }
    }
}
