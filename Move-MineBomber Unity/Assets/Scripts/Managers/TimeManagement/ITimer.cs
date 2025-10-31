using HighElixir.Timers;
using System;

namespace Bomb.Managers.Timers
{
    public interface ITimer
    {
        IObservable<float> ReactiveProperty { get; }

        void Start();
        int Get();
        void Stop();
        void Reset();
    }
}