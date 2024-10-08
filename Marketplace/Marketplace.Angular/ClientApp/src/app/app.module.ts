import {NgDompurifySanitizer} from "@tinkoff/ng-dompurify";
import {
  TUI_SANITIZER,
  TuiAlertModule,
  TuiButtonModule,
  TuiDialogModule,
  TuiLinkModule,
  TuiModeModule,
  TuiRootModule,
  TuiSvgModule
} from "@taiga-ui/core";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {provideHttpClient} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {CounterComponent} from './counter/counter.component';
import {FetchDataComponent} from './fetch-data/fetch-data.component';
import {TuiBarModule} from "@taiga-ui/addon-charts";
import {TuiAvatarLabeledModule, TuiAvatarModule, TuiFallbackSrcModule} from '@taiga-ui/experimental';
import {TuiCarouselModule, TuiIslandModule, TuiPaginationModule, TuiTabsModule} from "@taiga-ui/kit";
import {TuiAppBarModule} from "@taiga-ui/addon-mobile";
import {NgOptimizedImage} from "@angular/common";
import {ServicesStatusComponent} from "./pages/services-status/services-status.component";
import {HomeComponent} from "./pages/home/home.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'counter', component: CounterComponent},
      {path: 'fetch-data', component: FetchDataComponent},
      {path: 'infrastructure-status', component: ServicesStatusComponent},
    ]),
    BrowserAnimationsModule,
    TuiRootModule,
    TuiDialogModule,
    TuiAlertModule,
    TuiBarModule,
    TuiSvgModule,
    TuiTabsModule,
    TuiAppBarModule,
    TuiAvatarModule,
    TuiAvatarLabeledModule,
    TuiFallbackSrcModule,
    NgOptimizedImage,
    TuiCarouselModule,
    TuiButtonModule,
    TuiLinkModule,
    TuiModeModule,
    TuiIslandModule,
    TuiPaginationModule,
    HomeComponent
  ],
  providers: [{provide: TUI_SANITIZER, useClass: NgDompurifySanitizer}, provideHttpClient()],
  bootstrap: [AppComponent]
})
export class AppModule { }
