using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunController : MonoBehaviour {

    public GameObject smokeFab;
    public GameObject bloodSpray;
    public Transform spawnPoint;
    public AudioSource gunshot;
    bool fireRdy = true;
    public float reloadShot = 1.0f;
    float timer = 0.0f;
    public float shotDMG = 100.0f;
    float gunRotSpd = 2.0f;
    Quaternion gunAngle;
    float vertical = 0.0f;
    float zRotMax = 30.0f;
    float zRotMin = -30.0f;
    float zRot = 0.0f;
    float x = 0.0f;
    float y = 0.0f;
    float z = 0.0f;

    void weapontimer() {
        if (!fireRdy) {
            timer += Time.deltaTime;
            if (timer > reloadShot) {
                fireRdy = true;
                timer = 0.0f;
            }
        }
    }

    void shootGun() {
        fireRdy = false;
        GameObject smoke = Instantiate<GameObject>(smokeFab, spawnPoint);
        smoke.transform.position = spawnPoint.transform.position;
        smoke.transform.rotation = spawnPoint.transform.rotation;
        smoke.transform.SetParent(null);
        gunshot.Play();
    }



	// Use this for initialization
	void Start () {
        gunAngle = gameObject.transform.localRotation;
        //Cursor.lockState = true;
        

    }
	
	// Update is called once per frame
	void Update () {
        weapontimer();
        zRot = Input.GetAxis("Mouse Y") * 30 * Time.deltaTime;
        z = transform.eulerAngles.z;
        x = transform.eulerAngles.x;
        y = transform.eulerAngles.y;
        Vector3 desiredRot = new Vector3(x, y, z + zRot);
        if (desiredRot.z > zRotMax && desiredRot.z < 360 - (zRotMax+1) && zRot > 0)// && desiredRot.z  < 360-zRotMax
        {
            desiredRot = new Vector3(x, y, zRotMax);
        }
        else if (desiredRot.z < 360-zRotMax && desiredRot.z > zRotMax && zRot < 0) //desiredRot.z > 360 && 
        {
            desiredRot = new Vector3(x, y, 360-zRotMax);
        }
        transform.rotation = Quaternion.Euler(desiredRot);

    }
    void FixedUpdate()
    {
        
        if (Input.GetButtonDown("Fire"))
        {
            if (fireRdy)
            {
                shootGun();
                RaycastHit hit;
                //hit.transform.rotation = spawnPoint.transform.rotation;
                if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.transform.TransformDirection(Vector3.forward), out hit, 50.0f))
                {
                    if (hit.collider.gameObject.tag == "enemy")
                    {
                        float currentDmg = shotDMG - (hit.distance * 2);
                        if (currentDmg > 0)
                        {
                            hit.collider.transform.GetComponentInParent<healthController>().calculateDmg(shotDMG);
                            GameObject spray = Instantiate<GameObject>(bloodSpray, hit.point, Quaternion.identity);
                        }
                    }
                }
            }
            
        }
        
    }
}
