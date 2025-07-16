using UnityEngine;
using DG.Tweening;
public class explosion_from_rocket : MonoBehaviour
{
    public int dmg = 1;
    public float radius = 0.5f;

    [SerializeField] GameObject explo;
    [SerializeField] GameObject sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = radius*Vector3.one*2;
        explo.SetActive(true);
        Sequence tween = DOTween.Sequence().SetId(gameObject);
        tween
            .AppendInterval(0.05f)
            .AppendCallback(() =>
            {
                explo.SetActive(false);
                gameObject.GetComponent<Collider2D>().enabled = false;
            })
            .Append(transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InCubic))
            .AppendCallback(()=>
            {
                Destroy(gameObject);
            });
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.receive_air_hit(dmg);
        }
    }
}
