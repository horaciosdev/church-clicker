using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dizimos : MonoBehaviour
{
    public float tootalDizimo = 0f;
    public TextMeshProUGUI totalDizimoStr;

    public float GetDizimo()
    {
        return tootalDizimo;
    }

    public void AddDizimo(float valor)
    {
        tootalDizimo += valor;
        this.totalDizimoStr.text = ((int)this.tootalDizimo).ToString();
    }

    public void RemoveDizimo(int valor)
    {
        tootalDizimo -= valor;
        this.totalDizimoStr.text = ((int)this.tootalDizimo).ToString();
    }
}
