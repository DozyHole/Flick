using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrInputTouch : MonoBehaviour
{
    public float moveSensitivity;
    Dictionary<int, Vector2> _touchPositions;
    ScrPaddle _scrPaddle;

    // Start is called before the first frame update
    void Start()
    {
        _scrPaddle = GetComponent<ScrPaddle>();
        _touchPositions = new Dictionary<int, Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_scrPaddle)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _touchPositions.Add(touch.fingerId, touch.position);
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 posInitial = _touchPositions[touch.fingerId];
                    if (posInitial.x < Screen.width / 2.0f)
                    {
                        _scrPaddle.Move(touch.deltaPosition * moveSensitivity);
                    }
                    else
                    {
                        _scrPaddle.Rotate(-touch.deltaPosition.x);
                    }
                }

                if (touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    if (_touchPositions.ContainsKey(touch.fingerId))
                    {
                        Vector2 posInitial = _touchPositions[touch.fingerId];

                        if (posInitial.x > Screen.width / 2.0f)
                        {
                            _scrPaddle.Jump();
                            //if (touch.tapCount > 0)
                            //{
                            //    _scrPaddle.Jump();
                            //}
                        }
                        _touchPositions.Remove(touch.fingerId);
                    }
                }
            }
        }
    }
}
