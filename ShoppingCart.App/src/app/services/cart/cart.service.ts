import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private cartQuantitySubject = new BehaviorSubject<number>(0);
  cartQuantity$ = this.cartQuantitySubject.asObservable();

  constructor(private http: HttpClient) { }

  getCartProducts(): Observable<any> {
      return this.http.get<any>(`${environment.apiBase}/cart`, {});
  }

  updateCartQuantityFromApi(): void {
    this.getCartProducts().subscribe((response: any) => {
      const items = response.items || [];
      const quantity = items.reduce((sum: number, item: any) => sum + (item.quantity || 0), 0);
      this.cartQuantitySubject.next(quantity);
    });
  }

  removeCartItem(productId: number): Observable<any> {
    return this.http.delete(`${environment.apiBase}/cart/${productId}`);
  }
}
