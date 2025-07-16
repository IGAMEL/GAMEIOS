using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LOADING_MANAGER : MonoBehaviour
{
    [SerializeField] Image plane;
    private float tick = 0;

    private float start_time = 1.0f;

    // Update is called once per frame

    private void Awake()
    {
        start_time = Random.Range(1.0f, 3.0f);

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        tick += Time.deltaTime;

        plane.transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg*(tick*4)+90);
        plane.rectTransform.localPosition = new Vector3(Mathf.Cos(tick*4)*128, Mathf.Sin(tick*4)*128, -1);

        start_time -= Time.deltaTime;
        if (start_time <= 0)
        {
            LoadedGame();
        }
    }


    public void LoadedGame()
    {
        SceneManager.LoadScene("menu");
    }
}
