using UnityEngine;
using DG.Tweening;

public class Healthbar : MonoBehaviour
{

    private float value = 1.0f;
    private float max_value = 1.0f;

    [SerializeField] private GameObject front;
    
    public void Set_value(int _value)
    {
        value = _value;
        front.transform.DOScaleX(value / max_value, 0.05f).SetId(gameObject);
    }

    public void Setup_value(int _value, int _max_value)
    {
        value = _value;
        max_value = _max_value;
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
