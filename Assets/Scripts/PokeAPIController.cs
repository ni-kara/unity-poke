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

    private readonly string basePokeURL = "https://pokeapi.co/api/v2/";
    // Start is called before the first frame update
    void Start()
    {
        pokeName.text = pokeID.text = "";

        InitialUI();
    }

    public void BtnRandomPoke()
    {
        int rndPokeID = Random.Range(1, 887);

        pokeName.text = "Loading...";
        pokeID.text = "#" + rndPokeID;

        InitialUI();

        StartCoroutine(GetPokeAtIndex(rndPokeID));
    }

    IEnumerator GetPokeAtIndex(int rndPokeID)
    {
        string url = basePokeURL + "pokemon/" + rndPokeID;
        UnityWebRequest pokeRequest = UnityWebRequest.Get(url);

        yield return pokeRequest.SendWebRequest();

        if (pokeRequest.isHttpError || pokeRequest.isNetworkError)
        {
            Debug.LogError(pokeRequest.error);
            yield break;
        }

        JSONNode pokeInfo = JSON.Parse(pokeRequest.downloadHandler.text);

        pokeName.text = FirstLetterUppercase(pokeInfo["name"]);

        JSONNode types = pokeInfo["types"];
        for (int i = 0; i < types.Count; i++)
        {
            pokeTypeArray[i].text = types[i]["type"]["name"];
        }

        string pokeSpriteURL = pokeInfo["sprites"]["front_default"];
        StartCoroutine(GetPokeSprite(pokeSpriteURL));
    }

    IEnumerator GetPokeSprite(string pokeSpriteURL)
    {
        UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(pokeSpriteURL);
        yield return pokeSpriteRequest.SendWebRequest();

        if (pokeSpriteRequest.isHttpError || pokeSpriteRequest.isNetworkError)
        {
            Debug.LogError(pokeSpriteRequest.error);
            yield break;
        }

        pokeImage.texture= DownloadHandlerTexture.GetContent(pokeSpriteRequest);
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

    private string FirstLetterUppercase(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}
