import {Pipe, PipeTransform} from '@angular/core';
import {ServiceStatuses} from "../models/InstanceModel";

@Pipe({
  name: 'serviceStatusPipe',
  standalone: true
})
export class ServiceStatusPipePipe implements PipeTransform {

  transform(value: ServiceStatuses, type: string): string {
    switch (type){
      case "style":
        switch (value){
          case 0:
            return "working"
          case 1:
            return "warning"
          case 2:
            return "offline"
          default:
            return ""
        }

      default:
        switch (value){
          case 0:
            return "Работает в умеренной нагрузке"
          case 1:
            return "Работает в повышенной нагрузке"
          case 2:
            return "Не в сети"
          default:
            return ""
        }
    }
  }
}
