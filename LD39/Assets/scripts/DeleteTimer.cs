using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTimer : MonoBehaviour {

    public float DeletionTime = 1.0f;
    float timer = 0.0f;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > DeletionTime) {
            Destroy(gameObject);
        }
	}
}
