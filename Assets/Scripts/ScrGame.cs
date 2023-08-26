using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrGame : MonoBehaviour
{
    List<Transform> _balls;

    private void Awake()
    {
        _balls = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // check game conditions for all balls in play
        _balls.RemoveAll(item => item.position.y < -25.0f);
        if (_balls.Count == 0)
        {
            // reset scene - game over
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RegisterBall(Transform ball)
    {
        _balls.Add(ball);
    }

    public void UnregisterBall(Transform ball)
    {
        _balls.Remove(ball);
    }
}
