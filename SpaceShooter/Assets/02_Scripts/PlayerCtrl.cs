using UnityEngine;
using System.Collections;

[System.Serializable]
public class Anims {
	public AnimationClip idle;
	public AnimationClip runForward;
	public AnimationClip runBackward;
	public AnimationClip runRight;
	public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour {
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

	private Transform tr;
	private Animation anim;

	public Anims anims;

	public float moveSpeed = 10.0f;
	public float rotSpeed = 80.0f;

    private float initHp = 100.0f;
    private float currHp = 100.0f;
	
	// Start함수 호출 전 호출 ex) start에서 초기화 하기 전 동기화할 때 사용
	/*void Awake() {
	
	}

	// 활성/비활성 시 호출
	void OnEnable() {
 
	}*/

	// 초기화
	void Start () {
		//tr = this.gameObject.GetComponent("Transform") as Transform;
		//tr = this.gameObject.GetComponent<Transform>();
		tr = GetComponent<Transform>();
		anim = GetComponent<Animation>();
		anim.clip = anims.idle;
		anim.Play();
	}
	
	// 매 프레임 호출
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		float r = Input.GetAxis("Mouse X");

		//Debug.Log("h = " + h);
		//Debug.Log("v = " + v);
		 
		//transform.position += new Vector3(0.0f, 0.0f, 0.1f);

		Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
		tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed); //디바이스마다 프레임이 다르므로 Time.deltaTime을 곱해준다
		//transform.Translate(Vector3.forward * 0.1f * v);
		//transform.Translate(Vector3.right * 0.1f * h);

		transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

		if (v >= 0.1f)
		{
			anim.CrossFade(anims.runForward.name, 0.3f); // 애니메이션의 변경시 자연스럽게 하기 위한 것
		}
		else if (v <= -0.1f)
		{
			anim.CrossFade(anims.runBackward.name, 0.3f);
		}
		else if (h >= 0.1f)
		{
			anim.CrossFade(anims.runRight.name, 0.3f);
		}
		else if (h <= -0.1f)
		{
			anim.CrossFade(anims.runLeft.name, 0.3f);
		}
		else
		{
			anim.CrossFade(anims.idle.name, 0.3f);
		}
	}

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "PUNCH")
        {
            currHp -= 10.0f;
            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie() {
        Debug.Log("Player Die");
        OnPlayerDie();
        GameMgr.instance.isGameOver = true;

        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        //foreach (GameObject monster in monsters)
        //{
        //    monster.SendMessage("MonsterWin", SendMessageOptions.DontRequireReceiver);
        //}
    }

	// 주기가 필요한 경우 사용 ex)총알
	/*void FixedUpdate() {
	
	}

	// Update 함수에서 선처리 후 LateUpdate에서 후처리
	void LateUpdate() {
	
	}*/

}
