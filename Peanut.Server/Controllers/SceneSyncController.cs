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
        return Ok(JsonSerializer.Serialize(_sceneData));
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
                System.IO.File.WriteAllText("data_client.json",JsonSerializer.Serialize(sceneStructure));
                System.IO.File.WriteAllText("data_client2.json",jsonData);
                // ... ваш код обработки данных ...
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Ошибка десериализации JSON: " + ex.Message);
            }

            
            _sceneData = sceneStructure;   
            return Ok(_sceneData);
        }
    }



}
