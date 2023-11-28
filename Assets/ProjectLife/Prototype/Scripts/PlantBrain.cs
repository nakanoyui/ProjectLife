using UniRx;
using UnityEngine;

public class PlantBrain : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    // 成長するまでの日数
    [SerializeField] private int _growDays;

    // 枯れるまでの日数
    private int _blastedDays;

    // 水をあげる事ができるか
    private bool _canWatering = true;

    // 水をあげた回数
    private int _wateringCounter;

    public Subject<bool> OnTakeWater { get; } = new();

    private void Start()
    {
        OnEvent();
    }

    private void OnEvent()
    {
        _gameController.OnDayElapsed.Subscribe(_ => { _canWatering = true; });

        OnTakeWater.Subscribe(_ =>
        {
            if (_canWatering)
            {
                _wateringCounter++;
                _canWatering = false;

                Debug.Log(_wateringCounter + "回水を与えた。");

                if (_wateringCounter >= _growDays) Debug.Log("植物が育った。");
            }
        });
    }
}