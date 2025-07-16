using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;


public class FullGameManager : MonoBehaviour
{
    [SerializeField] private Enemy[] EnemyUnits;
    [SerializeField] private wave[] Waves;
    [SerializeField] private int wave = 0;

    [SerializeField] private GameObject enemies_container;

    [SerializeField] private way line;
    private Vector3[] points;



    [Header("Shop")]

    [SerializeField] private int money;
    
    [SerializeField] private Image shop_background;
    [SerializeField] private TMP_Text shop_money_amount;
    private tower_where_create selected_tower_where_create;

    [SerializeField] private Image sellTower;
    public float doDeleteSellTower = 0.1f;


    [SerializeField] private TMP_Text money_additional_counter;
    [SerializeField] private Image money_additional_counter_back;

    [Header("Waves")]
    [SerializeField] private TMP_Text wave_counter;
    private int max_waves;
    [SerializeField] private Button next_wave;
    private float between_waves = 0f;

    [Header("Lose win")]
    [SerializeField] private Canvas LoseCanv;
    [SerializeField] private Canvas WinCanv;

    [SerializeField] private int Level;
    //[SerializeField] private string lvl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = line.points;


        //print(EnemyUnits[0]);
        //CreateUnit(EnemyUnits[0]);

        sellTower.transform.localScale = Vector2.zero;
        max_waves = Waves.Length;

