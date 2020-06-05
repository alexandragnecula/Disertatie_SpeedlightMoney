import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MaterialModule } from '../material.module';
import { LayoutModule } from '@angular/cdk/layout';
import { PAGESROUTES } from './pages-routing';



@NgModule({
  declarations: [],
  imports: [
    PAGESROUTES,
    LayoutModule,
    CommonModule,
    MaterialModule
  ]
})
export class PagesModule { }
