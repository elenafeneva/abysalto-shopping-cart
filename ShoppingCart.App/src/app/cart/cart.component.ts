import { Component } from '@angular/core';
import { CartService } from '../services/cart/cart.service';
import { MessageService } from 'primeng/api';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent {
  cartProducts: any[] = [];

  constructor(private cartService: CartService, private messageService: MessageService) { }

  async ngOnInit() {
    await this.getCartProducts();
  }

  async getCartProducts() {
    try {
      const response: any = await firstValueFrom(this.cartService.getCartProducts());
      this.cartProducts = response.items;
    } catch (err) {
    }
  }

  // Calculate the total for the entire cart
  get totalAmount(): number {
    let total = 0;
    for (const item of this.cartProducts) {
      total += item.price * item.quantity;
    }
    return total;
  }

  removeItem(cartProduct: any) {
    this.cartService.removeCartItem(cartProduct.id).subscribe(async () => {
      this.messageService.add({
        severity: 'success',
        summary: 'Confirmed',
        detail: `${cartProduct.title} removed from cart`
      });
      await this.getCartProducts();
    });
  }
}
