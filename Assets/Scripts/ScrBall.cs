using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrBall : MonoBehaviour
{
    public Transform paddle;
    ScrPaddle scrPaddle;
    ScrGame scrGame;

    float _nextHitTime = 0.0f;
    Queue<Vector3> _prevPosBall;
    Queue<Vector3> _prevPosPaddle;
    Queue<float> _prevRotPaddle;

    Vector3 _posinit;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject objGame = GameObject.FindGameObjectWithTag("Game");
        if (objGame)
        {
            scrGame = objGame.GetComponent<ScrGame>();
            if(scrGame)
            {
                // register with our game, so it game track state etc
                scrGame.RegisterBall(transform);
            }
        }
        _prevPosBall = new Queue<Vector3>();
        _prevPosPaddle = new Queue<Vector3>();
        _prevRotPaddle = new Queue<float>();
        scrPaddle = paddle.GetComponent<ScrPaddle>();
        _posinit = transform.position;
        GetComponent<Rigidbody>().interpolation = UnityEngine.RigidbodyInterpolation.Interpolate;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (_prevPosBall.Count == 4)// && _prevPosPaddle.Count == 2 && _prevRotPaddle.Count == 2)
        {
            Vector3 lastPosBall = _prevPosBall.Dequeue();
            Vector3 lastPaddlePos = paddle.transform.position;// _prevPosPaddle.Dequeue();
            float angle = paddle.transform.eulerAngles.z;// _prevRotPaddle.Dequeue();

            if (Time.time > _nextHitTime)
            {
                Vector2 hitPointOut = new Vector2();
                bool hit = ScrCollision.PaddleBallCollision(lastPosBall, transform.position, lastPaddlePos + Vector3.up * 0.5f, paddle.transform.position + Vector3.up * 0.5f, 10.0f, angle, paddle.transform.eulerAngles.z, ref hitPointOut);

                if (hit)
                {
                    // paddle - dont keep rising after hit
                    scrPaddle.OnHit();

                    // ball
                    //float jumpMultiplierLoc = _isJumping ? jumpMultiplier : 1.0f;
                    //Vector2 up = new Vector2(transform.up.x, transform.up.y);
                    Vector3 velocityInit = GetComponent<Rigidbody>().velocity;
                    transform.position = hitPointOut;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;

                    // reflection equation: 𝑣1−2𝑛(𝑣1⋅𝑛)
                    Vector3 velocity = velocityInit - (2 * paddle.transform.up) * (Vector3.Dot(velocityInit, paddle.transform.up));
                    // we tightly control jump height (force), using reflection equation for direction only
                    GetComponent<Rigidbody>().AddForce(velocity.normalized * scrPaddle.GetForce());
                    // simplistic way to avoid double hits
                    _nextHitTime = Time.time + 0.25f;
                    // we must sync transforms considering we are using interpolation for the balls - for smoother movement
                    Physics.SyncTransforms();
                }
            }
        }

        _prevPosBall.Enqueue(transform.position);
        _prevPosPaddle.Enqueue(paddle.transform.position);
        _prevRotPaddle.Enqueue(paddle.transform.eulerAngles.z);
    }
}
