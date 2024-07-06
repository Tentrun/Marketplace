import {Component, Inject} from '@angular/core';
import {TuiAlertService} from "@taiga-ui/core";
import {Router} from "@angular/router";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  activeItemIndex = 0;

  constructor(
    @Inject(TuiAlertService)
    private readonly alerts: TuiAlertService, private readonly router: Router
  ) {

  }

  onClick(item: string): void {
    this.router.navigate([`${item}`]);
    //this.alerts.open(item).subscribe();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
