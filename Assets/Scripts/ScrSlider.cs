using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrSlider : MonoBehaviour
{
    public float force;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {


        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-force, 0.0f, 0.0f), ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(force, 0.0f, 0.0f), ForceMode.Impulse);
        }


        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                
            }
        }
    }
}
