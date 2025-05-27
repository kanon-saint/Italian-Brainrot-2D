using UnityEngine;

public class AxeOrbit : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    public float radius = 3f;
    public float orbitSpeed = 180f;
    public float selfSpinSpeed = 360f;
    public GameObject axePrefab;

    private Transform character;
    private float angle;
    public AxeOrbit[] orbitingAxes;
    private int currentLevel = 0;
    private bool initialized = false;
    private bool isOriginal = true;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (initialized) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            character = playerObj.transform;

        if (character == null)
        {
            Debug.LogError("Player object with tag 'Player' not found!");
            return;
        }

        if (isOriginal)
        {
            orbitingAxes = new AxeOrbit[1];
            orbitingAxes[0] = this;

            // Don't call UpdateOrbitingAxes here to avoid initializing with wrong level
        }

        initialized = true;
    }

    void Update()
    {
        if (!initialized) Initialize();
        if (character == null) return;

        // If original and weaponData is set and level has changed, update orbiting axes
        if (isOriginal && weaponData != null && weaponData.level != currentLevel)
        {
            currentLevel = weaponData.level;
            UpdateOrbitingAxes();
        }

        if (isOriginal && orbitingAxes != null)
        {
            angle += orbitSpeed * Time.deltaTime;
            if (angle > 360f) angle -= 360f;

            for (int i = 0; i < orbitingAxes.Length; i++)
            {
                if (orbitingAxes[i] != null)
                {
                    orbitingAxes[i].UpdateOrbit(character.position, angle, i);
                }
            }
        }
    }

private void UpdateOrbitingAxes()
{
    // Destroy all clones except original
    if (orbitingAxes != null && orbitingAxes.Length > 1)
    {
        for (int i = 1; i < orbitingAxes.Length; i++)
        {
            if (orbitingAxes[i] != null)
            {
                Destroy(orbitingAxes[i].gameObject);
                orbitingAxes[i] = null;  // Clear reference
            }
        }
    }

    int axeCount = Mathf.Clamp(currentLevel, 1, 5);

    // Create a new array with axeCount size
    AxeOrbit[] newOrbitingAxes = new AxeOrbit[axeCount];
    newOrbitingAxes[0] = this; // original axe

    for (int i = 1; i < axeCount; i++)
    {
        if (axePrefab != null)
        {
            GameObject newAxe = Instantiate(axePrefab, transform.position, Quaternion.identity, transform.parent);
            AxeOrbit axeOrbit = newAxe.GetComponent<AxeOrbit>();

            if (axeOrbit != null)
            {
                axeOrbit.radius = radius;
                axeOrbit.orbitSpeed = orbitSpeed;
                axeOrbit.selfSpinSpeed = selfSpinSpeed;
                axeOrbit.weaponData = null; // clones don't track level
                axeOrbit.character = character;
                axeOrbit.isOriginal = false;

                // IMPORTANT: assign the shared orbiting axes array after all are created
                axeOrbit.Initialize();

                newOrbitingAxes[i] = axeOrbit;
            }
        }
    }

    // Assign the new array to this and to all clones so they all share the same array reference
    orbitingAxes = newOrbitingAxes;
    for (int i = 1; i < axeCount; i++)
    {
        if (orbitingAxes[i] != null)
        {
            orbitingAxes[i].orbitingAxes = orbitingAxes;
        }
    }
}


    public void UpdateOrbit(Vector3 center, float baseAngle, int index)
    {
        float angleOffset = (360f / orbitingAxes.Length) * index;
        float currentAngle = baseAngle + angleOffset;

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.position = center + offset;

        transform.Rotate(Vector3.forward, -selfSpinSpeed * Time.deltaTime);
    }
}
