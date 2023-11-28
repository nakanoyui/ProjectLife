using UnityEngine;

/// <summary>
///     PlayerController
/// </summary>
[RequireComponent(typeof(ItemSelecter))]
public class PlayerController : MonoBehaviour
{
    private readonly float _speed = 5.0f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var joyStickDir = new Vector3(JoyStick.JoyStickX, 0, JoyStick.JoyStickY).normalized;

        transform.position += joyStickDir * _speed * Time.deltaTime;

        if (joyStickDir.sqrMagnitude > 0.1f) transform.rotation = Quaternion.LookRotation(joyStickDir);
    }
}