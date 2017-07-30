using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInteraction : MonoBehaviour {

    laserController LC = null;
    generatorController GC = null;
    public AudioSource gasPickup;
    public AudioSource pouringGas;
    public gameController gController;
    public playerStats PS;
    public Text helpMessage;
    string functioningMessage = "Press interact key to enable/disable turret";
    string brokenMessage = "This turret needs repairs";
    string gasFillMessage = "Press interact key to empty your collected gas into the generator";
    string noGasMessage = "Collect gas cans to refill the generator";

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "turret")
        {
            if (gController.GeneratorAlive)
            {
                //Debug.Log("found turret!");
                //findHighestTurretParent(other.transform);
                LC = other.GetComponent<laserController>();
                if (LC.getHealth() > 0)
                {
                    helpMessage.text = functioningMessage;
                }
                else
                {
                    helpMessage.text = brokenMessage;
                }
                helpMessage.enabled = true;

            }
        }

        else if (other.tag == "Gas")
        {
            PS.addFuel(10);
            gasPickup.Play();
            Destroy(other.gameObject);
        }
        else if (other.tag == "Generator")
        {
            if (gController.GeneratorAlive)
            {
                if (PS.getFuel() > 0)
                {
                    GC = other.GetComponent<generatorController>();
                    helpMessage.text = gasFillMessage;
                    helpMessage.enabled = true;
                }
                else
                {
                    GC = other.GetComponent<generatorController>();
                    helpMessage.text = noGasMessage;
                    helpMessage.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "turret")
        {
            LC = null;
            helpMessage.enabled = false;
        }
        else if (other.tag == "Generator") {
            GC = null;
            helpMessage.enabled = false;
        }
    }

    private void Update()
    {
        if (LC != null)
        {
            if (Input.GetButtonDown("interact"))
            {
                LC.toggleActivation();
            }
        }
        else if (GC != null) {
            if (Input.GetButtonDown("interact"))
            {
                if (PS.getFuel() > 0) {
                    GC.addFuel(PS.getFuel());
                    helpMessage.text = noGasMessage;
                    PS.removeFuel(PS.getFuel());
                    pouringGas.Play();
                }
            }
        }
    }


    void findHighestTurretParent(Transform suspect) {
        while (LC == null) { 
            LC = suspect.GetComponent<laserController>();
            suspect = suspect.parent;
        }
    }

}
