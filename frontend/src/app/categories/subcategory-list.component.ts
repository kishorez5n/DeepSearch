import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ISubcategory } from './subcategory';
import { ILink } from './link';
import { CategoryService } from './category.service';

@Component({
  templateUrl: './subcategory-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class SubcategoryComponent implements OnInit {
  pageTitle = 'Details';
  errorMessage = '';
  category: string;
  subcategories: ISubcategory[] = [];
  links: ILink[] = [];

  constructor(private route: ActivatedRoute,
    private router: Router,
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('id');
    if (param) {
      const id = param;
      this.category = id;
      this.pageTitle = id;
      this.getSubcategories(id);
      this.getLinks(id);
    }
  }

  getSubcategories(id: string) {
    this.categoryService.getSubcategories(id).subscribe(
        subcategories => {
            this.subcategories = subcategories;
        },
      error => this.errorMessage = <any>error
    );
  }

  getLinks(id: string) {
    this.categoryService.getLinks(id).subscribe(
        links => {
            this.links = links;
        },
      error => this.errorMessage = <any>error
    );
  }

  onBack(): void {
    this.router.navigate(['/categories']);
  }

  goToLink(url: string){
    window.open(url, "_blank");
  }
}
