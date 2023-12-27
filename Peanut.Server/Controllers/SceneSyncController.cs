using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using  Peanut.Server.Domain;

namespace Peanut.Server.Controllers;

[ApiController] 
public class SceneSyncController : ControllerBase
{
     
    private SceneData _sceneData = new SceneData();

    [HttpGet] 
    [Route("GetCurrentSceneStructure")]
    public IActionResult Get()
    {
        if (_sceneData != null)
        {
            return Ok(_sceneData);
        }
        else
        {
            return NotFound(); // Или другой код состояния в зависимости от вашего требования
        }
    }


    [HttpGet] 
    [Route("GetPositionByName")]
    public string GetPositionByName(string name)
    {
        return _sceneData?.GameObjectsUnity.Where(o=>o.Name == name).First().Positions.Last().x.ToString();
    }

    
    [HttpPost]
    [Route("SetCurrentSceneStructure")]
    public async Task<IActionResult> Post()
    {
        using (StreamReader reader = new StreamReader(Request.Body))
        {
            string jsonData = await reader.ReadToEndAsync(); // Асинхронное чтение данных из потока
            SceneData sceneStructure  = new SceneData();
            try
            {
                sceneStructure = JsonSerializer.Deserialize<SceneData>(jsonData); 
               // System.IO.File.WriteAllText("data_client.json",JsonSerializer.Serialize(sceneStructure));
                //System.IO.File.WriteAllText("data_client2.json",jsonData);
                // ... ваш код обработки данных ...
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Ошибка десериализации JSON: " + ex.Message);
            }

            
            _sceneData = sceneStructure;   

            foreach(var obj in _sceneData.GameObjectsUnity)
            {

                var nextPoint = PredictionUtility.PredictNextPoint(obj.Positions.ToArray());
                var lastPoint = obj.Positions.Last();

                bool IsZero = nextPoint.x==0 && nextPoint.y ==0; 


                obj.PredictedPosition  =  IsZero ? lastPoint : nextPoint; 
            } 

            return Ok(_sceneData);
        }
    }



}
