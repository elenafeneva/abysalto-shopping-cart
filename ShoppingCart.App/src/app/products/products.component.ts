import { Component } from '@angular/core';
import { ProductService } from '../services/product/product.service';
import { firstValueFrom } from 'rxjs';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent {
  products: any[] = [];
  totalRecords: number = 0;
  selectedProduct: any;
  displayDetails: boolean = false;

  constructor(private productService: ProductService, private messageService: MessageService) { }

  async getProducts(event: any): Promise<void> {
    let skip = event.first || 0;
    let limit = event.rows || 10;
    let sortField = event.sortField == undefined ? 'id' : event.sortField;
    let sortOrder = event.sortOrder == 1 ? 'asc' : 'desc';

    try {
      const response = await firstValueFrom(this.productService.getProducts(limit, skip, sortField, sortOrder));
      this.products = response.items;
      this.totalRecords = response.total;
    } catch (err) {
      console.error('Error fetching products:', err);
    }
  }

  getSeverity(stock: number): string {
    if (stock > 10) return 'success';
    if (stock > 0) return 'warning';
    return 'danger';
  }

  viewProduct(product: any): void {
    this.selectedProduct = product;
    this.displayDetails = true;
  }

  addProductToCart(product: any): void {
      this.productService.addToCart(product.id).subscribe({
      next: (response) => {
        if (response === true) {
          this.messageService.add({
            severity: 'success',
            summary: 'Confirmed',
            detail: `${product.title} added to cart`
          });
          this.getProducts({ first: 0, rows: 10 }); 
        }
      },
      error: (err) => {
        console.error('Error adding to cart:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Could not add product to cart.'
        });
      }
    });
  }

  addToFavorites(product: any) {
    this.productService.addToFavorites(product.id).subscribe({
      next: (response) => {
        if (response === true) {
          this.messageService.add({
            severity: 'success',
            summary: 'Confirmed',
            detail: `${product.title} added to favorites`
          });
          this.getProducts({ first: 0, rows: 10 }); 
        }
      },
      error: (err) => {
        console.error('Error adding to favorites:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Could not add product to favorites.'
        });
      }
    });
  }
}
