export class ApiEndpointsConsts {
  private static DefaultHost = 'https://localhost';
  //Под версионированние апи, в последующем понадобиться
  public static VERSION = '1.0.0';
  private static ServicesAddresses = {
    WebApi: `${ApiEndpointsConsts.DefaultHost}:7116`
  }
  public static ServicesMethods = {
    GetInstancesStatuses: `${ApiEndpointsConsts.ServicesAddresses.WebApi}/Settings/GetInstanceStatuses`,
    GetProductsOfTheDay: `${ApiEndpointsConsts.ServicesAddresses.WebApi}/UserProducts/GetProductsOfTheDay`
  }
}
