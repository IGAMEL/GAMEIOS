using UnityEngine;
using DG.Tweening;

public class sniper : Tower
{
    [SerializeField] protected LineRenderer shot;
    override protected void AttackAnim() //animation of attack
    {
        DOTween.Kill(sprite, true);
        Sequence tween = DOTween.Sequence().SetId(sprite);
        tween
            .Append(sprite.transform.DOLocalMoveY(-0.125f, 0.0f))
            .AppendCallback(() =>
            {
                shoot_effect1.SetActive(true);
                shot.SetPosition(1, enemy_aim.transform.position);
                shot.SetPosition(0, transform.position);
                shot.enabled = true;
            }
            )
            .AppendInterval(0.05f)

            .AppendCallback(() =>
            {
                shot.enabled = false;
                shoot_effect1.SetActive(false);
            }

            )
            .Append(sprite.transform.DOLocalMoveY(0.2f, 2.0f).SetEase(Ease.InOutElastic));

    }
}

