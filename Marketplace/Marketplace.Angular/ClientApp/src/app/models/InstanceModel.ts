export interface InstanceModel {
  readonly host: string;
  readonly port: string;
  readonly name: string;
  readonly serviceStatus: ServiceStatuses;
  readonly description: string;
}

export enum ServiceStatuses{
  Working = 0,
  Busy = 1,
  Offline = 2
}
