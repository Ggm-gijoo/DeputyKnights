using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharBase : MonoBehaviour, IHittable
{
    [HideInInspector] public bool isPlayer = false;
    [HideInInspector] public bool IsDead { get; protected set; } = false;
    [HideInInspector] public float dealStack = 0;
    [HideInInspector] public static float totalDealStack = 0;
    [HideInInspector] public CharBase targetObject = null;

    public CharacterSO charSO;
    [Header("공격 범위")]
    [SerializeField] protected float attackDistance; //공격 범위
    [Header("도망 범위")]
    [SerializeField] protected float escapeDistance; //도망 범위

    [Space]
    [Header("애니메이션 클립")]
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip moveClip;
    [SerializeField] private AnimationClip attackClip;
    protected Animator anim;
    protected AnimatorOverrideController overrideController;

    [HideInInspector] public float hp;
    [HideInInspector] public float atk;
    [HideInInspector] public float spd;

    protected bool isAct = false;

    protected Rigidbody2D rigid;
    protected HPBar hpBar;

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        StartCoroutine(SetAct());
    }

    private void Update()
    {
        RestrictionPos();
    }

    protected virtual void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        hpBar = GetComponentInChildren<HPBar>();

        SetTeam();
        SetStatus();
        SetAnimation();

        TeamManager.Instance.onDieEvent.RemoveListener(ResetTarget);
        TeamManager.Instance.onDieEvent.AddListener(ResetTarget);
    }
    private void SetTeam()
    {
        if (isPlayer)
            TeamManager.Instance.playerTeamList.Add(GetComponent<PlayerCharBase>());
        else
            TeamManager.Instance.enemyTeamList.Add(this);
    }
    private void SetStatus()
    {
        job = charSO.Job;
        hp = charSO.Hp;
        atk = charSO.Atk;
        spd = charSO.Spd;
    }
    private void SetAnimation()
    {
        overrideController = new AnimatorOverrideController();

        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (idleClip != null) overrideController["Idle"] = idleClip;
        if (moveClip != null) overrideController["Move"] = moveClip;
        if (attackClip != null) overrideController["Attack"] = attackClip;

        anim.runtimeAnimatorController = overrideController;
    }

    public void OnDamage(float damage, float critChance = 0, CharBase from = null)
    {
        if (IsDead) return;

        float critRate = Random.Range(0f, 100f);
        bool isCrit = critChance > critRate;

        damage *= isCrit ? 1.5f : 1f;
        hp -= damage;
        ShowDmgPopup.Instance.ShowDmg(damage, gameObject, isCrit);

        if (from != null)
        {
            SetTarget(from);
            from.dealStack += damage;
            totalDealStack += damage;

            TeamManager.Instance.OnHitEvent.Invoke();
        }

        hpBar.UpdateHpBar();

        if (hp <= 0)
        {
            IsDead = true;
            TeamManager.Instance.onDieEvent.Invoke();
            StartCoroutine(OnDie());
        }
    }

    protected virtual IEnumerator SetAct()
    {
        while (!IsDead)
        {
            yield return null;
            if (isAct) continue;

            float targetDist = Vector2.Distance(targetObject.transform.position, transform.position);

            switch(targetDist)
            {
                case float dist when dist < escapeDistance:
                    Escape();
                    break;

                case float dist when dist < attackDistance:
                    Attack();
                    break;

                default:
                    Chase();
                    break;
            }
        }

        rigid.velocity = Vector2.zero;
    }

    protected virtual void Attack() 
    {
        rigid.velocity = Vector2.zero;
    }
    protected virtual void Chase() 
    {
        Vector2 dir = (targetObject.transform.position - transform.position).normalized;
        rigid.velocity = dir * spd;
    }
    protected virtual void Escape() 
    {
        if(IsCharacterInEdge())
        {
            Attack();
            return;
        }    
        Vector2 dir = (transform.position - targetObject.transform.position).normalized;
        rigid.velocity = dir * spd;
    }

    protected virtual IEnumerator OnDie()
    {
        Managers.Pool.PoolManaging("DeathEffect", transform.position, Quaternion.Euler(Vector3.right * -90));
        yield return null;

        gameObject.SetActive(false);
    }

    protected virtual void ResetTarget() { }
    public void SetTarget(CharBase target) => targetObject = target;

    private void RestrictionPos()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13.5f, 13.5f), Mathf.Clamp(transform.position.y, -3f, 3f));
    }
    private bool IsCharacterInEdge()
    {
        return Mathf.Abs(transform.position.x) > 13f || Mathf.Abs(transform.position.y) > 2.5f;
    }
}
