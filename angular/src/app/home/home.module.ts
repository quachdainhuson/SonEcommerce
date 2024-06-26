import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { ChartModule } from 'primeng/chart';
import { DropdownModule } from 'primeng/dropdown';
@NgModule({
  declarations: [HomeComponent],
  imports: [SharedModule, HomeRoutingModule, ChartModule, DropdownModule],
})
export class HomeModule {}
