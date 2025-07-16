using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class transit : MonoBehaviour
{
    [SerializeField] GameObject panel;
    private bool did = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sequence tween = DOTween.Sequence().SetId(gameObject);
        tween
            .Append(Camera.main.transform.DOMoveX(-1.0f, 0.0f))
            .Append(Camera.main.transform.DOMoveX(0.0f, 0.5f))
            .Join(panel.transform.DOScaleX(0.0f, 0.5f))


            ;

    }

    public void MoveTo(string scene_to)
    {
        if (did == false)
        {
            Sequence tween = DOTween.Sequence().SetId(gameObject);
            tween
                .Append(Camera.main.transform.DOMoveX(1.0f, 0.5f).SetEase(Ease.InCubic))
                .Join(panel.transform.DOScaleX(1.1f, 0.5f).SetEase(Ease.InCubic))
                .AppendCallback(() =>
                {
                    SceneManager.LoadScene(scene_to);
                });

            ;
            did = true;
        }
    }

    public void Retry()
    {
        if (did == false)
        {
            Sequence tween = DOTween.Sequence().SetId(gameObject);
            tween
                .Append(Camera.main.transform.DOMoveX(1.0f, 0.5f).SetEase(Ease.InCubic))
                .Join(panel.transform.DOScaleX(1.1f, 0.5f).SetEase(Ease.InCubic))
                .AppendCallback(() =>
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });

            ;
            did = true;
        }


    }

    public void Quit()
    {
        if (did == false)
        {
            Sequence tween = DOTween.Sequence().SetId(gameObject);
            tween
                .Append(Camera.main.transform.DOMoveX(-1.0f, 0.5f).SetEase(Ease.InCubic))
                .Join(panel.transform.DOScaleX(1.1f, 0.5f).SetEase(Ease.InCubic))
                .AppendCallback(() =>
                {
                    Application.Quit();
                });

            ;
            did = true;
        }
    }
}
