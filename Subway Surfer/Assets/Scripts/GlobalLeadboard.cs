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

    public List<LeadBoardInfo> LeadBoardInfos = new List<LeadBoardInfo>();
    public TextMeshProUGUI PrefabText;
    public Transform ButtonParent;
    public static string currentName;
    bool scoreIsBigger = false;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //Get Data from server
        StartCoroutine(GetRequest("https://301.sebight.eu/api/leaderboard/2NF9IGg8iG"));
       

    }

    

    //Get Data from server
    IEnumerator GetRequest(string uri)
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

            LeadBoardInfos.Clear();
            LeadBoardInfos = JsonConvert.DeserializeObject<List<LeadBoardInfo>>(uwr.downloadHandler.text);
            CreatingUI();
        }
    }

   public void IsScoreBigger(int score)
    {
        foreach (LeadBoardInfo element in LeadBoardInfos)
        {
            if (element.score < score)
            {
                scoreIsBigger = true;
                break;
            }
        }

        if (scoreIsBigger)
        {
            LeadBoardInfo leadBoardInfo = new LeadBoardInfo()
            {
                score = score,
                username = currentName

            };

            DataToSend dataToSend = new DataToSend()
            {
                token = "i7hhbXCdL9m5Cwt",
                leaderboard = leadBoardInfo

            };

            string json = JsonConvert.SerializeObject(dataToSend, Formatting.Indented);

            StartCoroutine(PostRequest("https://301.sebight.eu/api/leaderboard/2NF9IGg8iG", json));

            scoreIsBigger = false;
        }
       
    }

    
    //Put Data to server
    
    IEnumerator PostRequest(string url, string json)
    {
        

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);

        
        StartCoroutine(GetRequest("https://301.sebight.eu/api/leaderboard/2NF9IGg8iG"));
        
    }


    void CreatingUI()
    {
        LeadBoardInfos.Sort(SortFunction);

        foreach (LeadBoardInfo element in LeadBoardInfos)
        {
           TextMeshProUGUI text = Instantiate(PrefabText, ButtonParent.transform);

            text.text = element.username;
            Transform tff = text.transform.GetChild(0);

            tff.gameObject.GetComponent<TextMeshProUGUI>().text = element.score.ToString();
        }
    }

    private int SortFunction(LeadBoardInfo a, LeadBoardInfo b)
    {
        if(a.score < b.score)
        {
            return 1;
        }
        else if (a.score > b.score)
        {
            return -1;
        }
        return 0;
    }
}
