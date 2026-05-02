import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'app-word',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: './word.html',
  styleUrl: './word.css',
})
export class Word {
  word = input.required<string>();
  add = output<string>();

  private readonly EMOJIS = [
    '🔥',
    '💧',
    '🌿',
    '⚡',
    '🌊',
    '🏔️',
    '🌙',
    '☀️',
    '❄️',
    '🌪️',
    '✨',
    '🌱',
    '🍃',
    '🌍',
    '💎',
    '🔮',
    '⚗️',
    '🧪',
    '🌸',
    '🦋',
  ];
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

  private wordHash = computed(() => {
    let h = 0;
    for (let i = 0; i < this.word().length; i++)
      h = (h * 31 + this.word().charCodeAt(i)) & 0xffffffff;
    return Math.abs(h);
  });

  emoji = computed(() => this.EMOJIS[this.wordHash() % this.EMOJIS.length]);
  glowColor = computed(() => this.COLORS[this.wordHash() % this.COLORS.length]);
}
