import {ChangeDetectorRef, Component} from '@angular/core';
import {ReactiveFormsModule} from "@angular/forms";
import {TuiCheckboxModule, TuiTagModule} from "@taiga-ui/kit";
import {TuiButtonModule} from "@taiga-ui/core";
import {TuiTableModule} from "@taiga-ui/addon-table";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {TuiLetModule} from "@taiga-ui/cdk";
import {InstancesServiceService} from "../../services/InstanceService/instances-service.service";
import {InstanceModel} from "../../models/InstanceModel";
import {NotificationService} from "../../services/notificationService/notification.service";
import {ServiceStatusPipePipe} from "../../pipes/service-status-pipe.pipe";

@Component({
  selector: 'app-services-status',
  standalone: true,
  imports: [
    TuiCheckboxModule,
    ReactiveFormsModule,
    TuiButtonModule,
    TuiButtonModule,
    TuiTableModule,
    TuiTagModule,
    NgForOf,
    NgIf,
    TuiLetModule,
    AsyncPipe,
    ServiceStatusPipePipe
  ],
  templateUrl: './services-status.component.html',
  styleUrl: './services-status.component.less'
})
export class ServicesStatusComponent {
  instances: InstanceModel[] = [];
  isLoading: boolean = true;
  isError: boolean = false;

  constructor(private instanceService: InstancesServiceService,
              private cd: ChangeDetectorRef,
              private readonly notificationsService: NotificationService ) {
  }

  ngOnInit(){
    this.instanceService.getInstances()
      .subscribe({
      next: result => {
        this.instances = result;
        this.isLoading = false;
      },
      complete : () => this.notificationsService.notifyInfo("Получены данные по инстансам"),
      error : () => {
        this.notificationsService.notifyError("Не удалось получить информацию по инстансам")
        this.isLoading = false;
        this.isError = true;
      }
    })
  }
}
