using UnityEngine;
using System.Collections;

public class RemoveBullet : MonoBehaviour {

    public GameObject sparkEffect;

    void OnCollisionEnter(Collision coll) {
        if (coll.collider.tag == "BULLET")
        {
            Object obj = Instantiate(sparkEffect, coll.transform.position, Quaternion.identity); //Quaternion은 회전 담당(identity는 그대로)
            Destroy(coll.gameObject);
            //Destroy(this);
            //Destroy(this.gameObject);
            Destroy(obj, 0.2f);
        }
    }
    /* 양쪽에 collider가 존재해야 하며 동체는
     * Rigibody가 있어야 한다*/
}
