using UnityEngine;
using System.Collections;

public class BarrelCtrl : MonoBehaviour {

    private int hitCount = 0;
    public GameObject expEffect;
    public Texture[] textures;
    public MeshRenderer _renderer;

    void Start() {
        _renderer = GetComponentInChildren<MeshRenderer>();

        int idx = Random.Range(0, textures.Length);
        _renderer.material.mainTexture = textures[idx];
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.collider.CompareTag("BULLET")) {
            if (++hitCount >= 3) {
                hitCount = -100;
                ExpBarrel();
            }
        }
    }

    void ExpBarrel() {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 1000.0f);
        Destroy(this.gameObject, 2.0f);

        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 3.0f);
    }
           
}
