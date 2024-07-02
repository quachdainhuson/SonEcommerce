import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleComponent } from './role/role.component';
import { UserComponent } from './user/user.component';
import { PermissionGuard, permissionGuard } from '@abp/ng.core';
import { CustomerComponent } from './Customer/customer.component';

const routes: Routes = [
{ path: 'role', 
  component: RoleComponent, 
  canActivate: [permissionGuard], 
  data: 
  {
    requirePolicy: 'AbpIdentity.Roles'
  } 
},
{ path: 'user', 
component: UserComponent, 
canActivate: [permissionGuard], 
data: 
{
  requirePolicy: 'AbpIdentity.Users'
} 
},
{ path: 'customer', 
  component: CustomerComponent,
  canActivate: [permissionGuard],
  data: 
  {
    requirePolicy: 'AbpIdentity.Users'
  
  },
},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SystemRoutingModule {}
