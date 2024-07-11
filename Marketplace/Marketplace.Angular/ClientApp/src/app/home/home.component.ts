import {ChangeDetectionStrategy, Component, Inject} from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomeComponent {
  constructor(@Inject('BASE_URL') _baseUrl: string) {

  }
  index = 0;

  readonly items = [
    '36 символов отображение тест12345678',
    'задняя запчасть от владика',
    'отвалившийся кусок ангуляра',
    'Кусок моей двенашки',
    'Трансформер из ваз 2112 в спорткар',
    'ВАЗ-2112 без страховки на дорогах',
  ];

  scrollRight(){
    if (this.index + 2 <= this.items.length)
    {
      this.index++
    }
    else {
      this.index = 0;
    }
  }

  scrollLeft(){
    if (this.index - 1 >= 0)
    {
      this.index--
    }
    else {
      this.index = this.items.length - 1;
    }
  }
}
