1. set a global json file
2. follow this guid : https://github.com/domaindrivendev/Swashbuckle.AspNetCore#using-the-tool-with-the-net-core-30-sdk-or-later
3. compile project 
4. run `dotnet swagger tofile --output v1.swagger.json ".\bin\Debug\netcoreapp3.1\SomeWebApp.dll" v1`  
   this should create on your project folder a file the swagger file : `v1.swagger.json`

References:
================
  * [swaggerGenerationLibrary](https://msazure.visualstudio.com/One/_git/AGE-Documents?path=%2Fdocs%2FCommon%2FswaggerGenerationLibrary.md&version=GBmaster&_a=preview)
  * [more custom swagger filters](https://msazure.visualstudio.com/One/_git/DI-Agri?path=/src/PaaS/src/csharp/BaseNetCoreApp/ServiceCollectionExtentions/Helpers)
  * [autorest/extensions](http://azure.github.io/autorest/extensions/)
  * [example](https://dev.azure.com/msazure/One/_git/DI-Agri/pullrequest/5144979?_a=files&path=/src/PaaS/src/csharp/ResourceProviderService/Docs/OpenApiSpecs/latest/semi_automated_swagger.json)
  * [PPT](https://microsoft-my.sharepoint.com/:p:/p/prjayasw/Ed7S0Ia9ZnVGhB1WQK16T5IBLsd4V_O-sxjizYcUuYjo8Q)

 
capabilty example:
======================
https://dev.azure.com/msazure/One/_git/DI-Agri/pullrequest/5144979
https://msazure.visualstudio.com/One/_git/DI-Agri?path=/src/PaaS/src/csharp/ResourceProviderService/Docs/arm-swagger/agfood/resource-manager/Microsoft.AgFoodPlatform/preview/2020-05-12-preview/agfood.json