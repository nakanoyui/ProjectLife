using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    // 一日の時間
    [SerializeField] private int _oneDayMiliSecounds = 5000;

    private readonly UnityEvent _onCallBack = new();

    private readonly Subject<bool> _onDayElapsed = new();
    public int DayElapsedCounter { get; set; }

    public IObservable<bool> OnDayElapsed => _onDayElapsed;

    private void Start()
    {
        _onCallBack.AddListener(ComputeOneDay);

        _onCallBack.Invoke();
    }

    private async void ComputeOneDay()
    {
        await ComputeOneDayAsync();

        // 一日が過ぎた
        DayElapsedCounter++;

        _onDayElapsed.OnNext(true);

        Debug.Log(DayElapsedCounter + "日が経過した");

        _onCallBack.Invoke();
    }

    private async UniTask ComputeOneDayAsync()
    {
        await UniTask.Delay(_oneDayMiliSecounds);
    }
}