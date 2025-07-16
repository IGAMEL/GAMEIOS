using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MENU_MANAGER : MonoBehaviour
{
    [SerializeField] Image menu;
    [SerializeField] Image settings;
    [SerializeField] Image level_select;

    [SerializeField] Image page1;
    [SerializeField] Image page2;

    [SerializeField] Camera Camera;


    [SerializeField] TMP_Text title;

    public int MaxLevelPlayerGot = 0;
    public Texture2D Locked_lvl;
    public Texture2D Locked_Next_Page;

    [SerializeField] Scrollbar music;
    [SerializeField] Scrollbar sfx;
    private void Awake()
    {
        MaxLevelPlayerGot = PlayerPrefs.GetInt("levels", 0);
        //PlayerPrefs.SetInt("levels", 10);

        Sequence tween = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo).SetId(Camera);
        tween
            .Append(Camera.transform.DOLocalRotate(new Vector3(0, 0, 0.5f), 5f).SetEase(Ease.InOutCubic))
            .Append(Camera.transform.DOLocalRotate(new Vector3(0, 0, -0.5f), 5f).SetEase(Ease.InOutCubic));

        Sequence tween_title = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo).SetId(Camera);
        tween_title
            .Append(title.transform.DOLocalRotate(new Vector3(0, 0, -1f), 1f).SetEase(Ease.InOutCubic))
            .Append(title.transform.DOLocalRotate(new Vector3(0, 0, 1f), 1f).SetEase(Ease.InOutCubic));


        AudioManager.Instance.musicSource.volume = PlayerPrefs.GetFloat("music_volume", 0.5f);
        AudioManager.Instance.source.volume = PlayerPrefs.GetFloat("sfx_volume", 0.5f);

        music.value = AudioManager.Instance.musicSource.volume;
        sfx.value = AudioManager.Instance.source.volume;
    }

    public void toMainMenu()
    {
        //menu.rectTransform.DOLocalMoveY(1055f, 0.5f).SetId(menu);
        menu.rectTransform.DOLocalMoveY(0f, 0.5f).SetId(menu).SetEase(Ease.OutQuart);
        Camera.transform.DOLocalMoveX(0f, 1.0f).SetId(Camera).SetEase(Ease.OutQuart);

        settings.rectTransform.DOLocalMoveX(-2209f, 0.5f).SetId(settings).SetEase(Ease.OutQuart);

        level_select.rectTransform.DOLocalMoveX(2209, 0.5f).SetId(level_select).SetEase(Ease.OutQuart);

        page1.rectTransform.DOLocalMoveX(0, 0.5f).SetId(page1).SetEase(Ease.OutQuart);
        page2.rectTransform.DOLocalMoveX(2209f, 0.25f).SetId(page2).SetEase(Ease.OutQuart);
    }

    public void toSettings()
    {
        menu.rectTransform.DOLocalMoveY(1055f, 0.5f).SetId(menu).SetEase(Ease.OutQuart);
        Camera.transform.DOLocalMoveX(-23f, 1.0f).SetId(Camera).SetEase(Ease.OutQuart);

        //settings.rectTransform.DOLocalMoveX(-2209f, 0.25f).SetId(settings).SetEase(Ease.OutQuart);
        settings.rectTransform.DOLocalMoveX(0f, 0.5f).SetId(settings).SetEase(Ease.OutQuart);

        level_select.rectTransform.DOLocalMoveX(2209, 0.5f).SetId(level_select).SetEase(Ease.OutQuart);

        page1.rectTransform.DOLocalMoveX(0, 0.5f).SetId(page1).SetEase(Ease.OutQuart);
        page2.rectTransform.DOLocalMoveX(2209f, 0.25f).SetId(page2).SetEase(Ease.OutQuart);
    }

    public void toChooseLevel()
    {
        menu.rectTransform.DOLocalMoveY(1055f, 0.5f).SetId(menu).SetEase(Ease.OutQuart);
        Camera.transform.DOLocalMoveX(22f, 1.0f).SetId(Camera).SetEase(Ease.OutQuart);

        settings.rectTransform.DOLocalMoveX(-2209f, 0.5f).SetId(settings).SetEase(Ease.OutQuart);

        //level_select.rectTransform.DOLocalMoveX(2209, 0.5f).SetId(level_select).SetEase(Ease.OutQuart);
        level_select.rectTransform.DOLocalMoveX(0, 0.5f).SetId(level_select).SetEase(Ease.OutQuart);
        toPage1();
    }

    public void toPage2()
    {
        page1.rectTransform.DOLocalMoveX(-2209f, 0.25f).SetId(page1).SetEase(Ease.OutQuart);
        page2.rectTransform.DOLocalMoveX(0f, 0.5f).SetId(page2).SetEase(Ease.OutQuart);

        Camera.transform.DOLocalMoveX(23f, 1.0f).SetId(Camera).SetEase(Ease.OutQuart);
    }

    public void toPage1()
    {
        page1.rectTransform.DOLocalMoveX(0, 0.5f).SetId(page1).SetEase(Ease.OutQuart);
        page2.rectTransform.DOLocalMoveX(2209f, 0.25f).SetId(page2).SetEase(Ease.OutQuart);

        Camera.transform.DOLocalMoveX(22f, 1.0f).SetId(Camera).SetEase(Ease.OutQuart);
    }



    public void ChangeVolumeMusic()
    {
        AudioManager.Instance.musicSource.volume = music.value;
        PlayerPrefs.SetFloat("music_volume", music.value);
    }
    public void ChangeVolumeSfx()
    {
        AudioManager.Instance.source.volume = sfx.value;
        PlayerPrefs.SetFloat("sfx_volume", sfx.value);
    }
}
