using UnityEngine;
using UnityEngine.UI;
public class LvlSelectButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private MENU_MANAGER MENU_MANAGER;
    [SerializeField] private int lvl;

    [SerializeField] private bool is_next_page = false;

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        Image image = gameObject.GetComponent<Image>();


        if (MENU_MANAGER.MaxLevelPlayerGot + 1 < lvl)
        {
            button.interactable = false;
            if (is_next_page == false)
            {
                image.sprite = Sprite.Create(MENU_MANAGER.Locked_lvl, new Rect(0, 0, MENU_MANAGER.Locked_lvl.width, MENU_MANAGER.Locked_lvl.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                image.sprite = Sprite.Create(MENU_MANAGER.Locked_Next_Page, new Rect(0, 0, MENU_MANAGER.Locked_Next_Page.width, MENU_MANAGER.Locked_Next_Page.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
