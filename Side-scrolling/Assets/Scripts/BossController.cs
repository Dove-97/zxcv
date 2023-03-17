using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    const int STATE_WALK = 0;
    const int STATE_ATTACK = 1;
    const int STATE_SKILLATTACK = 2;

    private GameObject Target;

    private Animator Anim;

    private SpriteRenderer renderer;

    private Vector3 Movement;
    private Vector3 EndPoint;

    private float CoolDown;
    private float Speed;
    private int HP;

    private bool SkillAttack;
    private bool Attack;
    private bool Walk;

    private bool Active;

    private int choice;

    private void Awake()
    {
        Target = GameObject.Find("Player");

        Anim = GetComponent<Animator>();

        renderer = GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        choice = 0;

        CoolDown = 1.5f;
        Speed = 0.5f;
        HP = 3000;

        SkillAttack = false;
        Attack = false;
        Walk = false;

        Active = true;

        StartCoroutine(OnCooldown());
    }

    void Update()
    {
        float result = Target.transform.position.x - transform.position.x;

        if (result < 0.0f)
            renderer.flipX = true;
        else if (result < 0.0f)
            renderer.flipX = false;

        if (ControllerManager.GetInstance().DirRight)
            transform.position += new Vector3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

        if(Active)
        {
            Active = false;
            StartCoroutine(OnCooldown());

        }

    }

    private int onController()
    {
        // ** 행동 패턴에 대한 내용을 추가
        {
            // ** 초기화
            if(Walk)
            {
                Movement = new Vector3(0.0f, 0.0f, 0.0f);
                Anim.SetFloat("Speed", Movement.x);
                Walk = false;
            }

            if(SkillAttack)
            {
            SkillAttack = false;

            }

            if(Attack)
            {
            Attack = false;
            }

        }

        // ** 로직




        //** 어디로 움직일지 정하는 시점에 플레이어의 윞치를 도착지점으로 셋팅
        EndPoint = Target.transform.position;


        // * [return]
        // * 0 : 공격     Attack
        // * 1 : 이동     Walk
        // * 2 : 슬라이딩 SkillAttack
        return Random.Range(0, 3);
    }




    private IEnumerator OnCooldown()
    {
        if(Active)
        {
            Active = false;
            choice = onController();
        }

        float fTime = CoolDown;

        while (fTime > 0.0f)
        {
            fTime -= Time.deltaTime;
            yield return null;
        }

        switch (choice)
        {
            case 0:
                OnAttack();
                break;

            case 1:
                OnWalk();
                break;

            case 2:
                OnSlide();
                break;
        }
    }

    private void OnAttack()
    {
        {
            print("OnAttack");
        }
        Active = true;

    }

    private void OnWalk()
    {
        Walk = true;
        Movement = new Vector3(Speed, 0.0f, 0.0f);

        transform.position -= Movement * Time.deltaTime;
        Anim.SetFloat("Speed", Movement.x);

        // ** 목적지에 도착할때까지

        float Distance = Vector3.Distance(EndPoint, transform.position);

        if(Distance > 0.5f)
        {
            Vector3 Direction = (EndPoint - transform.position).normalized;

            Movement = new Vector3(Speed * Direction.x, Speed * Direction.y, 0.0f);

            transform.position -= Movement * Time.deltaTime;
            Anim.SetFloat("Speed", Movement.x);
        }

        Active = true;

    }

    private void OnSlide()
    {
        {
            print("OnSlide");

        }
        Active = true;

    }

}
