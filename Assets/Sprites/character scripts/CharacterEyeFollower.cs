using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class CharacterEyeFollower : CharacterAbility
{
    public class EyeTarget
    {
        public Vector2 Position;
        public float Priority;
        public string Name;
    }

    public float threshold = 100f;
    public float maxDistance = 0.03f;
    [SerializeField] public float xOffset = -0.01f;
    [SerializeField] public float yOffset = 0.03f;
    public Transform eyeBoneTransform;
    private Vector2 originalLocalPosition;
    private Vector2 newLocalPosition;
    public List<EyeTarget> Targets = new List<EyeTarget>();

    [SerializeField] public float maxMousePriority = 10;
    [SerializeField] public float maxAbilityPriority = 50;
    [SerializeField] public float maxPriority = 100;

    protected override void Initialization()
    {
        newLocalPosition = originalLocalPosition = new Vector2(eyeBoneTransform.localPosition.x, eyeBoneTransform.localPosition.y);
        base.Initialization();
    }

    void Update()
    {
        Targets.Clear();
        Character character = GetComponent<Character>();

        // Mouse as a target
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        float mousePriority = 10/Vector2.Distance(mouseWorldPosition, transform.position);
        Targets.Add(new EyeTarget { Name = "Mouse", Position = mouseWorldPosition, Priority = mousePriority < maxMousePriority ? mousePriority : maxMousePriority }) ;

        // Priority for running, crouching, and looking up
        float directionMultiplier = character.IsFacingRight ? 5 : -5;

        if (_movement.CurrentState == CharacterStates.MovementStates.Running)
        {
            Targets.Add(new EyeTarget { Name = character.IsFacingRight ? "Right" : "Left", Position = transform.position + new Vector3(directionMultiplier, 0, 0), Priority = maxAbilityPriority });
        }
        if (_movement.CurrentState == CharacterStates.MovementStates.Dangling)
        {
            Targets.Add(new EyeTarget { Name = character.IsFacingRight?"Right":"Left", Position = transform.position + new Vector3(directionMultiplier, 0, 0), Priority = maxAbilityPriority });
        }
        if (_movement.CurrentState == CharacterStates.MovementStates.Crouching)
        {
            Targets.Add(new EyeTarget { Name = "Down", Position = transform.position + new Vector3(0, -1, 0), Priority = maxAbilityPriority });
        }
        if (_movement.CurrentState == CharacterStates.MovementStates.LookingUp)
        {
            Targets.Add(new EyeTarget { Name = "Up", Position = transform.position + new Vector3(0, 5, 0), Priority = maxAbilityPriority });
        }

        // Determine the target with the highest priority
        EyeTarget highestPriorityTarget = Targets[0];
        foreach (EyeTarget target in Targets)
        {
            if (target.Priority > highestPriorityTarget.Priority)
            {
                highestPriorityTarget = target;
            }
        }

        // Calculate the eye movement
        if (highestPriorityTarget != null)
        {
            // Debug.Log("[CharacterEyeFollwer Update()] Target "+highestPriorityTarget.Name+" is " + highestPriorityTarget.Priority + " from player and at Position: "+ highestPriorityTarget.Position);
            Vector2 targetLocalPos = eyeBoneTransform.parent.InverseTransformPoint(highestPriorityTarget.Position);
            Vector2 directionToMove = targetLocalPos - (Vector2)eyeBoneTransform.localPosition;
            float distanceToTarget = directionToMove.magnitude;

            if (distanceToTarget < threshold)
            {
                Vector2 normalizedDirection = directionToMove.normalized;
                newLocalPosition = originalLocalPosition + (normalizedDirection * maxDistance);
            }
        }

        // Move eyes towards the target
        eyeBoneTransform.localPosition = Vector3.Lerp(
            eyeBoneTransform.localPosition,
            new Vector3(newLocalPosition.x + xOffset, newLocalPosition.y + yOffset, eyeBoneTransform.localPosition.z),
            Time.deltaTime * 5f
        );

        // Reset the targetPosition to default
        newLocalPosition = new Vector3(originalLocalPosition.x, originalLocalPosition.y, eyeBoneTransform.localPosition.z);
    }
}
