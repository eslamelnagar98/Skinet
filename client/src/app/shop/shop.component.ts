import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/Product';
import { IType } from '../shared/models/ProductType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';
@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  @ViewChild('search', { static: true }) SearchTerm: ElementRef
  products: Array<IProduct>;
  brands: Array<IBrand>;
  types: Array<IType>;
  shopParams = new ShopParams();
  totalCount: number;
  sortOptions = [
    { 'name': 'Alphabetical', value: 'name' },
    { 'name': 'Price: Low To High', value: 'priceAsc' },
    { 'name': 'Price: High To Low', value: 'priceDesc' },
  ];
  constructor(private shopService: ShopService) { }

  ngOnInit() {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response: IPagination) => {
        this.products = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error) => console.error(error)
    }
    )
  }

  getBrands() {

    this.shopService.getBrands().subscribe({
      next: (response) => this.brands = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.error(error)
    })
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) => this.types = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.error(error)
    })
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.resetPageNumber();
    this.getProducts();
  }
  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.resetPageNumber();
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.resetPageNumber();
    this.getProducts();
  }

  onPageChanged(event: any) {
    if (this.shopParams.pageNumber != event) {
      this.shopParams.pageNumber = event;
      this.getProducts();
    }

  }

  onSearch() {
    this.shopParams.search = this.SearchTerm.nativeElement.value;
    this.resetPageNumber();
    this.getProducts();
  }

  onReset() {
    this.SearchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }

  resetPageNumber() {
    this.shopParams.pageNumber = 1;
  }



}
