using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LeadBoardInfo
{
    public int score;
    public string username;
}

[System.Serializable]
public class DataToSend
{
    public string token;
    public LeadBoardInfo leaderboard;

}

public class GlobalLeadboard : MonoBehaviour
{

    public List<LeadBoardInfo> leadBoardInfos = new List<LeadBoardInfo>();
    public TextMeshProUGUI PrefabText;
    public Transform ButtonParent;


    void Start()
    {
        //Get Data from server
        StartCoroutine(getRequest("https://301.sebight.eu/api/leaderboard/2NF9IGg8iG"));
       

    }

    //Get Data from server
    IEnumerator getRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            leadBoardInfos = JsonConvert.DeserializeObject<List<LeadBoardInfo>>(uwr.downloadHandler.text);
            CreatingUI();
        }
    }

    void TestJson()
    {
        LeadBoardInfo leadBoardInfo = new LeadBoardInfo()
        {
            score = 1,
            username = "Username"

        };

        DataToSend dataToSend = new DataToSend()
        {
            token = "i7hhbXCdL9m5Cwt",
            leaderboard = leadBoardInfo

        };

        string json = JsonConvert.SerializeObject(dataToSend, Formatting.Indented);

        StartCoroutine(postRequest("https://301.sebight.eu/api/leaderboard/2NF9IGg8iG", json));
        Debug.Log(json);

    }

    
    //Put Data to server
    
    IEnumerator postRequest(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);

        StartCoroutine(getRequest("https://301.sebight.eu/api/leaderboard/2NF9IGg8iG"));
    }


    void CreatingUI()
    {
        foreach (LeadBoardInfo element in leadBoardInfos)
        {
           TextMeshProUGUI text = Instantiate(PrefabText, ButtonParent.transform);

            text.text = element.username;
            Transform tff = text.transform.GetChild(0);

            tff.gameObject.GetComponent<TextMeshProUGUI>().text = element.score.ToString();
        }
    }
}
