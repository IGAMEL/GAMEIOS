using UnityEngine;
using DG.Tweening;
using TMPro;
public class tank : Enemy
{
    public TMP_Text blocked_txt;


    [SerializeField] TrailRenderer line1;
    [SerializeField] TrailRenderer line2;

    override public void receive_hit(int amount_of_dmg)
    {
        receive_dmg(Mathf.Max(amount_of_dmg-1,0));

        print("tank");
        print(Mathf.Max(amount_of_dmg - 1, 0));

        if (Mathf.Max(amount_of_dmg - 1, 0) == 0)
        {
            AudioManager.Instance.PlaySound("tank_blocked");

            DOTween.Kill(blocked_txt.gameObject, true);
            blocked_txt.enabled = true;
            blocked_txt.transform.localEulerAngles.Set(0, 0, Random.value * 4);
            Sequence tween = DOTween.Sequence().SetId(blocked_txt.gameObject);
            tween
                .Append(blocked_txt.gameObject.transform.DOScale(0.75f, 0))
                .AppendCallback(() => { blocked_txt.gameObject.SetActive(true); })
                .Append(blocked_txt.gameObject.transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutElastic))
                .Append(blocked_txt.gameObject.transform.DOScale(0.0f, 0.25f).SetEase(Ease.InCubic))
                .AppendCallback(() => { blocked_txt.gameObject.SetActive(false); });
        }
    }

    override protected void Animation()
    {
        sprite.transform.DOLocalRotate(new Vector3(0, 0, 8.0f), 0);
        Sequence tween = DOTween.Sequence().SetAutoKill(true).SetId(gameObject).SetLoops(-1, LoopType.Yoyo);
        tween
            //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -4.0f), 0.0125f).SetEase(Ease.InOutCubic))
            .Append(sprite.transform.DOLocalMove(new Vector3(0, 0.025f, 0), 0.125f).SetEase(Ease.InOutCubic))

            //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, 2.0f), 0.0125f).SetEase(Ease.InOutCubic))
            .Append(sprite.transform.DOLocalMove(new Vector3(0, -0.025f, 0), 0.125f).SetEase(Ease.InOutCubic))

            //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, 4.0f), 0.0125f).SetEase(Ease.InOutCubic))
            .Append(sprite.transform.DOLocalMove(new Vector3(0, 0.025f, 0), 0.125f).SetEase(Ease.InOutCubic))

            //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -2.0f), 0.0125f).SetEase(Ease.InOutCubic))
            .Append(sprite.transform.DOLocalMove(new Vector3(0, -0.025f, 0), 0.125f).SetEase(Ease.InOutCubic));
    }

    override protected void Die()
    {
        line1.emitting = false;
        line2.emitting = false;

        Collider2D.enabled = false;
        Sequence tween = DOTween.Sequence().SetAutoKill(true).SetId(gameObject);
        tween
            .Append(transform.DOScale(0.0f, 0.5f).SetEase(Ease.OutCubic))
            .AppendCallback(() => { Destroy(gameObject); });
        //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -8.0f), 0.25f).SetEase(Ease.InOutCubic));

    }

}
