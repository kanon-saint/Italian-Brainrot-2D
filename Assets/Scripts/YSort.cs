using UnityEngine;
using UnityEngine.Rendering; // for SortingGroup

[RequireComponent(typeof(SortingGroup))]
public class YSort : MonoBehaviour
{
    private SortingGroup sortingGroup;

    [SerializeField] private int sortingOffset = 0; // Add this for manual offset

    void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    void LateUpdate()
    {
        sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100) + sortingOffset;
    }
}
