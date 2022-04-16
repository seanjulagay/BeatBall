using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour {
    public int hits = 1;
    public int points = 10;
    public Vector3 rotator;
    
    public Material hitMaterial;
    Material origMaterial;
    Renderer brickRenderer;

    void Start() {
        transform.Rotate(rotator * (transform.position.x + transform.position.y));
        brickRenderer = gameObject.GetComponent<Renderer>();
        origMaterial = brickRenderer.sharedMaterial;
    }

    void Update() {
        transform.Rotate(rotator * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other) {
        gameObject.GetComponent<MeshRenderer>();
        hits--;
        brickRenderer.sharedMaterial = hitMaterial;
        Invoke("DestroyObject", 0.05f);
    }

    void DestroyObject() {
        if(hits <= 0) {
            GameManagerScript.Instance.Score += points;
            Destroy(gameObject);
        }
    }
}