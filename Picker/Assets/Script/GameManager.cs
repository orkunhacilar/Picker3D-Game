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
    public ParticleSystem Yıldız;
    public AudioSource kazanma;
    public GameObject canvas;

    int AtilanTopSayisi;
    int ToplamCheckPointSayisi;
    int MevcutCheckPointIndex;
    float ParmakPozX;
    [SerializeField] private List<TopAlaniTeknikIslemler> _TopAlaniTeknikIslemler = new List<TopAlaniTeknikIslemler>();


    // Start is called before the first frame update
    void Start()
    {
        ToplayiciHaraketDurumu = true;

        for(int i=0; i<_TopAlaniTeknikIslemler.Count; i++)
        {
            _TopAlaniTeknikIslemler[i].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikIslemler[i].AtilmasiGerekenTop;
        }

        ToplamCheckPointSayisi = _TopAlaniTeknikIslemler.Count-1;
    }

    // Update is called once per frame
    void Update()
    {

        if (ToplayiciHaraketDurumu)
        {
            ToplayiciObje.transform.position += 5f * Time.deltaTime * ToplayiciObje.transform.forward; // ileri dogru gitmesi icin komut

            if (Time.timeScale != 0) // OYUN DURMAMISSA
            {
                if (Input.touchCount > 0) //Ekranda dokunma var mi yok mu onu kontrol ediyoruz
                {
                    Touch touch = Input.GetTouch(0);

                    Vector3 TouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            ParmakPozX = TouchPosition.x - ToplayiciObje.transform.position.x;
                            break;

                        case TouchPhase.Moved:

                            if (TouchPosition.x - ParmakPozX > -1.15 && TouchPosition.x - ParmakPozX < 1.15)
                            {
                                ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position, new Vector3(TouchPosition.x - ParmakPozX,
                                    ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), 3f);
                            }
                            break;

                    }
                }
            }
        }

                

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
        _TopAlaniTeknikIslemler[MevcutCheckPointIndex].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikIslemler[MevcutCheckPointIndex].AtilmasiGerekenTop;
    }

    void AsamaKontrol()
    {
        if (AtilanTopSayisi >= _TopAlaniTeknikIslemler[MevcutCheckPointIndex].AtilmasiGerekenTop)
        {
            
            kazanma.Play();
            Yıldız.Play();
            Debug.Log("KAZANDIN");

            
            _TopAlaniTeknikIslemler[MevcutCheckPointIndex].TopAlaniAsansor.Play("Asansor");

            foreach(var item in _TopAlaniTeknikIslemler[MevcutCheckPointIndex].Toplar)
            {
                item.SetActive(false);
            }

            if(MevcutCheckPointIndex == ToplamCheckPointSayisi)
            {
                canvas.SetActive(true);
                kazanma.Play();
                Yıldız.Play();
                Debug.Log("OYUN BITTI - KAZANDIN PANELI ACIGA CIKABILIR");
                Time.timeScale = 0;
            }
            else
            {
                MevcutCheckPointIndex++;
                AtilanTopSayisi = 0;
            }

            
        }
        else
        {
            Debug.Log("Kaybettin - Kaybettin paneli aciga cikabilir");
        }
    }
}
