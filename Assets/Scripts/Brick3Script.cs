using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick3Script : MonoBehaviour {
    GameManagerScript gameManagerScript;
    
    public int hits = 5;
    public int points = 20;
    public Vector3 rotator;
    
    public Material hitMaterial;
    public Material hits1Material;
    public Material hits2Material;
    public Material hits3Material;
    public Material hits4Material;

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
        if(hits == 4) {
            brickRenderer.sharedMaterial = hits3Material;
            GameManagerScript.Instance.Score += 3;
        } else if(hits == 3) {
            brickRenderer.sharedMaterial = hits4Material;
            GameManagerScript.Instance.Score += 5;
        } else if(hits == 2) {
            brickRenderer.sharedMaterial = hits2Material;
            GameManagerScript.Instance.Score += 10;
        } else if(hits == 1) {
            brickRenderer.sharedMaterial = hits1Material;
            GameManagerScript.Instance.Score += 15;
        } else {
            GameManagerScript.Instance.Score += points;
            Destroy(gameObject);
        }
    }
}