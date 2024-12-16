import { Injectable } from '@angular/core';
import signalR, { HubConnectionBuilder } from '@microsoft/signalr';
import { APPEND_TEXT_MESSAGE, AppendTextMessage, DONE_MESSAGE, CompletedMessage, RECEIVED_MESSAGE, ReceivedMessage, SendMessage, StartMessage } from '../models/chat-real-time';
import { Subject } from 'rxjs/internal/Subject';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../consts/api';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly receivedMessage: Subject<ReceivedMessage> = new Subject<ReceivedMessage>(); 
  private readonly appendTextMessage: Subject<AppendTextMessage> = new Subject<AppendTextMessage>(); 
  private readonly completedMessage: Subject<CompletedMessage> = new Subject<CompletedMessage>(); 
  private hubConnection?: signalR.HubConnection

  receivedMessage$: Observable<ReceivedMessage> = this.receivedMessage.asObservable();
  appendTextMessage$: Observable<AppendTextMessage> = this.appendTextMessage.asObservable();
  completedMessage$: Observable<CompletedMessage> = this.completedMessage.asObservable();

  startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(`${API_BASE_URL}/chat-hub`)
    .build();

    setTimeout(() => {
      this.hubConnection!
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
    }, 1000);
  }

  addChatListeners = () => {
    this.hubConnection!.on(RECEIVED_MESSAGE, (msg: ReceivedMessage) => this.receivedMessage.next(msg));
    this.hubConnection!.on(APPEND_TEXT_MESSAGE, (msg: AppendTextMessage) => this.appendTextMessage.next(msg));
    this.hubConnection!.on(DONE_MESSAGE, (msg: CompletedMessage) => this.completedMessage.next(msg));
  }

  sendMessage = (message: SendMessage) => {
    this.sendMessageToHub('sendmessage', message);
  }

  startMessage = (message: StartMessage) => {
    this.sendMessageToHub('startmessage', message);
  }

  private sendMessageToHub<T>(method: string, content: T): void {
    this.hubConnection!.invoke(method, content).then(() => console.log(`Invoked '${method}' method.`));
  }
}
