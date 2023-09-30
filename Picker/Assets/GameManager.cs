using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // baska bir class ekledigimiz icin yazmis olabiliriz notlarima ekledim inceleyecegim.
using TMPro;

[Serializable]

public class TopAlaniTeknikIslemler
{
    public Animator TopAlaniAsansor;
    public TextMeshProUGUI SayiText;
    public int AtilmasiGerekenTop;
    public GameObject[] Toplar; 
}

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject ToplayiciObje;
    [SerializeField] private GameObject TopKontrolObjesi;
    public bool ToplayiciHaraketDurumu;

    int AtilanTopSayisi;
    [SerializeField] private List<TopAlaniTeknikIslemler> _TopAlaniTeknikIslemler = new List<TopAlaniTeknikIslemler>();


    // Start is called before the first frame update
    void Start()
    {
        ToplayiciHaraketDurumu = true;
        _TopAlaniTeknikIslemler[0].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikIslemler[0].AtilmasiGerekenTop;
    }

    // Update is called once per frame
    void Update()
    {

        if (ToplayiciHaraketDurumu)
        {
            ToplayiciObje.transform.position += 5f * Time.deltaTime * ToplayiciObje.transform.forward; // ileri dogru gitmesi icin komut


            if (Time.timeScale != 0) // OYUN DURMAMISSA
            {

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ToplayiciObje.transform.position.x - .1f,
                        ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), .05f);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(ToplayiciObje.transform.position.x + .1f,
                        ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), .05f);
                }
            }
        }

        

    }

    public void SiniraGelindi()
    {
        ToplayiciHaraketDurumu = false;
        Invoke("AsamaKontrol", 2f); // beli bir sure sonra metodu cagirma komutu 2f sure sonra cagir su metodu diyoruz. !

        //OverlapBox() -> bir menzil olusturmak
        Collider[] HitColl = Physics.OverlapBox(TopKontrolObjesi.transform.position, TopKontrolObjesi.transform.localScale / 2, Quaternion.identity);

        int i = 0;
        while (i < HitColl.Length) // Menzilde almis oldugun objelerin iclerine tek tek gir diyorum
        {

            
            HitColl[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, .8f), ForceMode.Impulse);

            i++;
        }

        Debug.Log(i);

       
       //gizmos kordinatlara gore cizgiler cekmek
    }

    public void ToplariSay()
    {
        AtilanTopSayisi++;
        _TopAlaniTeknikIslemler[0].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikIslemler[0].AtilmasiGerekenTop;
    }

    void AsamaKontrol()
    {
        if (AtilanTopSayisi >= _TopAlaniTeknikIslemler[0].AtilmasiGerekenTop)
        {
            Debug.Log("KAZANDIN");
            _TopAlaniTeknikIslemler[0].TopAlaniAsansor.Play("Asansor");

            foreach(var item in _TopAlaniTeknikIslemler[0].Toplar)
            {
                item.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Kaybettin");
        }
    }
}
