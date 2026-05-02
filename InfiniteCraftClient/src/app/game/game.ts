import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputText } from 'primeng/inputtext';
import { CombineWordCommand, GetAllWordsResponse, InfiniteCraftGameService } from '../api';
import { SoundService } from '../sound.service';
import { Word, WordSelection } from '../word/word';

interface CanvasWord {
  instanceId: string;
  word: string;
  emoji: string;
  x: number;
  y: number;
  isNew: boolean;
  isCombining: boolean;
}

interface DragState {
  instanceId: string;
  offsetX: number;
  offsetY: number;
}

@Component({
  selector: 'app-game',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, InputText, Word],
  templateUrl: './game.html',
  styleUrl: './game.css',
})
export class Game implements OnInit {
  allWords = signal<GetAllWordsResponse[]>([]);
  searchQuery = signal('');
  canvasWords = signal<CanvasWord[]>([]);
  combining = signal(false);
  lastDiscovery = signal<string | null>(null);
  localUser: string = '';
  filteredWords = computed(() => {
    const q = this.searchQuery().toLowerCase();
    const seen = new Set<string>();
    const unique = this.allWords().filter((w) => {
      if (seen.has(w.word)) return false;
      seen.add(w.word);
      return true;
    });
    if (!q) return unique;
    return unique.filter((w) => w.word.toLowerCase().includes(q));
  });
  private gameService = inject(InfiniteCraftGameService);
  private sound = inject(SoundService);
  private dragState: DragState | null = null;

  private readonly COLORS = [
    '#ff6b6b',
    '#ffd93d',
    '#6bcb77',
    '#4d96ff',
    '#c77dff',
    '#ff9a3c',
    '#a8edea',
    '#fed6e3',
  ];

  ngOnInit() {
    let localUser = localStorage.getItem('localUserId');
    if (localUser == null) {
      const guid = crypto.randomUUID();
      localStorage.setItem('localUserId', guid);
      localUser = guid;
    }
    this.localUser = localUser;
    this.gameService.getAllWords(this.localUser).subscribe((words) => {
      this.allWords.set(words);
    });
  }

  addToCanvas({ word, emoji }: WordSelection) {
    const id = crypto.randomUUID();
    const x = 80 + Math.random() * 500;
    const y = 60 + Math.random() * 350;
    this.sound.place();
    this.canvasWords.update((words) => [
      ...words,
      { instanceId: id, word, emoji, x, y, isNew: true, isCombining: false },
    ]);
    setTimeout(() => {
      this.canvasWords.update((words) =>
        words.map((w) => (w.instanceId === id ? { ...w, isNew: false } : w)),
      );
    }, 600);
  }

  onMouseDown(event: MouseEvent, instanceId: string) {
    event.preventDefault();
    const chipEl = event.currentTarget as HTMLElement;
    const canvasEl = chipEl.closest('.canvas') as HTMLElement;
    if (!canvasEl) return;
    const canvasRect = canvasEl.getBoundingClientRect();
    const word = this.canvasWords().find((w) => w.instanceId === instanceId);
    if (!word) return;
    this.dragState = {
      instanceId,
      offsetX: event.clientX - canvasRect.left - word.x,
      offsetY: event.clientY - canvasRect.top - word.y,
    };
  }

  onMouseMove(event: MouseEvent) {
    if (!this.dragState) return;
    const canvas = event.currentTarget as HTMLElement;
    const rect = canvas.getBoundingClientRect();
    const x = event.clientX - rect.left - this.dragState.offsetX;
    const y = event.clientY - rect.top - this.dragState.offsetY;
    const id = this.dragState.instanceId;
    this.canvasWords.update((words) =>
      words.map((w) => (w.instanceId === id ? { ...w, x, y } : w)),
    );
  }

  onMouseUp() {
    if (!this.dragState) return;
    const droppedId = this.dragState.instanceId;
    this.dragState = null;

    if (this.combining()) return;

    const words = this.canvasWords();
    const dropped = words.find((w) => w.instanceId === droppedId);
    if (!dropped) return;

    const target = words.find((w) => {
      if (w.instanceId === droppedId || w.isCombining) return false;
      const dx = w.x - dropped.x;
      const dy = w.y - dropped.y;
      return Math.sqrt(dx * dx + dy * dy) < 90;
    });

    if (target) {
      this.combine(dropped, target);
    }
  }

  stopDrag() {
    this.dragState = null;
  }

  combine(wordA: CanvasWord, wordB: CanvasWord) {
    this.sound.combining();
    this.combining.set(true);
    this.canvasWords.update((words) =>
      words.map((w) =>
        w.instanceId === wordA.instanceId || w.instanceId === wordB.instanceId
          ? { ...w, isCombining: true }
          : w,
      ),
    );

    const command: CombineWordCommand = {
      wordOne: wordA.word,
      wordTwo: wordB.word,
      userId: this.localUser,
    };
    this.gameService.combineWord(command).subscribe((response) => {
      const combined = response.wordCombination ?? '???';
      const emoji = response.emoji ?? '✨';
      const midX = (wordA.x + wordB.x) / 2;
      const midY = (wordA.y + wordB.y) / 2;
      const newId = crypto.randomUUID();

      this.canvasWords.update((words) => {
        const filtered = words.filter(
          (w) => w.instanceId !== wordA.instanceId && w.instanceId !== wordB.instanceId,
        );
        return [
          ...filtered,
          {
            instanceId: newId,
            word: combined,
            emoji,
            x: midX,
            y: midY,
            isNew: true,
            isCombining: false,
          },
        ];
      });

      const isNewWord = !this.allWords().some((w) => w.word === combined);
      if (isNewWord) {
        this.allWords.update((ws) => [...ws, { id: crypto.randomUUID(), word: combined, emoji }]);
      }

      if (response.firstDiscovery && isNewWord) {
        this.sound.discovery();
        this.lastDiscovery.set(combined);
        setTimeout(() => this.lastDiscovery.set(null), 3500);
      } else {
        this.sound.result();
      }

      this.combining.set(false);

      setTimeout(() => {
        this.canvasWords.update((words) =>
          words.map((w) => (w.instanceId === newId ? { ...w, isNew: false } : w)),
        );
      }, 600);
    });
  }

  removeFromCanvas(instanceId: string) {
    this.canvasWords.update((words) => words.filter((w) => w.instanceId !== instanceId));
  }

  getGlowColor(word: string): string {
    let h = 0;
    for (let i = 0; i < word.length; i++) h = (h * 31 + word.charCodeAt(i)) & 0xffffffff;
    return this.COLORS[Math.abs(h) % this.COLORS.length];
  }
}
