using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick2Script : MonoBehaviour {
    GameManagerScript gameManagerScript;
    
    public int hits = 2;
    public int points = 20;
    public Vector3 rotator;
    
    public Material hitMaterial;
    public Material hits1Material;
    Material origMaterial;
    Renderer brickRenderer;

    void Start() {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
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
        Invoke("HitsHandler", 0.1f);
    }

    void HitsHandler() {
        if(hits <= 0) {
            GameManagerScript.Instance.Score += points;
            Destroy(gameObject);
        } else if(hits == 1) {
            brickRenderer.sharedMaterial = hits1Material;
            GameManagerScript.Instance.Score += 5;
        }
    }
}