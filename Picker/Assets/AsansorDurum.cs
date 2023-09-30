using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsansorDurum : MonoBehaviour
{
    [SerializeField] private GameManager _GameManager;
    [SerializeField] private Animator BariyerAlani;

    public void BariyerKaldir()
    {
        BariyerAlani.Play("BariyerKaldir");
    }

    public void Bitti()
    {
        _GameManager.ToplayiciHaraketDurumu = true;
    }


}
