using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.Networking;
using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEditor.Experimental.GraphView;

public class SyncSceneManager : MonoBehaviour
{
    private int frameCount = 0;
    private const int framesPerRequest = 100; // Отправлять запрос каждый 10-й кадр
    private const int port = 5267;
    private string sendStructureURL = $"http://localhost:{port}/SendCurrentSceneStructure";
    private string getCurrentStructureURL = $"http://localhost:{port}/GetCurrentSceneStructure";

    private List<GameObject> _currentSceneStructure = new List<GameObject>();

    private async void Update()
    {
        frameCount++;

        if (frameCount % framesPerRequest == 0)
        {
            StartCoroutine(SendSceneStructure());
        }
    }

    private IEnumerator SendSceneStructure()
    {
        _currentSceneStructure = GetComponentsInChildren<Transform>().Select(t => t.gameObject ).ToList(); 

        UnityWebRequest sendStructureRequest = new UnityWebRequest(sendStructureURL);
        string jsonData = JsonUtility.ToJson(_currentSceneStructure); 
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        sendStructureRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        sendStructureRequest.SetRequestHeader("Content-Type", "application/json");
        yield return sendStructureRequest.SendWebRequest();
        // Проверяем, есть ли ошибка при отправке запроса
        if (sendStructureRequest.isNetworkError || sendStructureRequest.isHttpError)
        {
            Debug.LogError($"Error: {sendStructureRequest.error}");
        }
        else
        {
            // Запрос успешно отправлен, и ответ получен
            Debug.Log("Request successful!");
            Debug.Log("Response: " + sendStructureRequest.downloadHandler.text);
        }   

    }
}
