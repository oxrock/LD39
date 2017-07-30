using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthController : MonoBehaviour {

    public int maxHealth = 100;
    public int health = 100;
    public int armor = 10;
    public gameController gController;
    public GameObject gasCanFab;
    public int randomSlider = 5;
    public int scoreValue = 5;
    public bool replacement = false;
    public GameObject husk;

    public void calculateDmg(float dmg) {
        float mitigated = dmg * (armor / 100.0f);
        health -= Mathf.FloorToInt((dmg - mitigated));
    }

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {
            gController.removeMonster(gameObject);
            if (Random.Range(0, randomSlider) == 1) {
                GameObject gas = Instantiate<GameObject>(gasCanFab, gameObject.transform);
                gas.transform.position = transform.position;
                gas.transform.parent = null;
                if (replacement) {
                    Instantiate<GameObject>(husk,transform.position,transform.rotation);
                }
                //Debug.Log("spawned gas");
            }
            gController.adjustScore(scoreValue);
            Destroy(gameObject);
        }
	}
}
