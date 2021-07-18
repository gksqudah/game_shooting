using UnityEngine;
using System.Collections;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Die
}

public class Enemy : MonoBehaviour {

    public EnemyState ES;
    public Animator anim;
    float Speed;

    public float MoveSpeed;
    public float AttackSpeed;

    public float FindRange = 10f;
    public float Damage = 20f;
    public Transform Player;

    public Transform FX_Point;
    public GameObject Hit_FX;
    public AudioClip Hit_Sound;
    public AudioClip Death_Sound;

    public GameObject GUI_Pivot;
    public UISlider LifeBar;
    public float Max_hp = 100f;
    public float hp = 100f;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
        anim = this.GetComponent<Animator>();
    }

    void DistanceCheck()
    {
        if (Vector3.Distance(Player.position, transform.position) >= FindRange)
        {
            ES = EnemyState.Idle;
            anim.SetBool("Run", false);
            Speed = 0;
        }
        else
        {
            ES = EnemyState.Move;
            anim.SetBool("Run", true);
            Speed = MoveSpeed;
        }
    }

    void MoveUpdate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(Player.position.x, this.transform.position.y,Player.position.z) - transform.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    void Update()
    {
        if (ES == EnemyState.Idle)
        {
            DistanceCheck();
        }
        else if(ES == EnemyState.Move)
        {
            MoveUpdate();
            AttackRangeCheck();
        }
    }

    void AttackRangeCheck()
    {
        if (Vector3.Distance(Player.position, transform.position) < 1.5f && ES != EnemyState.Attack)
        {
            Speed = 0;
            ES = EnemyState.Attack;
            anim.SetTrigger("Attack");
        }
    }

    public void Attack_On()
    {
        Player.GetComponent<Player_Ctrl>().Hurt(Damage);
    }

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            ES = EnemyState.Hurt;
            Speed = 0;
            anim.SetTrigger("Hurt");

            //적이 맞았을 때 이펙트를 발생시키는 구문.
            GameObject FX = Instantiate(Hit_FX, FX_Point.position, Quaternion.LookRotation(FX_Point.forward)) as GameObject;

            //hp를 깎고 UI를 변경하는 부분.
            hp -= damage;
            LifeBar.sliderValue = hp / Max_hp;

            //맞았을 때 사운드를 발생시키는 부분.
            GetComponent<AudioSource>().clip = Hit_Sound;
            GetComponent<AudioSource>().Play();

            //hp가 0보다 작은 경우 Death 함수 실행.
            if (hp <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        ES = EnemyState.Die;   
        Speed = 0;
        anim.SetTrigger("Die");

        //GUI 끄고 죽는 사운드 플레이하기.
        GUI_Pivot.gameObject.SetActive(false);
        GetComponent<AudioSource>().clip = Death_Sound;
        GetComponent<AudioSource>().Play();

        //PlayManager를 찾아서 PM이라는 이름으로 가져와 함수 실행 시키기.
        PlayManager PM = GameObject.Find("PlayManager").GetComponent<PlayManager>();
        PM.EnemyDie();
        
    }
}
