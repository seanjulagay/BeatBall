using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour {
    GameManagerScript gameManagerScript;

    void Start() {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    void Update() {
        
    }

    void OnTriggerEnter(Collider other) {
        gameManagerScript.ballOuttaBounds = true;
    }
}
