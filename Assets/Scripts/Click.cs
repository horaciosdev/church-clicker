using UnityEngine;

public class Click : MonoBehaviour
{
    private Animator objectAnimator;
    private Money money;

    void Start()
    {
        objectAnimator = GetComponent<Animator>();

        GameObject gameObject = GameObject.Find("Money");
        money = gameObject.GetComponent<Money>();
    }

    void OnMouseDown()
    {
        objectAnimator.Play("ChurchClick", 0, 0f);
        money.AddMoney(1);
    }
}
