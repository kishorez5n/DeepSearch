import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';

import { ICategory } from './category';
import { ISubcategory } from './subcategory';
import { ILink } from './link';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private categoryUrl = 'https://graphserv.azurewebsites.net/api/graph/category';
  private subcategoryUrl = 'https://graphserv.azurewebsites.net/api/graph/category';
  private linkUrl = 'https://graphserv.azurewebsites.net/api/graph/link';

  constructor(private http: HttpClient) { }

  getCategories(): Observable<ICategory[]> {
    return this.http.get<string>(this.categoryUrl).pipe(map(response => {
        const array = JSON.parse(response);
        return array;
    }));
}

  getSubcategories(id: string): Observable<ISubcategory[]> {
    return this.http.get<string>(this.subcategoryUrl + "?name=" + encodeURIComponent(id)).pipe(map(response => {
      const array = JSON.parse(response);
      return array;
    }));
  }

  getLinks(id: string): Observable<ILink[]> {
    return this.http.get<string>(this.linkUrl + "?name=" + encodeURIComponent(id)).pipe(map(response => {
      const array = JSON.parse(response);
      return array;
    }));
  }

 
  private handleError(err: HttpErrorResponse) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
