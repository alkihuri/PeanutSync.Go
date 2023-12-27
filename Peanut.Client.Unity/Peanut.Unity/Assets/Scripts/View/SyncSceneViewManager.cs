using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;

public class SyncSceneViewManager : MonoBehaviour
{
    private int frameCount = 0;
    private const int framesPerRequest = 6; // Отправлять запрос каждый 200-й кадр
    private const int port = 5267;
    private string setStructureURL = $"http://localhost:{port}/SetCurrentSceneStructure";
    private string getCurrentStructureURL = $"http://localhost:{port}/GetCurrentSceneStructure";

    [SerializeField] private SceneData _currentSceneStructure = new SceneData();
    [SerializeField] private SceneData _sceneDataFromServer = new SceneData();
    private List<Transform> _listOfNativeTransforms = new List<Transform>();

    public UnityEvent OnAnyObjectChanged = new UnityEvent();

    private void Awake()
    {
        OnAnyObjectChanged.AddListener(SendSceneSctructureInvoke);


        SendSceneSctructureInvoke();
    }


    private async void Update()
    {
        frameCount++;

        if (frameCount % framesPerRequest == 0)
        {
            StartCoroutine(GetSceneStructure());
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

        }
    }


    private IEnumerator GetSceneStructure()
    { 
        UnityWebRequest www = UnityWebRequest.Get(getCurrentStructureURL);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Получение текстового ответа от сервера
            string responseText = www.downloadHandler.text;
            Debug.Log("Server Response (GET): " + responseText);

            _sceneDataFromServer = JsonUtility.FromJson<SceneData>(responseText);

            UpdatePistions(_sceneDataFromServer);
            // Здесь вы можете обработать ответ, например, десериализовать JSON
        }
        else
        {
            Debug.LogError("GET Request Error: " + www.error);
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
        _listOfNativeTransforms = GetComponentsInChildren<Transform>().ToList();
        _listOfNativeTransforms.Remove(transform);

        SceneData sceneData = new SceneData();

        foreach (Transform t in _listOfNativeTransforms)
        {

            if (!t.gameObject.GetComponent<TransformController>())
                t.gameObject.AddComponent<TransformController>().Innit(this);

            // mapping 

            List<Position> positions = new List<Position>();

            foreach (Vector3 v3 in t.gameObject.GetComponent<TransformController>().LastPositions)
            {
                positions.Add(new Position(v3));
            }


            Position p = new Position(t.position);
            sceneData.GameObjectsUnity.Add(new GameObjectUnity(t.gameObject.GetInstanceID(), gameObject.name, positions));
        }

        return sceneData;
    }
    private void UpdatePistions(SceneData sceneData)
    {


        foreach (var gameObject in sceneData.GameObjectsUnity)
        {

            var objectToSync = _currentSceneStructure.GameObjectsUnity.Where(x => x.ID == gameObject.ID).First().GetRealSceneGameObject(_listOfNativeTransforms);

            //  print(objectToSync.name + "<color=green> synced </color>");

            objectToSync.transform.localPosition = gameObject.Positions.LastOrDefault().UnityVector3;
        }

    }
}
