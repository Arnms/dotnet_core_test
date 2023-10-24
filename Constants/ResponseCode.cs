namespace dotnet_server_test.Constants;

public enum ResponseCode
{
    // General Code
    OK,

    // Authorization Code
    UNAUTHORIZE_TOKEN_INVALIDE = 4010,
    UNAUTHORIZE_TOKEN_EXPIRED = 4011,
    UNAUTHORIZE_NOT_FOUND_USER = 4012
};