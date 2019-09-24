import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from "@angular/common";

import { CategoryListComponent } from './category-list.component';
import { SubcategoryComponent } from './subcategory-list.component';
import { LinkComponent } from './links.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      { path: 'categories', component: CategoryListComponent },
      {
        path: 'categories/:id',
        component: SubcategoryComponent
      },
      {
        path: 'subcategories/:id',
        component: LinkComponent
      }
    ]),
    CommonModule
  ],
  declarations: [
    CategoryListComponent,
    SubcategoryComponent,
    LinkComponent
  ]
})
export class CategoryModule { }