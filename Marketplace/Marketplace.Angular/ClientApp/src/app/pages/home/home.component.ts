import {ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject} from '@angular/core';
import {ProductModel} from "../../models/products/productModel";
import {TuiCarouselModule, TuiCheckboxModule, TuiIslandModule, TuiTagModule} from "@taiga-ui/kit";
import {ReactiveFormsModule} from "@angular/forms";
import {TuiButtonModule, TuiLoaderModule} from "@taiga-ui/core";
import {TuiTableModule} from "@taiga-ui/addon-table";
import {AsyncPipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {TuiLetModule} from "@taiga-ui/cdk";
import {ServiceStatusPipePipe} from "../../pipes/service-status-pipe.pipe";
import {ProductsService} from "../../services/productsService/products.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  imports: [
    TuiCheckboxModule,
    ReactiveFormsModule,
    TuiButtonModule,
    TuiButtonModule,
    TuiTableModule,
    TuiTagModule,
    NgForOf,
    NgIf,
    TuiLetModule,
    AsyncPipe,
    ServiceStatusPipePipe,
    TuiLoaderModule,
    TuiCarouselModule,
    TuiIslandModule,
    NgOptimizedImage
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true
})

export class HomeComponent {
  constructor(private productsService : ProductsService,
              private cd : ChangeDetectorRef,
              @Inject('BASE_URL') _baseUrl: string) {

  }

  productsOfTheDay: ProductModel[] = [];
  carouselIndex = 0;

  ngOnInit(){
    this.productsService.getProductsOfTheDay().subscribe({
      next: res => {
        this.productsOfTheDay = res;
        this.cd.markForCheck();
      },
      error: err => {

      }
    })
  }


  scrollRight(){
    if (this.carouselIndex + 2 <= this.productsOfTheDay.length)
    {
      this.carouselIndex++
    }
    else {
      this.carouselIndex = 0;
    }
  }

  scrollLeft(){
    if (this.carouselIndex - 1 >= 0)
    {
      this.carouselIndex--
    }
    else {
      this.carouselIndex = this.productsOfTheDay.length - 1;
    }
  }
}
