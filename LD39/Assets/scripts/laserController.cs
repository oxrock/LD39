using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserController : MonoBehaviour {

    public Transform startPoint;
    public Transform endPoint;
    public LineRenderer laserLine;
    public Transform Head;
    public float maxDistance = 100.0f;
    public gameController gController;
    public AudioSource humm;
    public GameObject bloodSpray;
    public generatorController genCon;
    public Material redOn;
    public Material redOff;
    public Material greenOn;
    public Material greenOff;
    public MeshRenderer offIndicator;
    public MeshRenderer onIndicator;
    public int dps = 25;
    public bool activated = true;
    public GameObject huskFab;
    bool shotRdy = true;
    float shotTimer = 0.0f;
    float shotReload = 1.0f;
    public float fuelPerShot = 1.0f;
    public float fuelPerSecond = 0.5f;
    float energyTimer = 0.0f;
    float energyMaintTimer = 1.0f;
    float maxHealth = 100;
    float currentHealth = 100;


    public void changeHealth(float amount) {// -amount to take dmg
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0) {
            
            if (gameObject != null)
            {
                gController.removeFriendly(gameObject);
                currentHealth = 0;
            }
            //replace with husk!
            
        }
    }
    public void Death() {
        GameObject husk = Instantiate<GameObject>(huskFab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public float getHealth() {
        return currentHealth;
    }

    void maintenanceHandler() {
        energyTimer += Time.deltaTime;
        if (energyTimer > energyMaintTimer) {
            energyTimer = 0.0f;
            genCon.removeFuel(fuelPerSecond);
        }
        if (!genCon.fuelReport())
        {
            if (activated)
            {
                toggleActivation();
            }
        }
        else if (!gController.GeneratorAlive) {
            toggleActivation();
        }
    }

    public void toggleActivation() {
        activated = !activated;
        if (activated && gController.GeneratorAlive)
        {
            offIndicator.material = redOff;
            onIndicator.material = greenOn;
        }
        else {
            offIndicator.material = redOn;
            onIndicator.material = greenOff;
            laserLine.SetPosition(1, startPoint.position);
            laserLine.SetPosition(0, startPoint.position);


        }
    }

    void damageHandler() {
        if (shotRdy)
        {
            if (endPoint != null)
            {
                endPoint.transform.GetComponentInParent<healthController>().calculateDmg(dps);
                GameObject spray = Instantiate<GameObject>(bloodSpray, endPoint.position, Quaternion.identity);
                shotTimer = 0.0f;
                shotRdy = false;
                genCon.removeFuel(fuelPerShot);
            }
        }
        else {
            shotTimer += Time.deltaTime;
            if (shotTimer > shotReload) {
                shotRdy = true;
            }
        }

        
    }

	// Use this for initialization
	void Start () {
        //laserLine.SetWidth(.2f,.2f);
        laserLine.startWidth = .4f;
        laserLine.endWidth = .4f;
        //Application.runInBackground = true;
        toggleActivation();
    }

    void findTarget() {
        GameObject temp = null;
        float lowest = Mathf.Infinity;
        float tempLowest = Mathf.Infinity;
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject E in gController.enemies) {
            if (E != null)
            {
                tempLowest = Vector3.Distance(startPoint.position, E.transform.position);
                if (tempLowest < lowest)
                {
                    lowest = tempLowest;
                    temp = E;
                }
            }
        }
        if (lowest < maxDistance) {
            if (temp != null) {
                endPoint = temp.transform;
                //Debug.Log("Happened");
            }
        }
        //Debug.Log(lowest);
    }


    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (endPoint != null)
            {
                Head.LookAt(endPoint.position);
                laserLine.SetPosition(0, startPoint.position);
                laserLine.SetPosition(1, endPoint.position);
                if (!humm.isPlaying)
                {
                    humm.Play();
                }
            }
            else
            {
                humm.Pause();
                findTarget();
                laserLine.SetPosition(0, startPoint.position);
                laserLine.SetPosition(1, startPoint.position);

            }
            damageHandler();
            maintenanceHandler();
        }
        else {
            if (humm.isPlaying) {
                humm.Pause();
            }
        }
    }
}
