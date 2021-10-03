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
    public TextMeshProUGUI pokeName;
    public TextMeshProUGUI pokeID;
    public TextMeshProUGUI[] pokeTypeArray;

    public Button pokeRandomBtn;

    // Start is called before the first frame update
    void Start()
    {
        pokeName.text = pokeID.text = "";
        pokeRandomBtn.onClick.AddListener(BtnRandomPoke);
        InitialUI();
    }
    public void BtnRandomPoke()
    {
        int rndPokeID = Random.Range(1, 887);

        pokeName.text = "Loading...";
        pokeID.text = "#" + rndPokeID;

        InitialUI();
        StartCoroutine(HttpManager.GetPokeAtIndex(rndPokeID,OnCompleteGetPoke));
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
        print(pokeInfo["name"]);
        pokeName.text = FirstLetterUppercase(pokeInfo["name"]);
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
        pokeImage.texture = Texture2D.whiteTexture;

        foreach (var pokeType in pokeTypeArray)
        {
            pokeType.text = "";
        }
    }

    private string FirstLetterUppercase(string str) => 
        char.ToUpper(str[0]) + str.Substring(1);
    
}
