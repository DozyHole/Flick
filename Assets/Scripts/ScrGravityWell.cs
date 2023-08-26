using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrGravityWell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.drag = 0.95f;
            //rb.velocity *= 0.5f;
            //rb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.drag = 0.1f;
            //rb.useGravity = true;
        }
    }

    //void OnFixedUpdate()
    //{
    //    Rigidbody rb = other.GetComponent<Rigidbody>();
    //    if (rb)
    //    {
    //        rb.velocity *= 0.95f;
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        

        if (rb)
        {
            //rb.velocity *= 0.95f;
            Vector3 dir = transform.position - other.transform.position;
            float dist = dir.magnitude;

            if(dist < 0.5f)
            {
                // complete
                Destroy(other.gameObject);
                Destroy(gameObject);
                return;
            }


            float force = 4.0f - dist;
            
            if (force > 0.0f)
            {
                //print(dist/2.0f);
                //float scale = dist/2.0f;
                //this.transform.localScale = new Vector3(scale, scale, 1.0f);
                //force *= force;
                dir.Normalize();
                rb.AddForce(50.0f * force * dir, ForceMode.Acceleration);

                

            }
        }
    }
}
