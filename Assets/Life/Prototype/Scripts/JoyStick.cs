using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// JoyStick
/// </summary>
public class JoyStick : MonoBehaviour
{
    public GameObject _joyStick;

    private RectTransform _joyStickRectTransform;

    public GameObject _backGround;

    public int _stickRange = 3;

    private int _stickMovement;

    public static float _joyStickX;
    public static float _joyStickY;

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        _stickMovement = _stickRange * (Screen.width + Screen.height) / 100;

        _joyStickRectTransform = _joyStick.GetComponent<RectTransform>();

        JoyStickDisplay(false);
    }

    private void JoyStickDisplay(bool _isShow)
    {
        _backGround.SetActive(_isShow);
        _joyStick.SetActive(_isShow);
    }

    public void Move(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;

        float x = _backGround.transform.position.x - pointer.position.x;
        float y = _backGround.transform.position.y - pointer.position.y;

        float angle = Mathf.Atan2(y, x);

        if (Vector2.Distance(_backGround.transform.position, pointer.position) > _stickMovement)
        {
            y = _stickMovement * Mathf.Sin(angle);
            x = _stickMovement * Mathf.Cos(angle);
        }

        _joyStickX = -x / _stickMovement;
        _joyStickY = -y / _stickMovement;

        _joyStick.transform.position = new Vector2(_backGround.transform.position.x - x,_backGround.transform.position.y - y);
    }

    public void PointerDown(BaseEventData data)
    {
        PointerEventData pointer = data as PointerEventData;
        
        JoyStickDisplay(true);

        _backGround.transform.position = pointer.position;
    }

    public void PointerUp(BaseEventData data)
    {
        PositionInitialization();
        
        JoyStickDisplay(false);
    }

    private void PositionInitialization()
    {
        _joyStickRectTransform.anchoredPosition = Vector2.zero;

        _joyStickX = 0;
        _joyStickY = 0;
    }
}
