using System.Collections.Generic;
using UnityEngine;

public class MakeInvisible : MonoBehaviour
{
    [Header("List of Parent GameObjects, Children will be made invisible")]
    [Tooltip("Drag and drop parent GameObjects into this list. The SpriteRenderer of all child objects will be disabled at Start.")]
    [SerializeField]
    private List<GameObject> parentObjects;  // Drag your parent GameObjects here in the Unity Inspector

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject parentObject in parentObjects)
        {
            // Disable all child sprite renderers
            TurnOffChildSpriteRenderers(parentObject.transform);
        }
    }

    void TurnOffChildSpriteRenderers(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
            if (child.childCount > 0)
            {
                TurnOffChildSpriteRenderers(child);
            }
        }
    }
}
