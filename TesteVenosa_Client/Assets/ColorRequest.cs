using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ColorRequest : MonoBehaviour
{
    Material material;
    public TextMeshProUGUI logText;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        InvokeRepeating("ChangeColor", 2f, 1f);
    }

    void ChangeColor()
    {
        StartCoroutine(GetColor());
    }

    IEnumerator GetColor()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://x-colors.yurace.pro/api/random"))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                logText.text = "Error";
            }
            else
            {
                var json = webRequest.downloadHandler.text;
                ColorReq cReq = JsonUtility.FromJson<ColorReq>(json);
                Color color = HexToColor(cReq.hex);
                logText.text = cReq.hex;
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

    public class ColorReq
    {
        public string hex;
        public string rgb;
        public string hsl;

    }
}

