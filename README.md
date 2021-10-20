1. set a global json file
2. follow this guid : https://github.com/domaindrivendev/Swashbuckle.AspNetCore#using-the-tool-with-the-net-core-30-sdk-or-later
3. compile project 
4. run `dotnet swagger tofile --output v1.swagger.json ".\bin\Debug\netcoreapp3.1\SomeWebApp.dll" v1`  
   this should create on your project folder a file the swagger file : `v1.swagger.json`