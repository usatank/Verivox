import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { ElectricityPageComponent } from './electricity-page/electricity-page.component';
import { GasPageComponent } from './gas-page/gas-page.component';
import { InsurancePageComponent } from './insurance-page/insurance-page.component';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' }, // Redirects empty path to HomeComponent
    { path: 'home', component: HomePageComponent },
    { path: 'electricity', component: ElectricityPageComponent},
    { path: 'gas', component: GasPageComponent},
    { path: 'insurance', component: InsurancePageComponent},
    { path: '**', redirectTo: 'home' } // Redirects unknown routes to home
];
