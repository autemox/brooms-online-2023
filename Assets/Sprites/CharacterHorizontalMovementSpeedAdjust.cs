using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class CharacterHorizontalMovementSpeedAdjust : CharacterHorizontalMovement
    {
        [SerializeField] 
        private float timeBeforeRun = 0.3f; 
        private float timeKeyDown = 0f; 
        private bool isSpeedBoosted;

        private CharacterRun characterRun;

        protected override void Initialization()
        {
            base.Initialization();
            timeKeyDown = 0f;
            isSpeedBoosted = false;
            characterRun = GetComponent<CharacterRun>();
        }

        protected override void HandleHorizontalMovement()
        {
            base.HandleHorizontalMovement();

            if (_normalizedHorizontalSpeed != 0)
            {
                timeKeyDown += Time.deltaTime;
            }
            else if(isSpeedBoosted)
            {
                timeKeyDown = 0f;
                characterRun.ForceRun(false);
                isSpeedBoosted = false;
            }

            if (timeKeyDown >= timeBeforeRun && !isSpeedBoosted)
            {
                // Instead of changing WalkSpeed directly, calling RunStart function from CharacterRun component
                if (characterRun != null)
                {
                    // Debug.Log("[CharacterHorizontalMovementSpeedAdjust HandleHorizontalMovement() Switching to Run");
                    characterRun.ForceRun(true);
                    isSpeedBoosted = true;
                }
            }
        }
    }
}
