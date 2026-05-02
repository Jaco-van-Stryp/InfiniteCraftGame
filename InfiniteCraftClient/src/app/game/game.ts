import { Component, inject } from '@angular/core';
import { CombineWordCommand, CombineWordResponse, InfiniteCraftGameService } from '../api';
import { FormsModule } from '@angular/forms';
import { Button } from 'primeng/button';
import { faker } from '@faker-js/faker';
import { Word } from '../word/word';

@Component({
  selector: 'app-game',
  imports: [FormsModule, Button, Word],
  templateUrl: './game.html',
  styleUrl: './game.css',
})
export class Game {
  listOfWords: CombineWordResponse[] = [
    {
      wordCombination: faker.word.words(1),
      firstDiscovery: true,
    },
    {
      wordCombination: faker.word.words(1),
      firstDiscovery: false,
    },
    {
      wordCombination: faker.word.words(1),
      firstDiscovery: true,
    },
    {
      wordCombination: faker.word.words(1),
      firstDiscovery: false,
    },
    {
      wordCombination: faker.word.words(1),
      firstDiscovery: true,
    },
    {
      wordCombination: faker.word.words(1),
      firstDiscovery: false,
    },
  ];

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
