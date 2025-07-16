using UnityEngine;
using DG.Tweening;
public class slower : Tower
{
    override protected void AttackAnim() //animation of attack
    {
        DOTween.Kill(sprite, true);
        Sequence tween = DOTween.Sequence().SetId(sprite);
        tween
            .Append(sprite.transform.DOLocalMoveY(-0.125f, 0.0f))
            .AppendCallback(() =>
            {
                shoot_effect1.SetActive(true);

            }
            )
            .Append(sprite.transform.DOLocalMoveY(0.0f, 0.125f))
            //.AppendInterval(0.05f)
            .AppendCallback(() =>
            {
                shoot_effect1.SetActive(false);

            });

    }

    override protected void Attack() //do_dmg
    {
        Enemy enemy_to_attack = enemy_aim.GetComponent<Enemy>();
        enemy_to_attack.receive_hit(attack_dmg);
        enemy_to_attack.slow_down(0.5f);
    }
}
