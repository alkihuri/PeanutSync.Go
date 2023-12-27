using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using System.Collections;
using UnityEditor;

public class SyncSceneViewManager : MonoBehaviour
{
    private int frameCount = 0;
    private const int framesPerRequest = 200; // Отправлять запрос каждый 200-й кадр
    private const int port = 5267;
    private string setStructureURL = $"http://localhost:{port}/SetCurrentSceneStructure";
    private string getCurrentStructureURL = $"http://localhost:{port}/GetCurrentSceneStructure";

    [SerializeField] private SceneData _currentSceneStructure = new SceneData(new List<GameObjectUnity>());
    [SerializeField] private SceneData _sceneDataFromServer = new SceneData(new List<GameObjectUnity>());

    private async void Update()
    {
        frameCount++;

        if (frameCount % framesPerRequest == 0)
        {
            SendSceneSctructureInvoke();
        }
    }

    private void SendSceneSctructureInvoke()
    {
        StartCoroutine(SetSceneStructure());
    }

    private IEnumerator SetSceneStructure()
    {
        _currentSceneStructure = GetCurrentSceneStructure();
        string jsonData = JsonUtility.ToJson(_currentSceneStructure);

        UnityWebRequest www = new UnityWebRequest(setStructureURL, "POST");
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        DebugToFile(jsonBytes);

        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        www.downloadHandler = new DownloadHandlerBuffer();

        www.SetRequestHeader("Content-Type", "application/json"); // Устанавливаем заголовок Content-Type

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        { 
            string responseText = www.downloadHandler.text; // Получение текстового ответа от сервера
            Debug.Log("Server Response: " + responseText);

            // Дополнительная обработка ответа, например, десериализация JSON
            _sceneDataFromServer = JsonUtility.FromJson<SceneData>(responseText);
            UpdatePistions(_sceneDataFromServer);
            // Дальнейшая обработка sceneData...
        }
    }

    private void UpdatePistions(SceneData sceneData)
    {
        foreach (var gameObject in sceneData.GameObjectsUnity)
        {
            _currentSceneStructure.GameObjectsUnity.Where(x => x.Name == gameObject.Name).First().Position = gameObject.Position;
            Debug.Log(gameObject.Name + " position updated");
        }

    }

    private static void DebugToFile(byte[] jsonBytes)
    {
        string debugDataPath = Application.persistentDataPath + "/data.json";
        // Debug.Log(debugDataPath);
        System.IO.File.WriteAllBytes(debugDataPath, jsonBytes);
    }

    private SceneData GetCurrentSceneStructure()
    {
        var listOfNativeTransforms = GetComponentsInChildren<Transform>();
        SceneData sceneData = new SceneData(new List<GameObjectUnity>());

        foreach (Transform t in listOfNativeTransforms)
        {
            // mapping 
            Position p = new Position(t.position);
            sceneData.GameObjectsUnity.Add(new GameObjectUnity(t.gameObject.name, p));
        }

        return sceneData;
    }
}
