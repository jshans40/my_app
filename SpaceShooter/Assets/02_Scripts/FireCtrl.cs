using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireCtrl : MonoBehaviour {
    public List<GameObject> monsterPool = new List<GameObject>();
    [Range(5, 20)]
    public int maxPool = 10;

    public GameObject bullet; //Bullet Prefab
    public Transform firePos;
    public AudioClip fireSfx;
    public MeshRenderer muzzleFlash;

    public float fireRate = 0.2f;
    private float nextFire = 0.0f;

    private AudioSource _audio;


	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
        muzzleFlash.enabled = false;
	}
	
    RaycastHit hit;
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);

        if (Input.GetMouseButton(0)) {
            if (Time.time >= nextFire)
            {
                Fire();

                if (Physics.Raycast(firePos.position, firePos.forward, out hit, 10.0f, 1<<LayerMask.NameToLayer("MONSTER_BODY"))) {
                    Debug.Log("Monster Hit !!!");
                    hit.collider.gameObject.GetComponent<MonsterCtrl>().OnDamage(10, hit.point);
                }


                nextFire = Time.time + fireRate;
            }
        }
	}

    void Fire() {
        Instantiate(bullet, firePos.position, firePos.rotation);
        _audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(this.ShowMuzzleFlash());

       // _audio.clip = fireSfx;
       // _audio.volume = 1.0f;
       // _audio.Play();
    }

    IEnumerator ShowMuzzleFlash() { //IEnumerator : 코르틴 함수를 쓰기위함(싱글쓰레드를 멀티쓰레디와 비슷하게 작동하게 하기위함)
        float angle = Random.Range(0.0f, 360.0f);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        float scale = Random.Range(2.0f, 4.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale; // new Vector3(scale, scale, scale)

        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f)); // 0.2초후 update함수로 감
        muzzleFlash.enabled = false;
    }
}
