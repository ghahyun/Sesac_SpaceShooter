using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterController : MonoBehaviour
{
    public enum State
    {
        IDLE, TRACE, ATTACK, DIE
    }

    // 현재 몬스터의 상태
    public State state = State.IDLE;
    // 추적 사정거리
    [SerializeField] private float traceDist = 10.0f;
    // 공격 사정거리
    [SerializeField] private float attackDist = 2.0f;

    private Transform playerTr;
    private Transform monsterTr;
    private NavMeshAgent agent;
    private Animator animator;
    private readonly int hashIsTrace = Animator.StringToHash("IsTrace");
    private readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashDanceSpeed = Animator.StringToHash("DanceSpeed");

    public bool isDie = false;

    private float hp = 100.0f;

    void OnEnable()
    {
        PlayerController.OnPlayerDie += YouWin;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    void OnDisable()
    {
        PlayerController.OnPlayerDie -= YouWin;
    }

    void Awake()
    {
        //GameObject playerObj = GameObject.Find("Player");

        GameObject playerObj = GameObject.FindGameObjectWithTag("PLAYER");
        playerTr = playerObj?.GetComponent<Transform>();

        monsterTr = transform; //monsterTr = GetC omponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    IEnumerator CheckMonsterState()
    {
        while (isDie == false)
        {
            //몬스터 상태가 DIE일 경우 해당 코루틴을 정지
            if (state == State.DIE) yield break;

            //상태값을 측정
            float distance = Vector3.Distance(monsterTr.position, playerTr.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
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

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case State.IDLE:
                    // 아이들링일 경우 로직처리
                    agent.isStopped = true;
                    animator.SetBool(hashIsTrace, false);
                    break;

                case State.TRACE:
                    // 추적 상태일 때 로직처리
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false; // 추적,이동 상태
                    animator.SetBool(hashIsAttack, false);
                    animator.SetBool(hashIsTrace, true);
                    break;

                case State.ATTACK:
                    agent.isStopped = true;
                    animator.SetBool(hashIsAttack, true);
                    break;

                case State.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    animator.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;
                    //오브젝트 풀에 환원
                    Invoke(nameof(ReturnPool), 3.0f);
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    void ReturnPool()
    {
        hp = 100.0f;
        isDie = false;
        state = State.IDLE;
        GetComponent<CapsuleCollider>().enabled = true;
        this.gameObject.SetActive(false);
    }

    // void OnCollisionEnter(Collision coll)
    // {
    //     if (coll.gameObject.CompareTag("BULLET"))
    //     {
    //         PoolManager.Instance.bulletPool.Release(coll.gameObject.GetComponent<Bullet>());
    //     }
    // }

    public void OnDamaged()
    {
        animator.SetTrigger(hashHit);
        hp -= 20.0f;
        if (hp <= 0.0f)
        {
            state = State.DIE;
        }
    }

    public void YouWin()
    {
        animator.SetFloat(hashDanceSpeed, Random.Range(0.8f, 1.5f));
        animator.SetTrigger(hashPlayerDie);
        StopAllCoroutines();
        agent.isStopped = true;
    }

}
