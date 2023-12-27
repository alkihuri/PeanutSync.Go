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
    public async Task<IActionResult> Get()
    {

        var file = JsonSerializer.Deserialize<SceneData>(System.IO.File.ReadAllText("data_client.json"));
        if (_sceneData != null)
        {
            return Ok(file);
        }
        else
        {
            return NotFound(); // Или другой код состояния в зависимости от вашего требования
        }
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
                System.IO.File.WriteAllText("data_client2.json",jsonData);
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

            System.IO.File.WriteAllText("data_client.json",JsonSerializer.Serialize(_sceneData));
            return Ok(_sceneData);
        }
    }



}
