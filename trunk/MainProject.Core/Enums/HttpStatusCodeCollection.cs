namespace MainProject.Core.Enums
{
    /// <summary>
    /// Response result base on https://docs.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=netframework-4.7.2
    /// </summary>
    public enum HttpStatusCodeCollection
    {
        OK = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        BadGateway = 502,
    }
}
