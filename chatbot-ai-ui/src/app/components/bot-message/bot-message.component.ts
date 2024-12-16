import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Message, Reaction } from '../../models/messages';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../../services/signalr.service';
import { AppendTextMessage, CompletedMessage } from '../../models/chat-real-time';
import { API_BASE_URL } from '../../consts/api';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-bot-message',
  imports: [MatIconModule],
  templateUrl: './bot-message.component.html',
  styleUrl: './bot-message.component.scss',
  standalone: true
})
export class BotMessageComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];

  @Input() message?: Message;

  constructor(private httpClient: HttpClient, private signalRService: SignalrService) {
  }

  ngOnInit(): void {
    if (!this.message!.isCompleted) {
      this.subscriptions.push(this.signalRService.completedMessage$.subscribe((msg: CompletedMessage) => {
        if (msg.messageId === this.message?.id) this.message!.isCompleted = true;
      }));

      this.subscriptions.push(this.signalRService.appendTextMessage$.subscribe((msg: AppendTextMessage) => {
        if (msg.messageId != this.message?.id || this.message!.content!.parts.includes(msg.partId!)) return;
        
        this.message!.content!.parts.push(msg.partId!);
        this.message!.content!.text! += msg.text;
      }));
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  cancel() {
    this.httpClient.put(`${API_BASE_URL}/messages/${this.message?.id}/cancel`, {}).subscribe(x => {
      this.message!.isCompleted = true;
    });
  }

  thumbsUp() {
    this.react(Reaction.ThumbsUp);
  }

  thumbsDown() {
    this.react(Reaction.ThumbsDown);
  }

  reset() {
    this.react(undefined);
  }

  isUpvoted(): boolean {
    return this.message!.content!.reaction === Reaction.ThumbsUp;
  }

  isDownvoted(): boolean {
    return this.message!.content!.reaction === Reaction.ThumbsDown;
  }

  private react(reaction: Reaction | undefined) {
    this.httpClient.put(`${API_BASE_URL}/messages/${this.message?.id}/react`, { reaction: reaction }).subscribe(x => {
      this.message!.content!.reaction = reaction;
    });
  }
}
