using System;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif

namespace HighElixir.Timers
{
    public static class GlobalTimer
    {
        public class Wrapper
        {
            public readonly Lazy<Timer> Timer;
            public bool IsCreated => Timer.IsValueCreated;
            internal Timer Instance => Timer.Value;
            public Wrapper()
            {
                Timer = new Lazy<Timer>(() => new Timer(typeof(GlobalTimer)));
            }
        }

        internal static readonly Wrapper update = new Wrapper();
        internal static readonly Wrapper fixedUpdate = new Wrapper();

        public static Timer Update => update.Instance;
        public static Timer FixedUpdate => fixedUpdate.Instance;
        static GlobalTimer()
        {
#if UNITY_2017_1_OR_NEWER
            Application.quitting += () =>
            {
                update.Timer.Value?.Dispose();
                fixedUpdate.Timer.Value?.Dispose();
            };
            if (GameObject.FindAnyObjectByType<Timers.Internal.GlobalTimerDriver>() != null)
                return;
            GameObject go = new GameObject("GlobalTimerDriver");
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<Timers.Internal.GlobalTimerDriver>();
#else
            AppDomain.CurrentDomain.ProcessExit += (s, e) => 
            {
                _update.Timer.Value?.Dispose();
                _fixedupdate.Timer.Value?.Dispose();
            };
#endif
        }

    }

}