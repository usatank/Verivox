import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { ElectricityPageComponent } from './electricity-page/electricity-page.component';
import { GasPageComponent } from './gas-page/gas-page.component';
import { InsurancePageComponent } from './insurance-page/insurance-page.component';

export const routes: Routes = [
    { path: '', redirectTo: 'home-page-component', pathMatch: 'full' }, // Redirects empty path to HomeComponent
    { path: 'home-page-component', component: HomePageComponent },
    { path: 'electricity-page-component', component: ElectricityPageComponent},
    { path: 'gas-page-component', component: GasPageComponent},
    { path: 'insurance-page-component', component: InsurancePageComponent},
    { path: '**', redirectTo: 'home-page-component' } // Redirects unknown routes to home
];
