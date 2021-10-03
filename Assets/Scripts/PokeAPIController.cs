using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokeAPIController : MonoBehaviour
{
    public RawImage pokeImage;
    public TextMeshProUGUI pokeNameText;
    public TextMeshProUGUI pokeIDText;
    public TextMeshProUGUI[] pokeTypeArray;

    public Button pokeRandomBtn;
    public TMP_InputField idInput;
    public Button pokeByIdBtn;

    // Start is called before the first frame update
    void Start()
    {
        pokeRandomBtn.onClick.AddListener(delegate { BtnRandomPoke(); });
        pokeByIdBtn.onClick.AddListener(delegate { BtnRandomPoke(int.Parse(idInput.text)); });
        
        InitialUI();
    }
    public void BtnRandomPoke(int id = -1)
    {
        int pokeID;
        if (id == -1)
        {
            pokeID = Random.Range(1, 887);
        }
        else
        {
            pokeID = id;
        }
        print(isIdValid(pokeID));
        if (isIdValid(pokeID))
        {

            pokeNameText.text = "Loading...";
            pokeIDText.text = "#" + pokeID;

            idInput.GetComponent<Outline>().enabled = false;
            
            StartCoroutine(HttpManager.GetPokeAtIndex(pokeID, OnCompleteGetPoke));
        }
        else
        {
            idInput.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnCompleteGetPoke(JSONNode pokeInfo)
    {
        ViewPoke(pokeInfo);
        StartCoroutine(HttpManager.GetPokeSprite(pokeInfo["sprites"]["front_default"], OnCompleteGetPokeSprite));
    }

    private void OnCompleteGetPokeSprite(Texture2D text2D)=>
        ViewPokeSprite(text2D);

    private void ViewPoke(JSONNode pokeInfo)
    {
        pokeNameText.text = FirstLetterUppercase(pokeInfo["name"]);
        JSONNode types = pokeInfo["types"];
        for (int i = 0; i < types.Count; i++)
        {
            pokeTypeArray[i].text = types[i]["type"]["name"];
        }
    }

    private void ViewPokeSprite(Texture2D text2D)
    {
        pokeImage.texture = text2D;
        pokeImage.texture.filterMode = FilterMode.Point;
    }

    private void InitialUI()
    {
        pokeNameText.text = pokeIDText.text = "";
        pokeImage.texture = Texture2D.whiteTexture;

        foreach (var pokeType in pokeTypeArray)
        {
            pokeType.text = "";
        }
    }
    private bool isIdValid(int id)
    {
        if (id <= 0 || id >= 887)
            return false;
        else
            return true;
    }
    private string FirstLetterUppercase(string str) => 
        char.ToUpper(str[0]) + str.Substring(1);
}
