using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBoneFollower : MonoBehaviour
{
    public float threshold = 10f;
    public float maxDistance = 0.03f;
    public float xOffset = 0.03f;
    public Transform eyeBoneTransform;
    private Vector2 originalLocalPosition;
    public Vector2 target;
    private Vector2 newLocalPosition;

    void Start()
    {
        newLocalPosition=originalLocalPosition = new Vector2(eyeBoneTransform.localPosition.x, eyeBoneTransform.localPosition.y);
    }

    void Update()
    {
        // remove this later
        target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        
        if (target != null)
        {
            // calculate new
            Vector2 targetLocalPos = eyeBoneTransform.parent.InverseTransformPoint(target);
            Vector2 directionToMove = targetLocalPos - (Vector2)eyeBoneTransform.localPosition;
            float distanceToTarget = directionToMove.magnitude;

            if (distanceToTarget < threshold)
            {
                Vector2 normalizedDirection = directionToMove.normalized;
                newLocalPosition = originalLocalPosition + (normalizedDirection * maxDistance);
            }
        }

        // move eyes toward newLocalPosition
        eyeBoneTransform.localPosition = Vector3.Lerp(
            eyeBoneTransform.localPosition,
            new Vector3(newLocalPosition.x, newLocalPosition.y + xOffset, eyeBoneTransform.localPosition.z),
            Time.deltaTime * 5f
        );

        // reset targetPosition to default
        newLocalPosition = new Vector3(originalLocalPosition.x, originalLocalPosition.y, eyeBoneTransform.localPosition.z);
    }
}
