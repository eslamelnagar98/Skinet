import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IPagination } from './models/pagination';
import { IProduct } from './models/Product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  title = 'Skinet';
  products: Array<IProduct>;
  constructor(private http: HttpClient) {

  }
  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/products?pageSize=50').subscribe(
      {
        next: (response: IPagination) => {
          this.products = response.data
          console.log(response.count)
        },
        error: (error: any) => console.error(error)
      }
    )
  }
}
