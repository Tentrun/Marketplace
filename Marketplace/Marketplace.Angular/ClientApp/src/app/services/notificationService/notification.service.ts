import {Inject, Injectable} from '@angular/core';
import {TuiAlertService, TuiNotification} from "@taiga-ui/core";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    @Inject(TuiAlertService)
    private readonly notificationsService: TuiAlertService
  ) {
  }

  public notifySuccess = (text : string, title : string = 'Успех!') : void => {
    this.notificationsService
      .open(text, {
        label: title,
        status: TuiNotification.Success
      }).subscribe();
  }

  public notifyError = (text : string, title : string = 'Ошибка!') : void => {
    this.notificationsService
      .open(text, {
        label: title,
        status: TuiNotification.Error
      }).subscribe();
  }

  public notifyWarning = (text : string, title : string = 'Внимание!') : void => {
    this.notificationsService
      .open(text, {
        label: title,
        status: TuiNotification.Warning
      }).subscribe();
  }

  public notifyInfo= (text : string, title : string = 'Информация') : void => {
    this.notificationsService
      .open(text, {
        label: title,
        status: TuiNotification.Info
      }).subscribe();
  }
}
