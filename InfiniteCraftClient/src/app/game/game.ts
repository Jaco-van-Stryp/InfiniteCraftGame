import { Component, inject, OnInit } from '@angular/core';
import { CombineWordCommand, GetAllWordsResponse, InfiniteCraftGameService } from '../api';
import { FormsModule } from '@angular/forms';
import { Word } from '../word/word';

@Component({
  selector: 'app-game',
  imports: [FormsModule, Word],
  templateUrl: './game.html',
  styleUrl: './game.css',
})
export class Game implements OnInit {
  listOfWords: GetAllWordsResponse[] = {} as GetAllWordsResponse[];

  gameService = inject(InfiniteCraftGameService);

  ngOnInit() {
    this.gameService.getAllWords().subscribe((words) => {
      this.listOfWords = words;
    });
  }

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
