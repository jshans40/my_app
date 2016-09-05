using UnityEngine;
using System.Collections;

public class BulletCtrl : MonoBehaviour {

    public float speed = 800.0f;
    public Rigidbody rb;
    public int damage = 10;

	// Use this for initialization
	void Start () {
        //rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * speed);
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
