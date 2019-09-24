import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ILink } from './link';
import { CategoryService } from './category.service';

@Component({
  templateUrl: './links.component.html',
  styleUrls: ['./category-list.component.css']
})

export class LinkComponent implements OnInit {
  pageTitle: string;
  subcategory: string;
  errorMessage = '';
  links: ILink[] = [];

  constructor(private route: ActivatedRoute,
    private router: Router,
    private categoryService: CategoryService) {
  }

  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('id');
    if (param) {
      const id = param;
      this.pageTitle = id;
      this.subcategory = id;
      this.getLinks(id);
    }
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
    this.router.navigate(['/categories', this.subcategory]);
  }

  goToLink(url: string){
    window.open(url, "_blank");
  }
}
