using UnityEngine;

public class Click : MonoBehaviour
{
    private Animator objectAnimator;
    private Dizimos dizimos;

    void Start()
    {
        objectAnimator = GetComponent<Animator>();

        GameObject gameObject = GameObject.Find("Dizimos");
        dizimos = gameObject.GetComponent<Dizimos>();
    }

    void OnMouseDown()
    {
        objectAnimator.Play("ChurchClick", 0, 0f);
        dizimos.AddDizimo(1);
    }
}
