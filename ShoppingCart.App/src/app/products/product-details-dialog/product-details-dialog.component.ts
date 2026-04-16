import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ProductService } from '../../services/product/product.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-product-details-dialog',
  templateUrl: './product-details-dialog.component.html',
  styleUrls: ['./product-details-dialog.component.scss']
})

export class ProductDetailsDialogComponent {
  @Input() visible: boolean = false;
  @Input() productId: any;
  product: any;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() onAddToCart = new EventEmitter<any>();

  constructor(private productService: ProductService) { }

  async ngOnInit(): Promise<void> {
    await this.getProductById(this.productId);
  }

  async getProductById(productId: number) {
    try {
      const response = await firstValueFrom(this.productService.getProductById(productId));
      this.product = response.product;
    } catch (err) {
      console.error('Error fetching products:', err);
    }
  }

  closeDialog() {
    this.visible = false;
    this.visibleChange.emit(this.visible);
  }

  addToCart() {
    //this.onAddToCart.emit(this.product);
    this.closeDialog();
  }
}
