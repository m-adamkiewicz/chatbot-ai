import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, Subscription } from 'rxjs';
import { Author, Message } from '../../models/messages';
import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../../services/signalr.service';
import { API_BASE_URL } from '../../consts/api';
import { SendMessage, StartMessage } from '../../models/chat-real-time';
import { HumanMessageComponent } from '../human-message/human-message.component';
import { BotMessageComponent } from '../bot-message/bot-message.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-chat',
  imports: [FormsModule, CommonModule, MatInputModule, MatIconModule, BotMessageComponent, HumanMessageComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
  standalone: true
})
export class ChatComponent implements OnInit, OnDestroy {
  private readonly subscriptions: Subscription[] = [];
  history?: Message[];
  currentMessage?: Message;
  currentResponse?: Message;

  message: string = '';

  constructor(private httpClient: HttpClient, private signalRService: SignalrService) {
  }

  ngOnInit(): void {
    this.initializeSignalR();

    this.httpClient.get<Message[]>(`${API_BASE_URL}/messages`, { responseType: 'json' })
      .pipe(map((msg) => {
        msg.forEach(x => x.isCompleted = true);
        return msg;
      }))
      .subscribe(x => {
        this.history = x;
        this.scrollToCurrentMessage();
      });

    this.subscriptions.push(this.signalRService.receivedMessage$.subscribe(msg => {
      const message: Message = Message.create(msg.messageId!, Author.Human, msg.createdAt!, this.message, true);
      const response: Message = Message.create(msg.responseId!, Author.AI, msg.responseCreatedAt!, '', false);

      if (this.currentMessage !== undefined) {
        this.history!.push(this.currentMessage);
      }

      if (this.currentResponse !== undefined) {
        this.history!.push(this.currentResponse);
      }

      this.currentMessage = message;
      this.currentResponse = response;

      this.history = this.history;
      this.message = '';

      this.scrollToCurrentMessage();

      this.startMessage(message.id!, response.id!);
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  startMessage(messageId: string, responseId: string) {
    const message: StartMessage = { messageId: messageId, responseId: responseId };
    this.signalRService.startMessage(message);
  }

  sendMessage(): void {
    const message: SendMessage = { text: this.message };
    this.signalRService.sendMessage(message);
  }

  isHuman(msg: Message): boolean {
    return msg.author === Author.Human;
  }

  isSendDisabled(): boolean {
    return this.message.trim() === '' || this.currentResponse?.isCompleted === false;
  }

  private initializeSignalR(): void {
    this.signalRService.startConnection();
    this.signalRService.addChatListeners();
  }

  private scrollToCurrentMessage(): void {
    setTimeout(() => {
      document?.getElementById('current')?.scrollIntoView();
    }, 10)
  }
}