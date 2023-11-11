using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharBase : MonoBehaviour, IHittable
{
    [HideInInspector] public CharBase targetObject = null;

    public CharacterSO charSO;
    [Header("공격 범위")]
    [SerializeField] protected float attackDistance; //공격 범위
    [Header("도망 범위")]
    [SerializeField] protected float escapeDistance; //도망 범위

    #region boolean
    [HideInInspector] public bool isPlayer = false;
    [HideInInspector] public bool isAct = false;
    [HideInInspector] public bool isStun = false;

    [HideInInspector] public bool IsDead { get; protected set; } = false;
    #endregion
    #region mvp
    [HideInInspector] public float mvpStack = 0;
    [HideInInspector] public static float totalMvpStack = 0;
    #endregion
    #region animation
    [Space]
    [Header("애니메이션 클립")]
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip moveClip;
    [SerializeField] private AnimationClip attackClip;
    protected Animator anim;
    protected AnimatorOverrideController overrideController;

    protected readonly int _attack = Animator.StringToHash("Attack");
    protected readonly int _move = Animator.StringToHash("Move");
    #endregion
    #region stat
    [HideInInspector] public float maxHp;
    [HideInInspector] public float hp;
    [HideInInspector] public float atk;
    [HideInInspector] public float def;
    [HideInInspector] public float spd;
    [HideInInspector] public float crit;
    #endregion
    #region flip
    private bool initFlip;
    protected bool isFlip = false;
    #endregion

    protected Rigidbody2D rigid;
    protected SpriteRenderer sprite;
    protected HPBar hpBar;

    private void Awake()
    {
        Init();
        StartCoroutine(SetAct());
    }

    private void Update()
    {
        RestrictionPos();
        SetFlip();
    }

    #region Init
    protected virtual void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        hpBar = GetComponentInChildren<HPBar>();

        initFlip = sprite.flipX;

        SetTeam();
        SetStatus();
        SetAnimation();

        TeamManager.Instance.OnDieEvent.RemoveListener(ResetTarget);
        TeamManager.Instance.OnDieEvent.AddListener(ResetTarget);
    }
    private void SetTeam()
    {
        if (isPlayer)
            TeamManager.Instance.playerTeamList.Add(GetComponent<PlayerCharBase>());
        else
            TeamManager.Instance.enemyTeamList.Add(GetComponent<EnemyCharBase>());
    }
    private void SetStatus()
    {
        maxHp = charSO.Hp;
        hp = maxHp;
        atk = charSO.Atk;
        def = charSO.Def;
        spd = charSO.Spd;
        crit = charSO.Crit;
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
    #endregion
    #region Act
    protected virtual IEnumerator SetAct()
    {
        while (!IsDead)
        {
            yield return null;
            rigid.velocity = Vector2.zero;

            if (isAct || isStun) continue;
            if (targetObject == null) continue;

            float targetDist = Vector2.Distance(targetObject.transform.position, transform.position);

            switch (targetDist)
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

    protected virtual void Attack(){}
    protected virtual void Chase() 
    {
        anim.SetBool(_move, true);
        Vector2 dir = (targetObject.transform.position - transform.position).normalized;
        isFlip = dir.x > 0;
        rigid.velocity = dir * spd;
    }
    protected virtual void Escape() 
    {
        if(IsCharacterInEdge())
        {
            Attack();
            return;
        }
        anim.SetBool(_move, true);
        Vector2 dir = (transform.position - targetObject.transform.position).normalized;
        isFlip = dir.x < 0;
        rigid.velocity = dir * spd;
    }

    protected virtual IEnumerator OnDie()
    {
        CinemachineCameraShaking.Instance.CameraShake(5, 0.1f);
        Managers.Pool.PoolManaging("DeathEffect", transform.position, Quaternion.Euler(Vector3.right * -90));
        yield return null;

        gameObject.SetActive(false);
    }
    #endregion
    #region Target
    public virtual void ResetTarget(CharBase origin = null) { }
    public void SetTarget(CharBase target) => targetObject = target;
    #endregion
    #region Restriction
    private void RestrictionPos()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9f, 9f), Mathf.Clamp(transform.position.y, -3f, 3f));
    }
    private bool IsCharacterInEdge()
    {
        return Mathf.Abs(transform.position.x) > 8.5f || Mathf.Abs(transform.position.y) > 2.5f;
    }
    #endregion

    public virtual void OnDamage(float damage, float critChance = 0, CharBase from = null, string hitEffect = null)
    {
        if (IsDead) return;

        float critRate = Random.Range(0f, 100f);
        bool isCrit = critChance > critRate;

        damage *= isCrit ? 1.5f : 1f;

        float defPer = def / (def + 1);
        damage *= 1 - defPer; 

        hp -= damage;
        ShowDmgPopup.Instance.ShowDmg(damage, gameObject, isCrit);
        if (hitEffect == null)
            hitEffect = "HitEffect";
        Managers.Pool.PoolManaging(hitEffect, transform.position, Quaternion.AngleAxis(-90,Vector3.right));

        if (from != null && from.isPlayer)
        {
            from.mvpStack += damage;
            totalMvpStack += damage;

            TeamManager.Instance.OnHitEvent.Invoke();
        }

        hpBar.UpdateHpBar();

        if (hp <= 0)
        {
            IsDead = true;
            TeamManager.Instance.OnDieEvent.Invoke(this);
            if (isPlayer)
                GetComponent<PlayerCharBase>().playerDieEvents.Invoke();
            StartCoroutine(OnDie());
        }
    }
    public virtual void OnHeal(float value, CharBase from = null)
    {
        if (IsDead) return;

        hp += value;
        if (hp > maxHp)
            hp = maxHp;

        ShowDmgPopup.Instance.ShowDmg(-value, gameObject);
        if (from != null && from.isPlayer)
        {
            from.mvpStack += value;
            totalMvpStack += value;
        }

        hpBar.UpdateHpBar();
    }
    private void SetFlip() => sprite.flipX = isFlip != initFlip;
}
