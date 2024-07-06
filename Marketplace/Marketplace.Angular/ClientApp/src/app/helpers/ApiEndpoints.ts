export class ApiEndpointsConsts {
  private static DefaultHost = 'http://localhost';
  //Под версионированние апи, в последующем понадобиться
  public static VERSION = '1.0.0';
  private static ServicesAddresses = {
    WebApi: `${ApiEndpointsConsts.DefaultHost}:5028`
  }
  public static ServicesMethods = {
    GetInstancesStatuses: `${ApiEndpointsConsts.ServicesAddresses.WebApi}/Settings/GetInstanceStatuses`
  }
}
