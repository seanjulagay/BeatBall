using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {
    public float speed = 40f;
    Rigidbody rb;
    Vector3 velocity, xMovement;
    int startingMovement;
    float xStartingMovementSpeed = 5f;
    Renderer renderer;

    void DirectionOnStart() {
        startingMovement = Random.Range(0, 2);

        if(startingMovement == 0) {
            xMovement = Vector3.right * -xStartingMovementSpeed;
        } else {
            xMovement = Vector3.right * xStartingMovementSpeed;
        }
    }

    public void LaunchBall() {
        DirectionOnStart();
        rb.velocity = (Vector3.up * speed) + xMovement;
    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();

        Invoke("LaunchBall", 0.5f);
    }

    void Update() {
        
    }

    void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * speed;
        velocity = rb.velocity;

        if(!renderer.isVisible) {
            GameManagerScript.Instance.Balls--;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other) {
        rb.velocity = Vector3.Reflect(velocity, other.contacts[0].normal);
        if(other.gameObject.name == "Paddle") {
            Debug.Log("Hit paddle");
            Rigidbody paddleRb = other.gameObject.GetComponent<Rigidbody>();
            if(paddleRb.velocity.x < 0.5f || paddleRb.velocity.x > 0.5f) {
                Debug.Log("Added paddle force");
                rb.velocity = new Vector3((rb.velocity.x + (paddleRb.velocity.x / 2)), rb.velocity.y, 0);
            }
        }

        GetComponent<AudioSource>().Play();
    }
}
