using HighElixir.Timers;
using System;
using UnityEngine;

public class GameTimerModel : Bomb.Managers.Timers.ITimer, IDisposable
{
    private TimerTicket _currentTimer;
    private bool _isRunning;

    public IObservable<float> ReactiveProperty => Timer.GetCurrentReactive(_currentTimer);

    private Timer Timer => GlobalTimer.Update;
    public GameTimerModel()
    {
        // 名前付きで登録しておくとデバッグしやすい！
        _currentTimer = Timer.CountUpRegister("経過時間");
        _isRunning = false;
    }

    public void Start()
    {
        if (_isRunning) return;
        _isRunning = true;
        Timer.Start(_currentTimer);
    }

    public void Stop()
    {
        if (!_isRunning) return;
        _isRunning = false;
        Timer.Stop(_currentTimer);
    }

    public void Reset()
    {
        Timer.Reset(_currentTimer);
    }

    public int Get()
    {
        if (Timer.TryGetCurrentTime(_currentTimer, out var t))
        {
            return Mathf.FloorToInt(t);
        }
        return 0;
    }

    public void Dispose()
    {
        Timer.UnRegister(_currentTimer);
    }
}
