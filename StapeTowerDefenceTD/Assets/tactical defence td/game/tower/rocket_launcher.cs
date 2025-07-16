using UnityEngine;
using DG.Tweening;

public class rocket_launcher : Tower
{

    [SerializeField] explosion_from_rocket Rocket;
    [SerializeField] float explo_radius = 1f;

    [SerializeField] int multishot_count = 2;

    override protected void AttackAnim() //animation of attack
    {
        DOTween.Kill(sprite, true);
        Sequence tween = DOTween.Sequence().SetId(sprite);
        tween
            .Append(sprite.transform.DOLocalMoveY(-0.125f, 0.0f))
            .Append(sprite.transform.DOLocalMoveY(0.0f, 0.125f));

    }

    override protected void Attack() //do_dmg
    {
        Enemy enemy_to_attack = enemy_aim.GetComponent<Enemy>();
        //enemy_to_attack.receive_hit(attack_dmg);

        Instantiate(Rocket, transform, true);
        Rocket.transform.position = enemy_aim.transform.position + new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f, 0.5f), 0);
        Rocket.transform.position = new Vector3(Rocket.transform.position.x, Rocket.transform.position.y, 0);
        Rocket.dmg = attack_dmg;
        Rocket.radius = explo_radius;

        if (attack_count%multishot_count != 0)
        {
            attack_cooldown = 1.0f / (attack_speed*4);
            print(attack_cooldown);
        }
    }
}
