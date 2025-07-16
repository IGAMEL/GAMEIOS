using UnityEngine;
using DG.Tweening;
public class sniper_dounle : sniper
{
    override protected void AttackAnim() //animation of attack
    {
        DOTween.Kill(sprite, true);
        Sequence tween = DOTween.Sequence().SetId(sprite);
        tween
            .Append(sprite.transform.DOLocalMoveY(-0.12f, 0.0f))
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
                shot.SetPosition(1, enemy_aim.transform.position);
                shot.SetPosition(0, transform.position);
                shot.enabled = true;
            }
            )
            .AppendInterval(0.05f)
            .AppendCallback(() =>
            {

                switch (attack_count % 2)
                {
                    case 0:
                        shoot_effect1.SetActive(false);
                        break;
                    case 1:
                        shoot_effect2.SetActive(false);
                        break;
                }
                shot.enabled = false;
            }

            )
            .Append(sprite.transform.DOLocalMoveY(0.2f, 1.25f).SetEase(Ease.InOutElastic));

    }
}
