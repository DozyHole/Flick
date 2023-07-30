using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrInputKeys : MonoBehaviour
{
    public float moveSensitivity;
    ScrPaddle _scrPaddle;

    // Start is called before the first frame update
    void Start()
    {
        _scrPaddle = GetComponent<ScrPaddle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_scrPaddle)
        {
            // input - keys
            if (Input.GetKey(KeyCode.A))
            {
                _scrPaddle.Move(new Vector3(-moveSensitivity, 0.0f, 0.0f));
            }

            if (Input.GetKey(KeyCode.D))
            {
                _scrPaddle.Move(new Vector3(moveSensitivity, 0.0f, 0.0f));
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _scrPaddle.Rotate(4.0f);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _scrPaddle.Rotate(-4.0f);
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                _scrPaddle.Jump();
            }
        }
    }
}
