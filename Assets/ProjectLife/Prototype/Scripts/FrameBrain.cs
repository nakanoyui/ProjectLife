using UnityEngine;

public class FrameBrain : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private PlantBrain _plantBrain;

    public PlantBrain PlantBrain => _plantBrain;

    public bool IsHitting { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _playerController.gameObject) IsHitting = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(_playerController.tag)) IsHitting = false;
    }
}