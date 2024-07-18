import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService) { }

    ngOnInit() {
        this.model = [
            {
                label: 'Trang Chủ',
                items: [
                    { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }
                ]
            },
            {
                label: 'Sản Phẩm',
                items: [
                    { 
                        label: 'Danh Sách Sản Phẩm', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/catalog/product'] , 
                        permission: 'SonEcomAdminCatalog.Product' 
                    },
                    { 
                        label: 'Danh Sách Thuộc Tính', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/catalog/attribute'], 
                        permission: 'SonEcomAdminCatalog.Attribute' 
                    },
                    
                ]
            },
            {
                label: 'Danh Mục Và Nhà Sản Xuất',
                items: [
                    { 
                        label: 'Danh Sách Danh Mục', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/catalog/category'] , 
                        permission: 'SonEcomAdminCatalog.ProductCategory' 
                    },
                    { 
                        label: 'Danh Sách Nhà Sản Xuất', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/catalog/manufacturer'] , 
                        permission: 'SonEcomAdminCatalog.Manufacturer' 
                    },
                    
                ]
            },
            {
                label: 'Hệ Thống',
                items: [
                    { 
                        label: 'Danh Sách Quyền', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/system/role'] , 
                        permission: 'AbpIdentity.Roles'
                    },
                    { 
                        label: 'Danh Sách Nhân Viên', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/system/user'] , 
                        permission: 'AbpIdentity.Users'
                    },
                    { 
                        label: 'Danh Sách Người Dùng', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/system/customer'] 
                    },
                    
                ]
            },
            {
                label: 'Đơn hàng',
                items: [
                    { 
                        label: 'Quản lý đơn hàng', 
                        icon: 'pi pi-fw pi-users', 
                        routerLink: ['/order/order'],
                    },

                ]
            },
        ];
    }
}
