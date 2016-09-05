using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterCtrl : MonoBehaviour {

    public enum State
    {
        IDLE = 0,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;
    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    public bool isDie = false;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator anim;
    //Hash Table Index
    private int attackIdx;
    private int hitIdx;

    [HideInInspector]
    public GameObject bloodEffect;
    private GameObject bloodDecal;

    private int hp = 100;

    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += MonsterWin;
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }

    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= MonsterWin;
    }

	// Use this for initialization
	void Awake () {
        attackIdx = Animator.StringToHash("IsAttack");
        hitIdx = Animator.StringToHash("Hit");

        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        //bloodEffect = Resources.Load("BloodEffect") as GameObject;
        bloodDecal = Resources.Load<GameObject>("Prefabs/BloodDecal");
        bloodEffect = Resources.Load<GameObject>("Prefabs/BloodEffect");
        //Resourcsd.LoadAll<GameObject>("Items/");로 하면 Items 아래있는 모든것을 가져옴
    }

    IEnumerator CheckMonsterState() {
        while (!isDie)
        {
            //Check Monster State
            float dist = Vector3.Distance(monsterTr.position, playerTr.position);

            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator MonsterAction() {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    anim.SetBool("IsTrace", false);
                    nvAgent.Stop();
                    break;
                case State.TRACE:
                    //Debug.Log("Trace !!!");
                    anim.SetBool("IsTrace", true);
                    anim.SetBool(attackIdx, false);
                    nvAgent.SetDestination(playerTr.position);
                    nvAgent.Resume();
                    break;
                case State.ATTACK:
                    anim.SetBool(attackIdx, true);
                    nvAgent.Stop();
                    break;
                case State.DIE:
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.collider.CompareTag("BULLET")) { 
            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;

            ShowBloodEffect(coll.transform.position);
            Destroy(coll.gameObject);
            anim.SetTrigger(hitIdx);

            if (hp <= 0)
            {
                isDie = true;
                StopAllCoroutines();
                nvAgent.Stop();
                anim.SetTrigger("Die");
                GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }

    public void OnDamage(int damage, Vector3 pos) {
        hp -= damage;
        ShowBloodEffect(pos);
        anim.SetTrigger(hitIdx);

        if (hp <= 0)
        {
            isDie = true;
            StopAllCoroutines();
            nvAgent.Stop();
            anim.SetTrigger("Die");
            GetComponent<CapsuleCollider>().enabled = false;
            Invoke("ReturnPool", 3.0f);

            //GameObject.Find("GameManager").GetComponent<GameMgr>().DispScore(50);
            GameMgr.instance.DispScore(50);
        }
    }

    void ReturnPool() {
        isDie = false;
        hp = 100;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.SetActive(false);
    }
       
    void ShowBloodEffect(Vector3 pos) {
        Object obj = Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(obj, 3.0f);

        Quaternion rot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        GameObject obj2 = Instantiate(bloodDecal, monsterTr.position + (Vector3.up * 0.1f), rot) as GameObject;

        obj2.transform.localScale = Vector3.one * Random.Range(1.5f, 5.0f);

        Destroy(obj2, 5.0f);
    }

    public void MonsterWin() {
        isDie = true;
        StopAllCoroutines();
        nvAgent.Stop();

        anim.SetTrigger("PlayerDie");
    }
}
