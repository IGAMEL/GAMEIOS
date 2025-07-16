using UnityEngine;
using DG.Tweening;
public class undead : Enemy
{

    protected int amount_revive = 2;

    override protected void Die()
    {
        
        Collider2D.enabled = false;
        Sequence tween = DOTween.Sequence().SetAutoKill(true).SetId(gameObject);
        if (amount_revive > 0)
        {
            
            
            amount_revive -= 1;
            speed = 0.0f;
            tween
                .Append(transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutCubic))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    Collider2D.enabled = true;
                    receive_hit(-4);
                    start_speed += 0.25f;
                    speed = start_speed;
                })
                .Append(transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutElastic));
        }
        else {
            tween
                .Append(transform.DOScale(0.0f, 0.5f).SetEase(Ease.OutCubic))
                .AppendCallback(() => { Destroy(gameObject); });
        }

        //.AppendCallback(() => { Destroy(gameObject); });
        //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -8.0f), 0.25f).SetEase(Ease.InOutCubic));

    }
}
