using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : MonoBehaviour {

    public GameObject explosionFab;
    public float damage;
    public Transform target;
    Vector3 targetPos;
    float lethalRange = 0.5f;
    public float spd;
    string _tag = "";
    bool alive = true;
    

    public void setTarget(Transform _target,float _damage) {
        if (_target != null) {
            target = _target;
            targetPos = _target.position;
            targetPos.y = targetPos.y + 1.0f;
            _tag = _target.tag;
            damage = _damage;
        }
    }

    void movement()
    {
        if (alive)
        {
            if (target != null)
            {
                transform.LookAt(target);
                targetPos = target.position;
                targetPos.y = targetPos.y + 1.0f;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, spd * Time.deltaTime);
        }
    }
    void explosionLogic() {
        if (alive)
        {
            if (Vector3.Distance(transform.position, targetPos) < lethalRange)
            {

                if (target != null)
                {
                    if (_tag == "Generator")
                    {
                        target.GetComponent<generatorController>().alterHealth(-damage);
                    }
                    else if (_tag == "Player")
                    {
                        target.GetComponent<playerStats>().alterHealth(-damage);
                    }
                    else if (_tag == "turret")
                    {
                        target.GetComponent<laserController>().changeHealth(-damage);
                    }
                    else
                    {
                        Debug.Log("my target isn't listed: " + _tag);
                    }

                }
                GameObject _explosion = Instantiate<GameObject>(explosionFab, transform.position, Quaternion.identity);
                alive = false;
                Destroy(gameObject);

            }
        }
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        movement();
        explosionLogic();
	}
}
