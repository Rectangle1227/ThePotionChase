using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Message
{
    public string sender;
    public string message;
}

public class ReplyALL : MonoBehaviour
{
    public string receiver;
    public List<Text> requestText;//?其?憿舐內隤啣??隢策雿?
    public GameObject panel;

    public void GetMessages()
    {
        StartCoroutine(GetMessagesFromPHP());
    }

    public void ClearPreviousContent()
    {
        //皜?＊蝷?
        foreach (Text request in requestText)
        {
            request.text = "";
        }
        panel.SetActive(false);
    }


    IEnumerator GetMessagesFromPHP()
    {
        panel.SetActive(true);
        receiver = playerNameManager.Instance.playerName;

        //???拙振??POST?郡hp
        WWWForm form = new WWWForm();
        form.AddField("receiver", receiver);

        //POST?郡hp敺?敺hp瘙??函摰嗅末??蝷箄”鋆∪?鈭文??隢?鞈?
        using UnityWebRequest www = UnityWebRequest.Post("http://140.136.151.69/friend/replyAll.php", form);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //閫???嗅?son?澆??????
            string jsonData = www.downloadHandler.text;
            if (jsonData == "瘝?鈭箏???")
            {
                Debug.Log(jsonData);
            }
            else
            {
                jsonData = "{\"Items\":" + jsonData + "}";//?son?澆??耨敺拙?臭誑?沅sonHelper?撘?
                Message[] requestYes = JsonHelper.FromJson<Message>(jsonData);

                //??憟賢??隢??拙振??摮invitationData???
                for (int i = 0; i < requestYes.Length; i++)
                {
                    string senderName = requestYes[i].sender;
                    string mType = requestYes[i].message;
                    Text request = requestText[i];

                    //?芸摰嗅???隢?
                    request.text = "?拙振 " + senderName + mType;
                }
            }
        }         
    }


        public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

}