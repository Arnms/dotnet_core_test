using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using dotnet_server_test.Datas;
using dotnet_server_test.Attributes;
using dotnet_server_test.Models.Dtos;
using dotnet_server_test.Constants;

namespace dotnet_server_test.Apps;

[ApiController]
[Route("[controller]")]
public class AppController : ControllerBase
{
    private readonly AppManager _appManager;
    private readonly IMapper _mapper;

    public AppController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _appManager = new AppManager(dbContext, mapper);
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("app")]
    public async Task<IActionResult> GetApp() => Ok(new DataResponseModel<App>
    {
        Data = await _appManager.GetApp(),
        Code = (int)ResponseCode.OK,
        Message = "OK"
    });
}