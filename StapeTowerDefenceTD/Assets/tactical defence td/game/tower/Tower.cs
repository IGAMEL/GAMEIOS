using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Tower : MonoBehaviour
{
    [SerializeField] private Collider2D detection_radius;
    protected GameObject enemy_aim = null;
    protected List<GameObject> all_enemies_in_range = new List<GameObject>();
    
    [SerializeField] [Min(0.1f)] protected float attack_speed;
    [SerializeField] [Min(1)] protected int attack_dmg;
    protected float attack_cooldown = 0.0f;

    [SerializeField] [Min(1)] protected float radius = 2;
    [SerializeField] protected GameObject range_rad;


    [Header("Other stuff")]
    [SerializeField] protected GameObject sprite_higher;
    [SerializeField] protected GameObject sprite;

    [SerializeField] protected GameObject shoot_effect1;
    [SerializeField] protected GameObject shoot_effect2;
    protected int attack_count = 0;

    [SerializeField] protected string SoundShoot;

    private void Awake()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = radius;
        range_rad.transform.localScale = Vector3.one*radius * 2;

        Sequence tween = DOTween.Sequence().SetId(gameObject);
        tween
            .Append(sprite_higher.transform.DOScaleY(2.0f, 0.0f))
            .Join(sprite_higher.transform.DOScaleX(0.5f, 0.0f))

            .Append(sprite_higher.transform.DOScaleY(1.0f, 1.0f).SetEase(Ease.OutElastic))
            .Join(sprite_higher.transform.DOScaleX(1.0f, 1.0f).SetEase(Ease.OutElastic));
    }

    private void Update()
    {
        if (enemy_aim != null)
        {
            Has_enemy_in_range();
        }
        CooldownTick();
        if (all_enemies_in_range.Count > 0 && enemy_aim == null) {
            enemy_aim = all_enemies_in_range[0];
            StartCoroutine(Redirect());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collided");
        if (collision.gameObject.tag == "Enemy")
        {
            all_enemies_in_range.Add(collision.gameObject);
            if (enemy_aim == null)
            {
                attack_cooldown = Mathf.Max(attack_cooldown, 0.125f);
                enemy_aim = collision.gameObject;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            all_enemies_in_range.Remove(collision.gameObject);
            if (enemy_aim == collision.gameObject)
            {
                StartCoroutine(Redirect());
            }
        }
    }

    IEnumerator Redirect()
    {
        print("redirect");
        
        GameObject new_enemy = null;
        float distance = 99;
        all_enemies_in_range.RemoveAll(enemy => enemy == null || !enemy.activeSelf);
        for (int i = 0; i< all_enemies_in_range.Count; i++)
        {
            if (all_enemies_in_range[i] != null)
            {
                float dst = (all_enemies_in_range[i].transform.position - gameObject.transform.position).magnitude;
                print(dst);

                if (distance > dst)
                {
                    distance = dst;
                    new_enemy = all_enemies_in_range[i];
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
        if (new_enemy == null && all_enemies_in_range.Count > 0)
        {
            StartCoroutine(Redirect());
            yield return true;
        }
        else
        {
            enemy_aim = new_enemy;
            attack_cooldown = Mathf.Max(attack_cooldown, 0.125f);
        }
        yield return true;
    }

    private void Has_enemy_in_range()
    {
        //sprite_higher.transform.rotation = Quaternion.LookRotation(enemy_aim.transform.position);

        Vector3 enemy_pos = enemy_aim.transform.position;
        enemy_pos.z = transform.position.z;
        Vector3 direction = enemy_pos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //direction.y = 0; // Игнорируем разницу по высоте
        sprite_higher.transform.rotation = Quaternion.Euler(0, 0, angle-90);


        if (attack_cooldown <= 0)
        {
            DoAttack();
        }
    }

    private void CooldownTick()
    {
        if (attack_cooldown > 0)
        {
            //print(attack_cooldown);
            attack_cooldown -= Time.deltaTime;
        }
    }

    private void DoAttack() //abstract attack
    {
        print("DoAttack");

        attack_cooldown = 1.0f / attack_speed;
        attack_count += 1;
        Attack();
        AttackAnim();

        AudioManager.Instance.PlaySound(SoundShoot);

    }

    virtual protected void Attack() //do_dmg
    {
        Enemy enemy_to_attack = enemy_aim.GetComponent<Enemy>();
        enemy_to_attack.receive_hit(attack_dmg);

    }

    virtual protected void AttackAnim() //animation of attack
    {
        DOTween.Kill(sprite, true);
        Sequence tween = DOTween.Sequence().SetId(sprite);
        tween
            .Append(sprite.transform.DOLocalMoveY(-0.125f, 0.0f))
            .AppendCallback(() =>
            {
                switch (attack_count % 2)
                {
                    case 0:
                        shoot_effect1.SetActive(true);
                        break;
                    case 1:
                        shoot_effect2.SetActive(true);
                        break;
                }
            }
            )
            .Append(sprite.transform.DOLocalMoveY(0.0f, 0.125f))
            //.AppendInterval(0.05f)
            .AppendCallback(() =>
             {
                  shoot_effect1.SetActive(false);
                  shoot_effect2.SetActive(false);
             });

    }

    private void OnDestroy()
    {
        DOTween.Kill(sprite);
        DOTween.Kill(gameObject);
    }
}
