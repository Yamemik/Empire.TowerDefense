using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    Transform exit;//точка выхода
    [SerializeField]
    Transform[] wayPoints;//все точки к которым надо прикасаться
    [SerializeField]
    float navigation;//расчет перемещения
    [SerializeField]
    int health;
    [SerializeField]
    int rewardAmount;

    Transform enemy;//положение противника
    float navigationTime = 0;
    int target = 0;//к какой цели подошёл противник
    bool isDead = false;
    Collider2D enemyCollider;
    Animator anim;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Manager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints != null && isDead == false){
            navigationTime += Time.deltaTime;
            if (navigationTime > navigation){
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "MoviengPoint"){
            target += 1;
        }
        else if(collision.tag == "Finish"){
            Manager.Instance.RoundEscaped += 1;
            Manager.Instance.TotalEscaped += 1;
            Manager.Instance.UnregisterEnemy(this);
            Manager.Instance.IsWaveOver();
        }
        else if (collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackDamage);
            Destroy(collision.gameObject);
        }
    }

    public void EnemyHit(int hitPoints)
    {
        if (health-hitPoints>0)
        {
            //hurt/урон
            health -= hitPoints;
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            anim.Play("Hurt");
        }
        else
        {
            //die
            anim.SetTrigger("didDie");
            Die();
        }

    }
    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        Manager.Instance.TotalKilled += 1;
        Manager.Instance.AddMoney(rewardAmount);
        Manager.Instance.IsWaveOver();
        Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
    }
}