        wave_counter.text = wave + "/" + max_waves;
    }

    public void NextWave()
    {
        if (between_waves <= 0f)
        {
            Sequence tween = DOTween.Sequence().SetId(gameObject);
            tween
                .Append(next_wave.transform.DOScaleY(0.0f, 0.125f))
                .AppendCallback(() => { next_wave.gameObject.SetActive(false); });

            wave += 1;
            wave_counter.text = wave + "/" + max_waves;

            between_waves = 2.0f;

            generate_units();
        }
    }

    async void generate_units()
    {
        for (int units = 0; units < EnemyUnits.Length; units++)
        {
            for (int unit_amount = 0; unit_amount < Waves[wave - 1].EnemyUnitAmount[units]; unit_amount++)
            {
                await Task.Delay(100);
                CreateUnit(EnemyUnits[units]);
            }
        }
    }


    void CreateUnit(Enemy unit)
    {
        Enemy new_unit = Instantiate(unit, enemies_container.transform);

        new_unit.set_points(points);
        new_unit.transform.position = new Vector3(points[0].x+Random.Range(-1.0f,1.0f), points[0].y + Random.Range(-1.0f, 1.0f), 1);
        new_unit.FullGameManager = this;
    }

    private void Update()
    {
        between_waves -= Time.deltaTime;
        
        shop_money_amount.text = ""+ money;
        money_additional_counter.text = "" + money;

        /*        if (Input.GetMouseButtonUp(0) == true || (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)))
                {
                    tapped2();
                }*/

        if (Input.GetMouseButtonDown(0) == true || (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Began)))
        {
            tapped();
        }
        doDeleteSellTower -= Time.deltaTime;
        
        if (sellTower.gameObject.activeSelf)
        {
            sellTower.transform.position = selected_tower_where_create.transform.position + new Vector3(0, -1.0f, 0);
            sellTower.rectTransform.position = new Vector3(sellTower.rectTransform.position.x, sellTower.rectTransform.position.y, -1);
        }

        if (between_waves <= 0)
        {
            if (enemies_container.transform.childCount == 0 && next_wave.gameObject.activeSelf == false)
            {
                if (wave != max_waves)
                {
                    next_wave.transform.DOScaleY(1.0f, 0.125f).SetId(gameObject);
                    next_wave.gameObject.SetActive(true);
                }
                else
                {
                    Win();
                }
            }
        }
    }

    public void Add_money(int amount)
    {
        money += amount;
    }



    public void tapped()
    {
        if (sellTower.gameObject.activeSelf == true && doDeleteSellTower<=0)
        {
            Sequence tween = DOTween.Sequence().SetId(sellTower);
            tween
                .Append(sellTower.transform.DOScale(Vector2.zero, 0.125f))
                .AppendCallback(() =>
                {
                    sellTower.gameObject.SetActive(false);
                });
        }
    }
    public void tapped2()
    {
        if (sellTower.gameObject.activeSelf == true && doDeleteSellTower <= 0)
        {
            Sequence tween = DOTween.Sequence().SetId(sellTower);
            tween
                .Append(sellTower.transform.DOScale(Vector2.zero, 0.125f))
                .AppendCallback(() =>
                {
                    sellTower.gameObject.SetActive(false);
                });
        }
    }


    public void ChooseWhereTower(tower_where_create toChoose)
    {
        if (selected_tower_where_create != null)
        {
            selected_tower_where_create.transform.DOScale(1.0f, 0.25f).SetId(selected_tower_where_create).SetEase(Ease.OutElastic);
        }
        
        selected_tower_where_create = toChoose;
        ShowShop();

        selected_tower_where_create.transform.DOScale(1.25f, 0.25f).SetId(selected_tower_where_create).SetEase(Ease.OutElastic);
        //Camera.main.transform.DOLocalMoveX(Mathf.Clamp(selected_tower_where_create.transform.position.x,0,7), 0.25f).SetId(Camera.main);
        //Camera.main.transform.DOLocalMoveY(Mathf.Clamp(selected_tower_where_create.transform.position.y, -1, 1), 0.25f).SetId(Camera.main);
        //Camera.main.DOOrthoSize(3.75f, 0.5f).SetId(Camera.main).SetEase(Ease.OutElastic);
    }
    public void ChooseWhereDelete(tower_where_create toChoose)
    {
        if (shop_background.gameObject.activeSelf == true)
        {
            HideShop();
        }
        else
        {
            doDeleteSellTower = 0.05f;

            selected_tower_where_create = toChoose;
            selected_tower_where_create.transform.DOScale(1.0f, 0.25f).SetId(selected_tower_where_create).SetEase(Ease.OutElastic);
            sellTower.transform.position = toChoose.transform.position + new Vector3(0, -1.0f, 0);
            sellTower.rectTransform.position = new Vector3(sellTower.rectTransform.position.x, sellTower.rectTransform.position.y, -1);
            Sequence tween = DOTween.Sequence().SetId(sellTower);
            tween
                .AppendCallback(() => {
                    sellTower.gameObject.SetActive(true);
                })
                .Append(sellTower.transform.DOScale(Vector2.one, 0.125f));
        }
    }

    public void SellTower()
    {
        if (selected_tower_where_create != null)
        {
            selected_tower_where_create.sell();

            selected_tower_where_create.transform.DOScale(1.0f, 0.25f).SetId(selected_tower_where_create).SetEase(Ease.OutElastic);
            selected_tower_where_create = null;

            tapped();
        }
    }


    private void ShowShop()
    {
        DOTween.Kill(shop_background);
        Sequence tween = DOTween.Sequence().SetId(shop_background);
        tween
            .AppendCallback(()=> {
                shop_background.gameObject.SetActive(true);
            })
            .Append(shop_background.rectTransform.DOAnchorPosX(-381.41f, 0.125f))
            .Join(money_additional_counter_back.rectTransform.DOAnchorPosY(106f,0.125f));
    }

    public void HideShop()
    {
        if (selected_tower_where_create != null)
        {
            selected_tower_where_create.transform.DOScale(1.0f, 0.25f).SetId(selected_tower_where_create).SetEase(Ease.OutElastic);
        }

        DOTween.Kill(shop_background);
        selected_tower_where_create = null;
        Sequence tween = DOTween.Sequence().SetId(shop_background);
        tween
            .Append(shop_background.rectTransform.DOAnchorPosX(390.7661f, 0.125f))
            .Join(money_additional_counter_back.rectTransform.DOAnchorPosY(-106f, 0.125f))
            .AppendCallback(() =>
            {
                shop_background.gameObject.SetActive(false);
            });

        //Camera.main.transform.DOLocalMoveX(0, 0.125f).SetId(Camera.main);
        //Camera.main.transform.DOLocalMoveY(0, 0.125f).SetId(Camera.main);
        //Camera.main.DOOrthoSize(5.0f, 0.125f).SetId(Camera.main);
    }

    public void buy_tower(int type)
    {
        int cost = 0;
        switch (type)
        {
            case 0: cost = 25; break;
            case 1: cost = 200; break;
            case 2: cost = 50; break;
            case 3: cost = 500; break;
            case 4: cost = 100; break;
            case 5: cost = 1000; break;
            case 6: cost = 200; break;
        }
        
        
        if (money >= cost)
        {
            selected_tower_where_create.transform.DOScale(1.0f, 0.25f).SetId(selected_tower_where_create).SetEase(Ease.OutElastic);
            

            selected_tower_where_create.choosenTower(type);
            money -= cost;
            HideShop();
        }
    }

    public void Lose()
    {
        if (WinCanv.gameObject.activeSelf == false && LoseCanv.gameObject.activeSelf == false)
        {
            LoseCanv.gameObject.SetActive(true);

            CanvasGroup canvagro = LoseCanv.gameObject.GetComponent<CanvasGroup>();

            canvagro.alpha = 0;

            Sequence tween = DOTween.Sequence().SetId(LoseCanv);
            tween
                .Append(canvagro.DOFade(1.0f, 0.125f));
        }
    }

    public void Win()
    {
        if (LoseCanv.gameObject.activeSelf == false && WinCanv.gameObject.activeSelf == false)
        {
            WinCanv.gameObject.SetActive(true);
            CanvasGroup canvagro = WinCanv.gameObject.GetComponent<CanvasGroup>();

            canvagro.alpha = 0;

            Sequence tween = DOTween.Sequence().SetId(WinCanv);
            tween
                .Append(canvagro.DOFade(1.0f, 0.125f));


            int level_now = PlayerPrefs.GetInt("levels", 0);
            PlayerPrefs.SetInt("levels", Mathf.Max(Level, level_now));
        }
    }
}
