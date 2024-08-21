using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //게임 개발 디자인 패턴
    //싱글턴 디자인 패턴(Singleton Design Pattern)
    public static GameManager Instance = null;

    public List<Transform> points = new List<Transform>();
    public GameObject monsterPrefab;

    private bool isGameOver = false;

    //오브젝트 풀 정의(선언)
    public List<GameObject> monsterPool = new List<GameObject>();
    //오브젝트 풀의 갯수
    public int maxPool = 10;

    //프로퍼티 선언
    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
            // if (isGameOver)
            // {
            //     CancelInvoke(nameof(CreateMonster));
            // }
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //다른 씬이 오픈 되어도 지속하도록하는 메소드
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            //중복해서 생성된 GameManager 삭제
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        var spawnPointGroup = GameObject.Find("SpawnPointGroup");
        spawnPointGroup.GetComponentsInChildren<Transform>(points);

        CreateMonsterPool();
        StartCoroutine(CreateMonster());
    }

    private void CreateMonsterPool()
    {
        for (int i = 0; i < maxPool; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.name = $"Monster_{i:00}";
            monster.SetActive(false);
            //오브젝트 풀에 추가
            monsterPool.Add(monster);
        }
    }

    IEnumerator CreateMonster()
    {
        yield return new WaitForSeconds(2.0f);
        while (!isGameOver)
        {
            //난수 발생
            int index = UnityEngine.Random.Range(1, points.Count);

            //오브젝트 풀에서 비활성화 된 몬스터를 추출
            foreach (var monster in monsterPool)
            {
                if (monster.activeSelf == false)
                {
                    monster.transform.position = points[index].position;
                    monster.SetActive(true);
                    break;
                }
            }
            //Instantiate(monsterPrefab, points[index].position, Quaternion.identity);

            yield return new WaitForSeconds(3.0f);
        }
    }
}
