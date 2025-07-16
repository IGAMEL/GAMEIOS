using UnityEngine;
using DG.Tweening;
using TMPro;

public class plane : Enemy
{
    public TMP_Text blocked_txt;
    [SerializeField] float chance = 0.5f;

    [SerializeField] TrailRenderer line1;

    override public void receive_hit(int amount_of_dmg)
    {
        float fat_chance = Random.Range(0.00f, 1.00f);
        if (fat_chance < chance)
        {
            receive_dmg(Mathf.Max(amount_of_dmg, 0));
        }
        else
        {
            AudioManager.Instance.PlaySound("plane_evaded");

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
        
    }

    override protected void Die()
    {
        line1.emitting = false;

        Collider2D.enabled = false;
        Sequence tween = DOTween.Sequence().SetAutoKill(true).SetId(gameObject);
        tween
            .Append(transform.DOScale(0.0f, 0.5f).SetEase(Ease.OutCubic))
            .AppendCallback(() => { Destroy(gameObject); });
        //.Append(sprite.transform.DOLocalRotate(new Vector3(0, 0, -8.0f), 0.25f).SetEase(Ease.InOutCubic));

    }
}
