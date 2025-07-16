using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class Enemy : MonoBehaviour
{
    [SerializeField] [Min(0)] protected int health = 0;

    [SerializeField] [Min(0)] protected float speed = 0;
    protected float start_speed = 0;

    //[SerializeField] private ;
    [SerializeField] protected GameObject sprite;
    [SerializeField] protected GameObject sprite_higher;


    public Vector3[] way;
    public int current_id_of_point_of_way = -1;
    private float dir_to_point;
    protected Vector3 current_point;

    [SerializeField] private Healthbar Healthbar;
    [SerializeField] protected Collider2D Collider2D;

    [SerializeField] protected int money_gives = 10;
    public FullGameManager FullGameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start_speed = speed;

        Animation();
        GotToPoint();
        Healthbar.Setup_value(health, health);
        Healthbar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        WalkTowardsPoint();
        if (speed < start_speed)
        {
            speed = Mathf.Clamp(speed + Time.deltaTime * 0.1f, 0.25f, start_speed);
        }

    }

    private void WalkTowardsPoint()
    {
        if ((transform.localPosition - current_point).magnitude <= 0.5)
        {
            GotToPoint();
        }


        transform.localPosition = Vector3.MoveTowards(transform.localPosition, current_point,speed*Time.deltaTime);
    }

    public void set_points(Vector3[] points)
    {
        way = points;
    }


    private void GotToPoint()
    {
        current_id_of_point_of_way += 1;
        
        if (current_id_of_point_of_way > way.Length - 1)
        {
            FullGameManager.Lose();
        }
        else
        {
            current_point = way[current_id_of_point_of_way];

            current_point += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            current_point.z = transform.position.z;



            dir_to_point = Mathf.Atan2(current_point.y - transform.position.y, current_point.x - transform.position.x) * Mathf.Rad2Deg;

            Tween tween = sprite_higher.transform.DORotate(new Vector3(0, 0, dir_to_point), 0.5f).SetEase(Ease.OutElastic).SetAutoKill(true).SetId(gameObject);
        }
    }
    protected virtual void Animation()
    {
        sprite.transform.DOLocalRotate(new Vector3(0, 0, 8.0f), 0);
        Sequence tween = DOTween.Sequence().SetAutoKill(true).SetId(gameObject).SetLoops(-1,LoopType.Yoyo);
        tween
            .Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -8.0f), 0.25f).SetEase(Ease.InOutCubic));
    }

    public virtual void receive_hit(int amount_of_dmg)
    {
        receive_dmg(amount_of_dmg);
    }
    public virtual void receive_air_hit(int amount_of_dmg)
    {
        receive_dmg(amount_of_dmg);
    }

    protected void receive_dmg(int amount)
    {
        
        Healthbar.gameObject.SetActive(true);
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }


        Healthbar.Set_value(health);
    }

    public void slow_down(float amount)
    {
        speed = Mathf.Clamp(speed-amount,0.25f,speed);
    }

    virtual protected void Die()
    {
        Collider2D.enabled = false;
        speed = 0.25f;
        Sequence tween = DOTween.Sequence().SetAutoKill(true).SetId(gameObject);
        tween
            .Append(transform.DOScale(0.0f, 0.5f).SetEase(Ease.OutCubic))
            .AppendCallback(() => { Destroy(gameObject); });
            //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -8.0f), 0.25f).SetEase(Ease.InOutCubic));
        
    }

    protected void OnDestroy()
    {
        FullGameManager.Add_money(money_gives);
        DOTween.Kill(gameObject);
    }
}
