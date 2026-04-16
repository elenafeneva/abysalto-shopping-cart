import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  userRefresh = new Subject<void>();

   constructor(private http: HttpClient) { }
 
   getCurrentUser() {
     return this.http.get<any>(`${environment.apiBase}/auth/currentuser`);
   }
}
