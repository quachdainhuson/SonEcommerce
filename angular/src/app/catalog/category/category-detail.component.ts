import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ProductAttributeDto } from '@proxy/product-attributes';
import { attributeTypeOptions } from '@proxy/son-ecommerce/attributes';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '../../shared/services/notification.service';
import { UtilityService } from '../../shared/services/utility.service';
import { ProductCategoriesService, ProductCategoryDto } from '@proxy/product-categories';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-category-detail',
  templateUrl: './category-detail.component.html',
})
export class CategoryDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  btnDisabled = false;
  public form: FormGroup;
  public coverPicture: any

  //Dropdown
  dataTypes: any[] = [];
  selectedEntity = {} as ProductCategoryDto;

  constructor(
    private categoryService: ProductCategoriesService,
    private fb: FormBuilder,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef,
    private utilService: UtilityService,
    private notificationSerivce: NotificationService,
    private sanitizer: DomSanitizer,
    private cd: ChangeDetectorRef,
    private primengConfig: PrimeNGConfig
  ) {}

  validationMessages = {
    
  };

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.primengConfig.zIndex = {
      modal: 700,    // dialog, sidebar
      overlay: 1000,  // dropdown, overlaypanel
      menu: 1000,     // overlay menus
      tooltip: 1100
    };
    this.buildForm();
    this.loadAttributeTypes();
    this.initFormData();
  }


  initFormData() {
    //Load edit data to form
    if (this.utilService.isEmpty(this.config.data?.id) == true) {
      this.getNewSuggestionCode();
      this.toggleBlockUI(false);
    } else {
      this.loadFormDetails(this.config.data?.id);
    }
  }
  getNewSuggestionCode() {
    this.categoryService.getSuggestNewCode()
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response : string) => {
        this.form.patchValue({
          code: response
        
        });
      }
    });
  }
  loadFormDetails(id: string) {
    this.toggleBlockUI(true);
    this.categoryService
      .get(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ProductCategoryDto) => {
          this.selectedEntity = response;
          this.loadCoverPicture(this.selectedEntity.coverPicture);
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  saveChange() {
    this.toggleBlockUI(true);

    if (this.utilService.isEmpty(this.config.data?.id) == true) {
      this.categoryService
        .create(this.form.value)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.toggleBlockUI(false);

            this.ref.close(this.form.value);
          },
          error: err => {
            this.notificationSerivce.showError(err.error.error.message);

            this.toggleBlockUI(false);
          },
        });
    } else {
      this.categoryService
        .update(this.config.data?.id, this.form.value)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.toggleBlockUI(false);
            this.ref.close(this.form.value);
          },
          error: err => {
            this.notificationSerivce.showError(err.error.error.message);
            this.toggleBlockUI(false);
          },
        });
    }
  }

  loadAttributeTypes() {
    attributeTypeOptions.forEach(element => {
      this.dataTypes.push({
        value: element.value,
        label: element.key,
      });
    });
  }

  private buildForm() {
    this.form = this.fb.group({
      name: new FormControl(this.selectedEntity.name, [Validators.required]),
      code: new FormControl(this.selectedEntity.code, [Validators.required]),
      slug: new FormControl(this.selectedEntity.slug),
      seoMetaDescription : new FormControl(this.selectedEntity.seoMetaDescription),
      visibility : new FormControl(this.selectedEntity.visibility || true),
      isActive : new FormControl(this.selectedEntity.isActive || true),
      coverPictureName:new FormControl(this.selectedEntity.coverPicture || null),
      coverPictureContent: new FormControl(null)

      
      
    });
  }

  loadCoverPicture(fileName: string){
    this.categoryService.getCoverPicture(fileName)
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (response: string) => {
        var fileExt = this.selectedEntity.coverPicture?.split('.').pop();
        this.coverPicture = this.sanitizer.bypassSecurityTrustResourceUrl(
          `data:image/${fileExt};base64, ${response}`
        );
      },
    });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
      this.btnDisabled = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
        this.btnDisabled = false;
      }, 1000);
    }
  }
  generateSlug() {
    this.form.controls['slug'].setValue(this.utilService.MakeSeoTitle(this.form.get('name').value));
  }


  onFileChange(event){
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.form.patchValue({
          coverPictureName: file.name,
          coverPictureContent: reader.result,
        });

        // need to run CD since file load runs outside of zone
        this.cd.markForCheck();
      };
    }
  }
}
