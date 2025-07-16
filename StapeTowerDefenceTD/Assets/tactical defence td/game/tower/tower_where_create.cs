using UnityEngine;
using System.Collections.Generic;
public class tower_where_create : MonoBehaviour
{
    [SerializeField] private List<Tower> towers;
    [SerializeField] private FullGameManager FullGameManager;

    public Tower tower_that_created = null;

    public void Tap()
    {
        if (tower_that_created == null)
        {
            FullGameManager.ChooseWhereTower(this);
        }
        else
        {
            FullGameManager.ChooseWhereDelete(this);
        }
    }

    public void choosenTower(int type)
    {
        tower_that_created = Instantiate(towers[type], gameObject.transform);
    }

    public void sell()
    {
        print("destroy");
        Destroy(tower_that_created.gameObject);
    }
}
