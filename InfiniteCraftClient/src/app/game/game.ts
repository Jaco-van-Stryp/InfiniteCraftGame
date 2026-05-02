import { Component, inject } from '@angular/core';
import { CombineWordCommand, InfiniteCraftGameService } from '../api';
import { FormsModule } from '@angular/forms';
import { Button } from 'primeng/button';

@Component({
  selector: 'app-game',
  imports: [FormsModule, Button],
  templateUrl: './game.html',
  styleUrl: './game.css',
})
export class Game {
  gameService = inject(InfiniteCraftGameService);
  wordOne: string = '';
  wordTwo: string = '';
  wordCombined: string = '';

  setCombinedWords() {
    const command: CombineWordCommand = {
      wordOne: this.wordOne,
      wordTwo: this.wordTwo,
    };

    this.gameService.combineWord(command).subscribe((response) => {
      console.log(response);
      this.wordCombined = response.wordCombination || '';
    });
  }
}
