

namespace Cw6.Services
{
    public interface IDbservice
    {
        bool ckeckIndex(string index);
    }
}
/*
1. httpContext.Request.Method
2. [..].Path
3. [..].Body
4. [..].QueryString
​
    
var bodyStream = string.Empty;


                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStream = await reader.ReadToEndAsync();
                }


*/