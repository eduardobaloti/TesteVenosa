using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Networking;

public class ColorRequest : MonoBehaviour
{
    Material material;
    public TextMeshProUGUI logText;
    System.Random random = new System.Random();

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        InvokeRepeating("ChangeColor", 1f, 1f);
    }

    void ChangeColor()
    {
        StartCoroutine(GetColor());
    }

    IEnumerator GetColor()
    {
        string id = random.Next(1, 7).ToString();
        string query = $"{{\"query\":\"query {{\\n  color(id:\\\"{id}\\\") {{\\n    data {{\\n      attributes {{\\n        hex\\n      }}\\n    }}\\n  }}\\n}}\"}}";

        using (UnityWebRequest webRequest = UnityWebRequest.Put("http://localhost:1337/graphql", query))
        {
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                logText.text = "Error";
                logText.color = Color.red;
            }
            else
            {
                var ret = webRequest.downloadHandler.text;
                int indiceDoHashtag = ret.IndexOf('#');
                string hex = ret.Substring(indiceDoHashtag, Mathf.Min(7, ret.Length - indiceDoHashtag));
                Color color = HexToColor(hex);
                logText.text = hex;
                material.color = color;
            }
        }
    }


    Color HexToColor(string hex)
    {
        hex = hex.TrimStart('#');
        float r = System.Convert.ToInt32(hex.Substring(0, 2), 16) / 255f;
        float g = System.Convert.ToInt32(hex.Substring(2, 2), 16) / 255f;
        float b = System.Convert.ToInt32(hex.Substring(4, 2), 16) / 255f;
        return new Color(r, g, b);
    }

}

