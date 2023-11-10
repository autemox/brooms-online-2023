using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    public class CharacterLedgeHangNoClimb : CharacterLedgeHang
    {
        /// <summary>
        /// Override the HandleInput to disable climb
        /// </summary>
        protected override void HandleInput()
        {
            // Do nothing, effectively disabling the climb behavior
        }

        /// <summary>
        /// Override the Climb coroutine to disable climb
        /// </summary>
        protected override IEnumerator Climb()
        {
            // Simply return null, without calling base.Climb() climbing is effectively disabled
            yield return null;
        }
    }
}
