using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HttpManager : MonoBehaviour
{

    private const string basePokeURL = "https://pokeapi.co/api/v2/";
  
    public static IEnumerator GetPokeAtIndex(int pokeID, Action<SimpleJSON.JSONNode> onSuccess, Action<string> onError = null)
    {
        string url = basePokeURL + "pokemon/" + pokeID;
        UnityWebRequest pokeRequest = UnityWebRequest.Get(url);

        yield return pokeRequest.SendWebRequest();
       
        if (pokeRequest.isHttpError || pokeRequest.isNetworkError)
        {
            Debug.LogError(pokeRequest.error);
            onError(pokeRequest.error);
            yield break;
        }
        SimpleJSON.JSONNode data = SimpleJSON.JSON.Parse(pokeRequest.downloadHandler.text);
        onSuccess(data);
    }

    public static IEnumerator GetPokeSprite(string pokeSpriteUrl, Action<Texture2D> onSuccess, Action<string> onError=null) 
    {
        using (UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(pokeSpriteUrl))
        {
            yield return pokeSpriteRequest.SendWebRequest();
            if (pokeSpriteRequest.isHttpError || pokeSpriteRequest.isNetworkError)
            {
                Debug.LogError(pokeSpriteRequest.error);
                onError(pokeSpriteRequest.error);
                yield break;
            }
            onSuccess(DownloadHandlerTexture.GetContent(pokeSpriteRequest));
        }
    }
}
