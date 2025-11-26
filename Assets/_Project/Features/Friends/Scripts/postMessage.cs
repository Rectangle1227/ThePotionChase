using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI?拐辣?閬??迨銵?
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;


public class postMessage : MonoBehaviour
{
    public InputField receiverName;//?嗡辣鈭?
    public InputField message;//靽∩辣?批捆
    public string senderName;//?拙振?箏??犖
    public Text sendText;//撖?蝷?

    //public GameObject panel;

    [Serializable]//?拙振撖縑?批捆
    public class messager
    {
        public string sendName;
        public string name;
        public string messT;
    }

    public messager sendMessage;

    public void Post()
    {
        //panel.SetActive(true);
        string reName = receiverName.text;
        string mess = message.text;

        sendMessage.name = reName;
        sendMessage.messT = mess;
        sendMessage.sendName = playerAttributeManager.Instance.pname;

        // 撠隞嗉??JSON摮泵銝?
        string jsonPost = JsonUtility.ToJson(sendMessage);

        if (jsonPost != null && sendMessage.messT != "")
        {
            // ?潮OST隢?
            StartCoroutine(SendMessageT(jsonPost));
            
        }
        //panel.SetActive(false);
    }

    IEnumerator SendMessageT(string jsonData)
    {
        // 撱箇?銝?”??
        WWWForm form = new WWWForm();
        form.AddField("json", jsonData);

        using (UnityWebRequest www = UnityWebRequest.Post("http://140.136.151.69/friend/postMessage.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                sendText.text = responseText;
            }
        }
        receiverName.text = null;
        message.text = null;
    }
}