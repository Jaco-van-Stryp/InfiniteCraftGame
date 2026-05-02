import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SoundService {
  private ctx: AudioContext | null = null;

  private get audioCtx(): AudioContext {
    if (!this.ctx) this.ctx = new AudioContext();
    if (this.ctx.state === 'suspended') this.ctx.resume();
    return this.ctx;
  }

  /** Short pop when a word is placed on the canvas */
  place() {
    this.safe((ctx) => {
      const osc = ctx.createOscillator();
      const gain = ctx.createGain();
      osc.connect(gain);
      gain.connect(ctx.destination);
      osc.type = 'sine';
      osc.frequency.setValueAtTime(520, ctx.currentTime);
      osc.frequency.exponentialRampToValueAtTime(260, ctx.currentTime + 0.08);
      gain.gain.setValueAtTime(0.12, ctx.currentTime);
      gain.gain.exponentialRampToValueAtTime(0.0001, ctx.currentTime + 0.12);
      osc.start(ctx.currentTime);
      osc.stop(ctx.currentTime + 0.12);
    });
  }

  /** Soft pulsing hum when combination begins */
  combining() {
    this.safe((ctx) => {
      this.tone(ctx, 330, ctx.currentTime, 0.35, 0.07);
      this.tone(ctx, 440, ctx.currentTime + 0.08, 0.35, 0.07);
    });
  }

  /** Pleasant two-note chime when a result appears */
  result() {
    this.safe((ctx) => {
      this.tone(ctx, 523, ctx.currentTime, 0.45, 0.18);
      this.tone(ctx, 659, ctx.currentTime + 0.07, 0.45, 0.15);
    });
  }

  /** Ascending four-note arpeggio for first discovery */
  discovery() {
    this.safe((ctx) => {
      [523, 659, 784, 1047].forEach((freq, i) => {
        this.tone(ctx, freq, ctx.currentTime + i * 0.1, 0.5, 0.16);
      });
    });
  }

  private safe(fn: (ctx: AudioContext) => void) {
    try {
      fn(this.audioCtx);
    } catch {
      /* silently ignore if audio unavailable */
    }
  }

  private tone(
    ctx: AudioContext,
    freq: number,
    startAt: number,
    duration: number,
    volume = 0.15,
    type: OscillatorType = 'sine',
  ) {
    const osc = ctx.createOscillator();
    const gain = ctx.createGain();
    osc.connect(gain);
    gain.connect(ctx.destination);
    osc.type = type;
    osc.frequency.value = freq;
    gain.gain.setValueAtTime(volume, startAt);
    gain.gain.exponentialRampToValueAtTime(0.0001, startAt + duration);
    osc.start(startAt);
    osc.stop(startAt + duration);
  }
}
