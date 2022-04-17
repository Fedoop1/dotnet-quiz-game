import { Component, OnInit } from '@angular/core';
import { ErrorNotificationService } from 'src/app/services/error-notification.service';
import { DestroyableComponent } from '../destroyable-component/destroyable.component';

@Component({
  selector: 'error-notification',
  templateUrl: 'error-notification.component.html',
  styleUrls: ['error-notification.component.scss'],
})
export class ErrorNotificationComponent extends DestroyableComponent {
  constructor(public errorNotificationService: ErrorNotificationService) {
    super();
  }

  public close() {
    this.errorNotificationService.clear();
  }
}
