using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Peanut.Server.Controllers;

[ApiController] 
public class SceneSyncController : ControllerBase
{
     
    private List<GameObjectUnity> _objectsOnScene = new List<GameObjectUnity>();

    [HttpGet] 
    [Route("GetCurrentSceneStructure")]
    public IActionResult Get()
    {
        return Ok(JsonSerializer.Serialize(_objectsOnScene));
    }

    [HttpPost]
    [Route("SendCurrentSceneStructure")]
    public IActionResult Post([FromBody] List<GameObjectUnity> sceneStructure)
    {
         _objectsOnScene = sceneStructure;
         return Ok(_objectsOnScene);
    }


}



public class GameObjectUnity
{
    public string Name { get; set; }
    public Vector3 Position { get; set; }
}
