namespace API.Controllers;
public class BuggyController : BaseApiController
{
    private readonly StoreContext _storeContext;

    public BuggyController(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        var thing = _storeContext.Products.Find(42);
        if (thing is null)
        {
            return NotFound(new ApiResponse(404));
        }
        return Ok();
    }


    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        var thing = _storeContext.Products.Find(42);
        var thingToReturn = thing.ToString();
        //try
        //{
        //}
        //catch
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError));
        //}
        return Ok();
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return Ok();
    }
}

