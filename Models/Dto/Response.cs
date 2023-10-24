namespace dotnet_server_test.Models.Dtos;

public class ResponseModel
{
  public int Code { get; set; }
  public string? Message { get; set; }
}

public class DataResponseModel<T> : ResponseModel
{
  public T? Data { get; set; }
}