using UnityEngine;

namespace HighElixir.Timers.Internal
{
#if UNITY_2017_1_OR_NEWER
    [DefaultExecutionOrder(-100)]
    internal class GlobalTimerDriver : MonoBehaviour
    {
        private void Update()
        {
            if (GlobalTimer.update.IsCreated)
                GlobalTimer.update.Instance.Update(Time.deltaTime);
        }
        private void FixedUpdate()
        {
            if (GlobalTimer.fixedUpdate.IsCreated)
                GlobalTimer.fixedUpdate.Instance.Update(Time.fixedDeltaTime);
        }
    }
#else
    internal class GlobalTimerDriver
    {
        // No implementation needed for non-Unity environments
    }
#endif
}