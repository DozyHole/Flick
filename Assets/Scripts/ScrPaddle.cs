using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrPaddle : MonoBehaviour
{
    public Transform ball;
    public float force;
    public float jumpForce;
    public float jumpMultiplier;
    public float turnSensitivity;

    Vector3 _velocity;
    Queue<Vector3> _ballPositions;
    Queue<Vector3> _lastPositions;
    Queue<float> _lastAngles;
    bool _isJumping = false;
    float _nextHitTime = 0.0f;
    float _nextJumpTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        //_ballPositions = new Queue<Vector3>();
        //_lastPositions = new Queue<Vector3>();
        //_lastAngles = new Queue<float>();
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
    }

    void UpdateVelocity()
    {
        // velocity
        transform.position = transform.position + _velocity;

        // gravity
        if (transform.position.y > 0.0f)
        {
            _velocity = new Vector3(_velocity.x, _velocity.y + (Physics.gravity.y * Time.deltaTime * 0.125f), _velocity.z);
        }

        // clamp to ground
        if (transform.position.y < 0.0f)
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            _velocity = Vector3.zero;
            _isJumping = false;
        }
    }

    public void OnHit()
    {
        // paddle - dont keep rising after hit
        _velocity = Vector3.zero;
    }

    public float GetForce()
    {
        return force * (_isJumping ? jumpMultiplier : 1.0f);
    }

    public void Move(Vector2 delta)
    {
        transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime, transform.position.y, transform.position.z);
    }

    public void Rotate(float value)
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, value * Time.deltaTime * turnSensitivity));
        if (transform.rotation.eulerAngles.z > 30.0f && transform.rotation.eulerAngles.z < 90.0f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 30.0f));
        }
        if (transform.rotation.eulerAngles.z < 330.0f && transform.rotation.eulerAngles.z > 270.0f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 330.0f));
        }
    }

    public void Jump()
    {
        if (Time.time > _nextJumpTime)
        {
            _velocity = new Vector3(_velocity.x, _velocity.y + jumpForce, _velocity.z);
            _nextJumpTime = Time.time + 0.5f;
            _isJumping = true;
        }
    }
}
