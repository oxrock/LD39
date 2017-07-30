using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generatorController : MonoBehaviour {
    public Image fuelLevel;
    public Image Health;
    public gameController gController;
    public float maxHealth = 100.0f;
    public float maxFuel = 200.0f;
    public float currentHealth = 20.0f;
    public GameObject generatorHusk;
    float currentFuel = 100.0f;
    float fuelFill = 0.0f;
    float healthFill = 0.0f;

    float calculateFill(float max, float current) {
        if (current <= 0)
        {
            return 0.0f;
        }
        return current / max;
    }

    public void alterHealth(float amount)//- amount for dmg
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            gController.GeneratorAlive = false;
            gController.removeFriendly(gameObject);
            //generator husk!
            Instantiate<GameObject>(generatorHusk, transform.position, transform.rotation);
        }
        healthFill = (calculateFill(maxHealth, currentHealth));
        Health.fillAmount = healthFill;
        
    }

    public void removeFuel(float amount) {
        currentFuel -= amount;
        fuelFill = (calculateFill(maxFuel,currentFuel));
        fuelLevel.fillAmount = fuelFill;
    }
    public void addFuel(float amount) {
        currentFuel += amount;
        if (currentFuel > maxFuel) {
            currentFuel = maxFuel;
        }
        fuelFill = (calculateFill(maxFuel, currentFuel));
        fuelLevel.fillAmount = fuelFill;
    }

    public bool fuelReport() {
        if (currentFuel > 0) {
            return true;
        }
        return false;
    }
	// Use this for initialization
	void Start () {
        Health.fillAmount = calculateFill(maxHealth, currentHealth);
        fuelLevel.fillAmount = calculateFill(maxFuel, currentFuel);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
