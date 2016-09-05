using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour {
    public static GameMgr instance = null;

    public Text scoreText;
    public Image hpBar;

    public List<GameObject> monsterPool = new List<GameObject>();
    [Range(5,20)]
    public int maxPool = 10;

    public GameObject monster;
    public Transform[] points;
    public float createTime = 3.0f;

    public bool isGameOver = false;

    private int highScore = 0;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        highScore = PlayerPrefs.GetInt("SCORE", 0);
        DispScore(0);

        //Make MonsterPool
        for (int i = 0; i < maxPool; i++)
        {
            GameObject _monster = Instantiate(monster) as GameObject;
            _monster.name = string.Format("Monster_{0}", i.ToString("00"));
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        //InvokeRepeating("CreateMonster", 1.0f, createTime);
        StartCoroutine(CreateMonster());
    }

    IEnumerator CreateMonster()
    {
        while(!isGameOver) {
            int idx = Random.Range(1, points.Length);

            foreach (GameObject _monster in monsterPool)
            {
                if (!_monster.activeSelf)
                {
                    _monster.transform.position = points[idx].position;
                    _monster.transform.LookAt(points[0].position);
                    _monster.SetActive(true);
                    break;
                }
            }

            //GameObject _monster = Instantiate(monster) as GameObject;
            //_monster.transform.position = points[idx].position;
            //_monster.transform.rotation = Quaternion.LookRotation(points[0].position - _monster.transform.position);//몬스터가 화면 가운데를 항상 바라보게 하기 위함.

 //           _monster.transform.LookAt(points[0].position);
            yield return new WaitForSeconds(createTime);
        }
    }

    public void DispScore(int score) {
        highScore += score;

        PlayerPrefs.SetInt("SCORE", highScore);
        scoreText.text = "SCORE:" + highScore.ToString("0000");
    }
}