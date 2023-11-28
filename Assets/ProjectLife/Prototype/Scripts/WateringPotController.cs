using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

/// <summary>
///     WateringPotController
/// </summary>
public class WateringPotController : MonoBehaviour
{
    [SerializeField] private ActionButton _actionButton;

    [SerializeField] private GameObject _frames;

    private List<FrameBrain> _frameBrains;

    private void Start()
    {
        _frameBrains = _frames.GetComponentsInChildren<FrameBrain>().ToList();

        OnEvent();
    }

    private void OnEvent()
    {
        _actionButton.OnPlantsClickWateringButton.Subscribe(_ =>
        {
            foreach (var brain in _frameBrains)
                if (brain.IsHitting)
                {
                    brain.PlantBrain.OnTakeWater.OnNext(true);
                    break;
                }
        });
    }
}