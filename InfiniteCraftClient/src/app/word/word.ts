import { Component, input } from '@angular/core';
import { Badge } from 'primeng/badge';

@Component({
  selector: 'app-word',
  imports: [Badge],
  templateUrl: './word.html',
  styleUrl: './word.css',
})
export class Word {
  word = input.required<string>();
  firstDiscovery = input.required<boolean>();
}
