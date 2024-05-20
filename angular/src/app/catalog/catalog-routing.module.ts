import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { AttributeComponent } from './attributes/attribute.component';
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  { path: 'product', 
  component: ProductComponent, 
  canActivate: [permissionGuard], 
  data: 
  {
    requirePolicy: 'SonEcomAdminCatalog.Product',
  } 
},
{ path: 'attribute', 
  component: AttributeComponent, 
  canActivate: [permissionGuard], 
  data: 
  {
    requirePolicy: 'SonEcomAdminCatalog.Attribute',
  } 
},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}
