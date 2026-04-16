import { Component } from '@angular/core';
import { firstValueFrom, Subscription } from 'rxjs';
import { UserService } from './services/user/user.service';
import { Router } from '@angular/router';
import { AuthService } from './services/auth/auth.service';
import { CartService } from './services/cart/cart.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Abysalto Shopping Cart';
  user: any;
  cartQuantity: number = 0;
  private cartQuantitySub?: Subscription;

  constructor(private userService: UserService, private router: Router, public authService: AuthService, private cartService: CartService) {
    // Listen for user refresh events
    this.userService.userRefresh.subscribe(() => {
      this.getCurrentUser();
    });
  }

  async ngOnInit(): Promise<void> {
    await this.getCurrentUser();
    this.cartService.updateCartQuantityFromApi();
    this.cartQuantitySub = this.cartService.cartQuantity$.subscribe(q => this.cartQuantity = q);
  }

  async getCurrentUser(): Promise<void> {
    try {
      const response = await firstValueFrom(this.userService.getCurrentUser());
      this.user = response.user;
    } catch (err) {
      console.error('Error fetching current user:', err);
    }
  }

  logOut(): void {
    this.authService.removeToken();
    this.user = null;
    if (this.cartQuantitySub) this.cartQuantitySub.unsubscribe();
    this.router.navigate(['/sign-in']);
  }
}

