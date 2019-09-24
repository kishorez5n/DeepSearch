import { Component, OnInit } from '@angular/core';

import { ICategory } from './category';
import { CategoryService} from './category.service';

@Component({
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
    pageTitle = 'Categories';
    imageWidth = 50;
    imageMargin = 2;
    showImage = false;
    errorMessage = '';
  
    filteredCategories: ICategory[] = [];
    categories: ICategory[] = [];

    constructor(private categoryService: CategoryService) {
  
    }
  
    ngOnInit(): void {
      this.categoryService.getCategories().subscribe(
        categories => {
          this.categories = categories;
          this.filteredCategories = this.categories;
        },
        error => this.errorMessage = <any>error
      );
    }
  }