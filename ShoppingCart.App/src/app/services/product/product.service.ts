import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private http: HttpClient) { }

  getProducts(limit: number, skip: number, sortField: string, sortOrder: string) {
    const params = new HttpParams()
      .set('limit', limit.toString())
      .set('skip', skip.toString())
      .set('sortField', sortField)
      .set('sortOrder', sortOrder);

    return this.http.get<any>(`${environment.apiBase}/products`, { params });
  }

  getProductById(productId: number) {
    return this.http.get<any>(`${environment.apiBase}/products/${productId}`);
  }

  addToFavorites(productId: number): Observable<boolean> {
    return this.http.post<any>(`${environment.apiBase}/products/${productId}/favorites`, {}).pipe(
      map(response => {
        return response.favoriteProductCreated;
      }),
      catchError((err) => {
        console.error('Error adding to favorites:', err);
        return of(false);
      })
    );
  }

  addToCart(productId: any) {
    var quantity = 1;
    return this.http.post<any>(`${environment.apiBase}/cart`, { productId, quantity }).pipe(
      map(response => {
        return response.cartItemCreated;
      }),
      catchError((err) => {
        console.error('Error adding to cart:', err);
        return of(false);
      })
    );
  }
}
